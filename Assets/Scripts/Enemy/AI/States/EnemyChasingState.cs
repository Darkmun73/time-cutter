using UnityEngine;

public class EnemyChasingState : IState
{
    private readonly EnemyController enemyController;

    public EnemyChasingState(EnemyController controller)
    {
        this.enemyController = controller;
    }

    public void Enter()
    {
        Transform enemy = enemyController.transform;
        Transform target = enemyController.Target.Transform;
        Movement movement = enemyController.Movement;

        var horizontalInput =  enemy.position.x > target.position.x ? -1f : 1f;
        movement.SetUpDirections(horizontalInput, 0f);

        if (target == null)
            Debug.Log("Target to chase is null!");
    }

    public void Exit() {}

    public void LogicUpdate()
    {
        Transform enemy = enemyController.transform;
        Transform target = enemyController.Target.Transform;
        Movement movement = enemyController.Movement;

        var horizontalInput =  enemy.position.x > target.position.x ? -1f : 1f;
        Direction newMovementDirection = DirectionHelper.FromVector(new(horizontalInput, 0));
        Direction currenMovementtDirection = movement.GetMovementDirection();

        // same direction = don't need to change it
        if (newMovementDirection != currenMovementtDirection)
        {
            movement.SetUpDirections(horizontalInput, 0f);
        }
    }

    public void PhysicsUpdate()
    {
        enemyController.Movement.MoveHorizontal();
    }
}