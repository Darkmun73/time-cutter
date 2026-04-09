using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(DirectionsController))]
[RequireComponent(typeof(KnockbackController))]
public class PlayerController : MonoBehaviour
{
    private Player player;
    private KnockbackController knockbackController;
    private Movement movement;
    private DirectionsController directions;

    [SerializeField] private InputReader inputReader;

    private bool isMovingByInput = false;

    void Awake()
    {
        player = GetComponent<Player>();
        movement = GetComponent<Movement>();
        directions = GetComponent<DirectionsController>();
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
                movement.AdjustHorizontalSpeed(directions.MovementDirection, 2f);
        }
        else
        {
            if (isMovingByInput)
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
        isMovingByInput = horizontal != 0;
    }
}
