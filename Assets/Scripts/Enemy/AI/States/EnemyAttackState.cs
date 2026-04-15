using UnityEngine;

public class EnemyAttackState : IState
{
    private readonly EnemyController enemyController;
    public EnemyAttackState(EnemyController controller)
    {
        enemyController = controller;
    }

    public void Enter()
    {
        enemyController.Combat.StartCooldown();
    }

    public void Exit() {}

    public void LogicUpdate() {}

    public void PhysicsUpdate()
    {
        enemyController.Movement.StopHorizontalMovement();
        
        if (enemyController.Combat.CanAttack)
            enemyController.Combat.Attack();
    }
}