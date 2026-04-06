using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnockbackController : MonoBehaviour
{
    private Rigidbody2D rigidBody; 

    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackDuration = 1f;

    private float currentKnockbackTime = 0f;
    private Vector2 currentKnockbackVector = Vector2.zero;
    //private float currentKnockbackHorizontalForce = 0f;
    private float knockbackSubstractionCoef; // For linear knockback

    public bool IsKnockedBack {get; private set;}

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        knockbackSubstractionCoef = Time.fixedDeltaTime * knockbackForce / knockbackDuration;
        //Debug.Log(knockbackSubstractionCoef);
    }

    void FixedUpdate()
    {
        if (currentKnockbackTime > 0 && (rigidBody.linearVelocityX > 1f || rigidBody.linearVelocityX < -1f) )
        {
            //currentKnockbackHorizontalForce = 
            //float knockbackXSubValue = knockbackSubstractionCoef * currentKnockbackVector.x;
            //float knockbackYSubValue = knockbackSubstractionCoef * currentKnockbackVector.y;
            rigidBody.linearVelocityX -= rigidBody.linearVelocityX > 0 ? knockbackSubstractionCoef : -knockbackSubstractionCoef;
            //rigidBody.linearVelocityY -= rigidBody.linearVelocityY > 0 ? knockbackSubstractionCoef : -knockbackSubstractionCoef;
            currentKnockbackTime -= Time.fixedDeltaTime;
        }
        else
            IsKnockedBack = false;
    }

    public void Knockback(Transform source)
    {
        if (!enabled)
        {
            Debug.Log("Knockback is disabled!");
            return;
        }

        Vector2 knockbackVector = (transform.position - source.position).normalized;
        rigidBody.linearVelocityX = knockbackVector.x * knockbackForce;
        float g = Mathf.Abs(Physics2D.gravity.y * rigidBody.gravityScale);
        rigidBody.linearVelocityY = knockbackVector.y * Mathf.Sqrt(g * knockbackForce * knockbackDuration);//knockbackVector.x * player.Data.KnockbackForce;
        //rigidBody.linearVelocityY += knockbackVector.y * rigidBody.gravityScale;

        currentKnockbackTime = knockbackDuration;
        currentKnockbackVector = knockbackVector;
        //currentKnockbackHorizontalForce = rigidBody.linearVelocityX;

        IsKnockedBack = true;
    }

    // public void Knockback(Vector3 source)
    // {
    //     Vector2 knockbackVector = (transform.position - source).normalized;
    //     //rigidBody.linearVelocityX = knockbackVector.x * knockbackForce;
        
    //     Debug.Log(knockbackVector * knockbackForce);
    //     rigidBody.AddForce(knockbackVector * knockbackForce, ForceMode2D.Impulse);

    //     //IsKnockedBack = true;
    // }
}
