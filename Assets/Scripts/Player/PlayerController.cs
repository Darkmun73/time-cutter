using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rigidBody;

    [SerializeField] private InputReader inputReader;

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
        if ((player.IsFacingRight && horizontalMove < 0) || 
            (!player.IsFacingRight && horizontalMove > 0))
        {
            player.ChangeDirection();
        }

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
