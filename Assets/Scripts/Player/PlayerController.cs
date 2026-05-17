using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(DirectionsController))]
[RequireComponent(typeof(KnockbackController))]
[RequireComponent(typeof(PlayerCombat))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    private Player player;
    private KnockbackController knockbackController;
    private Movement movement;
    private DirectionsController directions;
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
        player = GetComponent<Player>();
        movement = GetComponent<Movement>();
        directions = GetComponent<DirectionsController>();
        knockbackController = GetComponent<KnockbackController>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    void OnEnable()
    {
        inputReader.MovingAndLooking += OnDirectionInput;
        inputReader.Jumping += movement.Jump;

        player.HitReceived += knockbackController.Knockback;
    }

    void OnDisable()
    {
        inputReader.MovingAndLooking -= OnDirectionInput;
        inputReader.Jumping -= movement.Jump;
        
        player.HitReceived -= knockbackController.Knockback;
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

    private void OnDirectionInput(Vector2 values)
    {
        float horizontal = values.x, vertical = values.y;
        directions.SetUpDirections(horizontal, vertical);
        shouldRun = horizontal != 0;
    }
}
