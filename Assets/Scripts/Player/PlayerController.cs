using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rigidBody;

    [SerializeField] private InputReader inputReader;

    private float horizontalMove;
    private float currentKnockbackTime = 0f;
    private Vector2 currentKnockbackVector = Vector2.zero;
    //private float currentKnockbackHorizontalForce = 0f;
    private float knockbackSubstractionCoef; // For linear knockback

    void Awake()
    {
        player = GetComponent<Player>();
        rigidBody = GetComponent<Rigidbody2D>();

        knockbackSubstractionCoef = Time.fixedDeltaTime * player.Data.KnockbackForce / player.Data.KnockbackDuration;
        Debug.Log(knockbackSubstractionCoef);
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

        if (currentKnockbackTime > 0 && (rigidBody.linearVelocityX > 0.3 || rigidBody.linearVelocityX < -0.3) )
        {
            //currentKnockbackHorizontalForce = 
            //float knockbackXSubValue = knockbackSubstractionCoef * currentKnockbackVector.x;
            //float knockbackYSubValue = knockbackSubstractionCoef * currentKnockbackVector.y;
            rigidBody.linearVelocityX -= rigidBody.linearVelocityX > 0 ? knockbackSubstractionCoef : -knockbackSubstractionCoef;
            rigidBody.linearVelocityX += horizontalMove;// * knockbackSubstractionCoef;
            //rigidBody.linearVelocityY -= rigidBody.linearVelocityY > 0 ? knockbackSubstractionCoef : -knockbackSubstractionCoef;
            Debug.Log(rigidBody.linearVelocity);
            currentKnockbackTime -= Time.fixedDeltaTime;
        }
        else
        {
            rigidBody.linearVelocityX = horizontalMove * player.Data.MovementSpeed;
        }
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

    public void Knockback(Vector3 source)
    {
        Vector2 knockbackVector = (transform.position - source).normalized;
        Debug.Log(knockbackVector);
        rigidBody.linearVelocityX = knockbackVector.x * player.Data.KnockbackForce;
        float g = Mathf.Abs(Physics2D.gravity.y * rigidBody.gravityScale);
        rigidBody.linearVelocityY = knockbackVector.y * Mathf.Sqrt(g * player.Data.KnockbackForce * player.Data.KnockbackDuration);//knockbackVector.x * player.Data.KnockbackForce;
        //rigidBody.linearVelocityY += knockbackVector.y * rigidBody.gravityScale;
        Debug.Log(rigidBody.linearVelocity);

        currentKnockbackTime = player.Data.KnockbackDuration;
        currentKnockbackVector = knockbackVector;
        //currentKnockbackHorizontalForce = rigidBody.linearVelocityX;
    }
}
