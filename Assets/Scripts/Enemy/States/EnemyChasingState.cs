using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private Transform target;

    public EnemyChasingState(Enemy enemy, Transform target) : base(enemy)
    {
        this.target = target;
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
    }

    public override void Exit()
    {
        base.Exit();
    }
}