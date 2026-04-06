using UnityEngine;

public class EnemyIdleState : IState
{
    private readonly Movement enemyMovement;
    public EnemyIdleState(Movement enemyMovement)
    {
        this.enemyMovement = enemyMovement;
    }

    public void Enter() {}

    public void Exit() {}

    public void LogicUpdate() {}

    public void PhysicsUpdate()
    {
        enemyMovement.StopHorizontalMovement();
    }
}