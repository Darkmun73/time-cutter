using Unity.VisualScripting;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private Transform target;

    public EnemyChasingState(Enemy enemy, Transform target) : base(enemy)
    {
        this.target = target;
    }

    public EnemyChasingState(Enemy enemy) : base(enemy)
    {
        this.target = null;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        var velocityX = enemy.Data.MovementSpeed;
        if (enemy.transform.position.x > target.position.x)
            velocityX *= -1;

        enemy.SetVelocityX(velocityX);
    }

    public override void Exit()
    {
        target = null;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}