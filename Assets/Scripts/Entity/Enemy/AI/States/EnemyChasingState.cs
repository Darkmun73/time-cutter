using UnityEngine;

public class EnemyChasingState : IState
{
    private readonly Transform enemy;
    private readonly Movement enemyMovement;
    private Transform target;

    public EnemyChasingState(Transform enemy, Transform target, Movement enemyMovement)
    {
        this.enemy = enemy;
        this.target = target;
        this.enemyMovement = enemyMovement;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void Enter()
    {
        var horizontalInput =  enemy.position.x > target.position.x ? -1f : 1f;
        enemyMovement.SetUpDirections(horizontalInput, 0f);

        if (target == null)
            Debug.Log("Target to chase is null!");
    }

    public void Exit() {}

    public void LogicUpdate()
    {
        var horizontalInput =  enemy.position.x > target.position.x ? -1f : 1f;
        Direction newMovementDirection = DirectionHelper.FromVector(new(horizontalInput, 0));
        Direction currenMovementtDirection = enemyMovement.GetMovementDirection();

        // same direction = don't need to change it
        if (newMovementDirection != currenMovementtDirection)
        {
            enemyMovement.SetUpDirections(horizontalInput, 0f);
        }
    }

    public void PhysicsUpdate()
    {
        enemyMovement.MoveHorizontal();
    }
}