using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(KnockbackController))]
public class PlayerController : MonoBehaviour
{
    private Player player;
    private KnockbackController knockbackController;

    private Movement movement;
    [SerializeField] private InputReader inputReader;

    private bool isMovingByInput = false;

    void Awake()
    {
        player = GetComponent<Player>();
        movement = GetComponent<Movement>();
        knockbackController = GetComponent<KnockbackController>();

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

    void FixedUpdate()
    {
        if (knockbackController.IsKnockedBack) //&& (rigidBody.linearVelocityX > player.Data.MovementSpeed || rigidBody.linearVelocityX < -player.Data.MovementSpeed) )
        {
            if (isMovingByInput)
                movement.AdjustHorizontalSpeed(2f);
        }
        else
        {
            if (isMovingByInput)
                movement.MoveHorizontal();
            else
                movement.StopHorizontalMovement();
        }
            
        movement.ClampFallSpeed();
    }
    
    private void OnDirectionInput(Vector2 values)
    {
        float horizontal = values.x, vertical = values.y;
        movement.SetUpDirections(horizontal, vertical);
        isMovingByInput = horizontal != 0;
    }
}
