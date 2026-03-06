using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rigidBody;

    [SerializeField] private InputReader inputReader;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float maxSpeed;

    private float horizontalMove;

    void Awake()
    {
        player = GetComponent<Player>();
        rigidBody = GetComponent<Rigidbody2D>();

    }

    void OnEnable()
    {
        inputReader.Moving += SetHorizontalMove;
        inputReader.Jumping += Jump;
    }

    void OnDisable()
    {
        inputReader.Moving -= SetHorizontalMove;
        inputReader.Jumping -= Jump;
    }

    void FixedUpdate()
    {
        if ((player.MoveDirection == Direction.Right && horizontalMove < 0) || 
            (player.MoveDirection == Direction.Left && horizontalMove > 0))
        {
            player.ChangeMoveDirection();
        }

        // if (horizontalMove != 0)
        // {
        //     rigidBody.linearDamping = 0;
        //     rigidBody.AddForce(new(horizontalMove * acceleration, 0), ForceMode2D.Force);

        //     if (Mathf.Abs(rigidBody.linearVelocityX) > maxSpeed) // возможно поменять на магнитуду
        //     {
        //         rigidBody.linearVelocityX = maxSpeed * Mathf.Sign(rigidBody.linearVelocityX);
        //     }
        // }  else
        // {
        //     rigidBody.linearDamping = 10;
        //     //rigidBody.AddForce(new(rigidBody.linearVelocityX * -deceleration, 0), ForceMode2D.Force);
        // }

        if (player is IKnockbackable knockbackObj && knockbackObj.IsKnockedBack) //&& (rigidBody.linearVelocityX > player.Data.MovementSpeed || rigidBody.linearVelocityX < -player.Data.MovementSpeed) )
            rigidBody.linearVelocityX += horizontalMove * 2f;// * knockbackSubstractionCoef;
        else
            rigidBody.linearVelocityX = horizontalMove * player.Data.MovementSpeed;
            
        if (rigidBody.linearVelocityY < -player.Data.MaxFallSpeed)
            rigidBody.linearVelocityY = -player.Data.MaxFallSpeed;
        
    }

    private void SetHorizontalMove(float value)
    {
        horizontalMove = value;
    }

    private void Jump()
    {
        if (player.IsTouchingGround)
            rigidBody.linearVelocityY = player.JumpForce;
    }
}
