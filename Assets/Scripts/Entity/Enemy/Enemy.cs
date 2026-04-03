using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : Entity
{
    private Rigidbody2D rigidBody;
    
    private KnockbackController knockbackController;
    public KnockbackController KnockbackController => knockbackController;

    private Transform target = null;
    public Transform Target {
        get => target;
        set => target = value;
    }

    public readonly EnemyEvents events = new();
    protected override EntityEvents Events => events;

    protected override void Awake()
    {
        base.Awake();
        rigidBody = GetComponent<Rigidbody2D>();
        knockbackController = GetComponent<KnockbackController>();
    }

    void OnEnable()
    {
        if (knockbackController != null)
            events.GetHitted += knockbackController.Knockback;
    }

    void OnDisable()
    {
        if (knockbackController != null)
            events.GetHitted -= knockbackController.Knockback;
    }
    
    // private void HandlePlayerHit(GameObject hit)
    // {
    //     // MAYBE TODO: Если будет работать не точно, то мб поменять на IsTouching или подобное
    //     bool hitTouching = coll.Distance(hit.GetComponent<Collider2D>()).isOverlapped;
    //     if (hitTouching)
    //     {
    //         HandleHit(hit.transform.parent);
    //     }
    // }

    public void SetVelocityX(float x)
    {
        if (x > 0)
            MoveDirection = Direction.Right;
        else
            MoveDirection = Direction.Left;
        rigidBody.linearVelocityX = x;
    }

    public override void Die()
    {
        Debug.Log("Destroy Enemy");
        Destroy(gameObject);
    }
}
