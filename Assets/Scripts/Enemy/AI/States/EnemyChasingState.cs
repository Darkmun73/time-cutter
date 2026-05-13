using Pathfinding;
using UnityEngine;
using UnityEngine.Events;

public class EnemyChasingState : IState
{
    private readonly EnemyController enemyController;

    public event UnityAction ChasingStarted;
    public event UnityAction ChasingStopped;

    public EnemyChasingState(EnemyController controller)
    {
        this.enemyController = controller;
    }

    public void Enter()
    {
        Transform target = enemyController.Target.Transform;
        Debug.Assert(target != null);

        enemyController.Navigator.StartUpdatingPath();
        ChasingStarted?.Invoke();

        // Transform enemy = enemyController.transform;
        // Movement movement = enemyController.Movement;

        // var horizontalInput =  enemy.position.x > target.position.x ? -1f : 1f;
        // movement.SetUpDirections(horizontalInput, 0f);

    }

    public void Exit()
    {
        enemyController.Navigator.StopUpdatingPath();
        ChasingStopped?.Invoke();
    }

    public void LogicUpdate()
    {
        Transform target = enemyController.Target.Transform;
        DirectionsController directions = enemyController.Directions;

        directions.FaceTarget(target);
    }

    public void PhysicsUpdate()
    {
        enemyController.Navigator.FollowPath();
    }

}
