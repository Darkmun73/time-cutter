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
        //Debug.Log("in attack state");
        enemyController.Combat.StartCooldown();
    }

    public void Exit() {}

    public void LogicUpdate() {}

    public void PhysicsUpdate()
    {
        enemyController.Movement.StopHorizontalMovement();
        
        if (enemyController.Combat.CanAttack)
        {
            //Debug.Log("attack");
            enemyController.Combat.Attack();
        }
    }
}