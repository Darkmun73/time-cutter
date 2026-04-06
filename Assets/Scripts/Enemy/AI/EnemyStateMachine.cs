using UnityEngine;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(KnockbackController))]
public class EnemyStateMachine : StateMachine
{
    private Enemy enemy;
    private Movement enemyMovement;
    private KnockbackController knockbackController;

    private Transform target = null;


    private EnemyIdleState idleState;
    private EnemyChasingState chasingState;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyMovement = GetComponent<Movement>();
        knockbackController = GetComponent<KnockbackController>();
    }

    void Start()
    {
        idleState = new EnemyIdleState(enemyMovement);
         // TODO: дать структуру с контекстом (ради target, потому что он изменяется), контекст обновлять в этом классе
        chasingState = new EnemyChasingState(enemy.transform, target, enemyMovement);
        
        CurrentState = idleState;
    }

    protected override void FixedUpdate()
    {
        if (!knockbackController.IsKnockedBack)
            base.FixedUpdate();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.transform;
            chasingState.SetTarget(target);
            CurrentState = chasingState;

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = null;
            chasingState.SetTarget(target);
            CurrentState = idleState;
        }
    }
}
