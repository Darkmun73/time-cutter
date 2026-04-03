using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rigidBody;
    private KnockbackController knockbackController;

    [SerializeField] private InputReader inputReader;

    private float horizontalMove;

    void Awake()
    {
        player = GetComponent<Player>();
        rigidBody = GetComponent<Rigidbody2D>();
        knockbackController = GetComponent<KnockbackController>();

    }

    void OnEnable()
    {
        inputReader.MovingAndLooking += SetMoveAndLook;
        inputReader.Jumping += Jump;

        if (knockbackController != null)
            player.events.GetHitted += knockbackController.Knockback;
    }

    void OnDisable()
    {
        inputReader.MovingAndLooking -= SetMoveAndLook;
        inputReader.Jumping -= Jump;
        
        if (knockbackController != null)
            player.events.GetHitted -= knockbackController.Knockback;
    }

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, player.LookDirection.ToVector() * 5f, Color.yellow);

        if (knockbackController != null && knockbackController.IsKnockedBack) //&& (rigidBody.linearVelocityX > player.Data.MovementSpeed || rigidBody.linearVelocityX < -player.Data.MovementSpeed) )
            rigidBody.linearVelocityX += horizontalMove * 2f;// * knockbackSubstractionCoef;
        else
            rigidBody.linearVelocityX = horizontalMove * player.Data.MovementSpeed;
            
        if (rigidBody.linearVelocityY < -player.Data.MaxFallSpeed)
            rigidBody.linearVelocityY = -player.Data.MaxFallSpeed;
        
    }

    private void SetMoveAndLook(float moveValue, float lookValue)
    {

        horizontalMove = moveValue;
        if (moveValue == 1f)
            player.MoveDirection = Direction.Right;
        else if (moveValue == -1f)
            player.MoveDirection = Direction.Left;
        if (lookValue == 1f)
            player.LookDirection = Direction.Up;
        else if (lookValue == -1f)
            player.LookDirection = Direction.Down;

        if (moveValue == 0f && lookValue == 0f)
            player.LookDirection = player.MoveDirection;
    }
    
    private void SetMoveAndLook(Vector2 values)
    {
        SetMoveAndLook(values.x, values.y);
    }

    private void Jump()
    {
        if (player.IsTouchingGround)
            rigidBody.linearVelocityY = player.JumpForce;
    }
}
