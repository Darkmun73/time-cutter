using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyStateMachine : StateMachine
{
    private Enemy enemy;
    private KnockbackController knockbackController;

    private EnemyIdleState idleState;
    private EnemyChasingState chasingState;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    void Start()
    {
        knockbackController = enemy.KnockbackController;

        idleState = new EnemyIdleState(enemy);
        chasingState = new EnemyChasingState(enemy);
    }

    protected override void FixedUpdate()
    {
        if (!(knockbackController != null && knockbackController.IsKnockedBack))
            CurrentState.PhysicsUpdate();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy.Target = collision.transform;
            CurrentState = chasingState;

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy.Target = null;
            CurrentState = idleState;
        }
    }
}
