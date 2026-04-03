using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(Enemy enemy) : base(enemy) {}

    public override void PhysicsUpdate()
    {
        enemy.SetVelocityX(0);
    }
}