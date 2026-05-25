using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HitReceiver))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(DirectionsController))]
[RequireComponent(typeof(KnockbackController))]
[RequireComponent(typeof(PlayerCombat))]
[RequireComponent(typeof(PlayerLifecycleHandler))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    private HitReceiver hitReceiver;
    private Movement movement;
    private Health health;
    private PlayerLifecycleHandler lifecycleHandler;
    private DirectionsController directions;
    private KnockbackController knockbackController;
    private PlayerCombat playerCombat;

    public event UnityAction RunningStarted;
    public event UnityAction RunningStopped;

    private bool shouldRun = false;

    private bool isRunning = false;
    private bool IsRunning
    {
        get => isRunning;
        set
        {
            if (isRunning != value)
            {
                if (value)
                    RunningStarted?.Invoke();
                else
                    RunningStopped?.Invoke();
            }
            isRunning = value;
        }
    }

    void Awake()
    {
        hitReceiver = GetComponent<HitReceiver>();
        movement = GetComponent<Movement>();
        health = GetComponent<Health>();
        lifecycleHandler = GetComponent<PlayerLifecycleHandler>();
        directions = GetComponent<DirectionsController>();
        knockbackController = GetComponent<KnockbackController>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    void OnEnable()
    {
        inputReader.MovingAndLooking += OnDirectionInput;
        inputReader.Jumping += movement.TryJump;
        hitReceiver.HitReceived += OnHitReceived;
        knockbackController.KnockedBack += ProhibitAttackWhileKnockedback;
        movement.GroundLeft += playerCombat.ProhibitAttack;
        movement.GroundTouched += playerCombat.AllowAttack;
        playerCombat.AttackStarted += ProhibitJumpWhileAttacking;
        health.HealthDepleted += lifecycleHandler.Die;
    }

    void OnDisable()
    {
        inputReader.MovingAndLooking -= OnDirectionInput;
        inputReader.Jumping -= movement.TryJump;
        hitReceiver.HitReceived -= OnHitReceived;
        knockbackController.KnockedBack -= ProhibitAttackWhileKnockedback;
        movement.GroundLeft -= playerCombat.ProhibitAttack;
        movement.GroundTouched -= playerCombat.AllowAttack;
        playerCombat.AttackStarted -= ProhibitJumpWhileAttacking;
        health.HealthDepleted -= lifecycleHandler.Die;
    }

    void Update()
    {
        if (!shouldRun || (playerCombat.IsAttacking && shouldRun))
            IsRunning = false;
        else if (shouldRun)
            IsRunning = true;
    }

    void FixedUpdate()
    {
        // if (knockbackController.IsKnockedBack) //&& (rigidBody.linearVelocityX > player.Data.MovementSpeed || rigidBody.linearVelocityX < -player.Data.MovementSpeed) )
        // {
        //     if (IsRunning)
        //         movement.AdjustHorizontalSpeed(directions.MovementDirection, 2f);
        // }
        // else
        if (!knockbackController.IsKnockedBack)
        {
            if (IsRunning)
                movement.MoveHorizontal(directions.MovementDirection);
            else
                movement.StopHorizontalMovement();
        }
            
        movement.ClampFallSpeed();
    }

    public void Reset()
    {
        IsRunning = false;
        shouldRun = false;
    }

    private void OnDirectionInput(Vector2 values)
    {
        float horizontal = values.x, vertical = values.y;
        directions.SetUpDirections(horizontal, vertical);
        shouldRun = horizontal != 0;
    }

    private void OnHitReceived(HitInfo hitInfo)
    {
        health.TakeDamage(hitInfo.Damage);
        knockbackController.Knockback(hitInfo.Source);
    }

    private void ProhibitAttackWhileKnockedback()
    {
        float duration = knockbackController.GetDuration();
        playerCombat.ProhibitAttack(duration);
    }

    private void ProhibitJumpWhileAttacking(PlayerCombat.AttackInfo attackInfo)
    {
        float duration = playerCombat.GetAttackDuration();
        movement.ProhibitJump(duration);
    }
}
