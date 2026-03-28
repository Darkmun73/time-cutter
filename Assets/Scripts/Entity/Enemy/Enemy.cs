using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : Entity
{
    
    private Collider2D coll;
    private KnockbackController knockbackController;

    private StateMachine stateMachine;
    private EnemyIdleState idleState;
    private EnemyChasingState chasingState;

    private PlayerEvents playerEvents;

    public readonly EnemyEvents events;
    protected override EntityEvents Events => events;

    protected override void Awake()
    {
        base.Awake();
        coll = GetComponent<Collider2D>();
        knockbackController = GetComponent<KnockbackController>();
        playerEvents = FindFirstObjectByType<Player>().events;

        idleState = new EnemyIdleState(this, rigidBody);
        chasingState = new EnemyChasingState(this);

        stateMachine = new(idleState);
    }

    void OnEnable()
    {
        playerEvents.HitOccured += HandlePlayerHit;
    }

    void OnDisable()
    {
        playerEvents.HitOccured -= HandlePlayerHit;
    }

    void Update()
    {
        stateMachine.Update();
    }

    void FixedUpdate()
    {
        if (!(knockbackController != null && knockbackController.IsKnockedBack))
            stateMachine.FixedUpdate();
    }
    
    private void HandlePlayerHit(GameObject hit)
    {
        // MAYBE TODO: Если будет работать не точно, то мб поменять на IsTouching или подобное
        bool hitTouching = coll.Distance(hit.GetComponent<Collider2D>()).isOverlapped;
        if (hitTouching)
        {
            if (knockbackController != null)
                knockbackController.Knockback(hit.transform.parent.position);
        }
    }

    public void SetVelocityX(float x)
    {
        rigidBody.linearVelocityX = x;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            chasingState.SetTarget(collision.transform);
            stateMachine.CurrentState = chasingState;

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            stateMachine.CurrentState = idleState;

        }
    }
}
