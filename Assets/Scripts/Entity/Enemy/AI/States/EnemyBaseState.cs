public abstract class EnemyBaseState : IState
{
    protected Enemy enemy;

    public EnemyBaseState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public virtual void Enter() {}

    public virtual void Exit() {}

    public virtual void PhysicsUpdate() {}

    public virtual void LogicUpdate() {}
}