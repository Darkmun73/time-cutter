using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    public EnemyChasingState(Enemy enemy) : base(enemy) {}

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (enemy.Target == null) return;

        var velocityX = enemy.Data.MovementSpeed;
        if (enemy.transform.position.x > enemy.Target.position.x)
            velocityX *= -1;

        enemy.SetVelocityX(velocityX);
    }
}