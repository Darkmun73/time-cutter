using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnockbackController : MonoBehaviour
{
    private Rigidbody2D rigidBody; 

    [SerializeField] private KnockbackData data;

    private float currentKnockbackTime = 0f;
    private float knockbackSubstractionCoef; // For linear knockback

    public bool IsKnockedBack {get; private set;}

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        knockbackSubstractionCoef = Time.fixedDeltaTime * data.Force / data.Duration;
        //Debug.Log(knockbackSubstractionCoef);
    }

    void FixedUpdate()
    {
        if (currentKnockbackTime > 0 && (rigidBody.linearVelocityX > 1f || rigidBody.linearVelocityX < -1f) )
        {
            rigidBody.linearVelocityX -= rigidBody.linearVelocityX > 0 ? knockbackSubstractionCoef : -knockbackSubstractionCoef;
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
        rigidBody.linearVelocityX = knockbackVector.x * data.Force;
        float g = Mathf.Abs(Physics2D.gravity.y * rigidBody.gravityScale);
        rigidBody.linearVelocityY = knockbackVector.y * Mathf.Sqrt(g * data.Force * data.Duration);

        currentKnockbackTime = data.Duration;

        IsKnockedBack = true;
    }
}
