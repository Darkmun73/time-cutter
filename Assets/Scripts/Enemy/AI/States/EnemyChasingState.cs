using System.Collections;
using Pathfinding;
using UnityEngine;

public class EnemyChasingState : IState
{
    private readonly EnemyController enemyController;
    private Path path;
    private int currentWaypoint;
    
    private Coroutine updatePathCoroutine;

    public EnemyChasingState(EnemyController controller)
    {
        this.enemyController = controller;
    }

    public void Enter()
    {
        Transform target = enemyController.Target.Transform;
        Debug.Assert(target != null);
        
        updatePathCoroutine = enemyController.StartCoroutine(RepeatUpdatePath());

        // Transform enemy = enemyController.transform;
        // Movement movement = enemyController.Movement;

        // var horizontalInput =  enemy.position.x > target.position.x ? -1f : 1f;
        // movement.SetUpDirections(horizontalInput, 0f);

    }

    public void Exit()
    {
        enemyController.StopCoroutine(updatePathCoroutine);
    }

    public void LogicUpdate()
    {
        Transform target = enemyController.Target.Transform;
        DirectionsController directions = enemyController.Directions;

        directions.FaceTarget(target);
    }

    public void PhysicsUpdate()
    {
        PathFollow();
    }

    private IEnumerator RepeatUpdatePath()
    {
        while (true)
        {
            UpdatePath();
            yield return new WaitForSeconds(enemyController.PathUpdateTime);
        }
    } 

    private void UpdatePath()
    {
        Transform enemy = enemyController.transform;
        Transform target = enemyController.Target.Transform;
        Seeker seeker = enemyController.Seeker;
        if (seeker.IsDone())
        {
            seeker.StartPath(enemy.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
            return;

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            Debug.Log("reached end");
            return;
        }

        // Direction Calculation
        Vector2 direction = ((Vector2)(path.vectorPath[currentWaypoint] - enemyController.transform.position)).normalized;

        Movement movement = enemyController.Movement;
        DirectionsController directions = enemyController.Directions;

        // Jump
        if (movement.IsTouchingGround &&
            direction.y > enemyController.JumpNodeHeightRequirement)
            movement.Jump();

        // Movement
        movement.MoveHorizontal(directions.MovementDirection);

        // Next Waypoint
        float distance = Vector2.Distance(enemyController.transform.position, path.vectorPath[currentWaypoint]);
        if (distance < enemyController.NextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private void OnPathComplete(Path path)
    {
        if (!path.error)
        {
            this.path = path;
            currentWaypoint = 0;
        }
    }
}