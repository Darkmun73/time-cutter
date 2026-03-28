public abstract class EnemyBaseState : BaseState
{
    protected Enemy enemy;

    public EnemyBaseState(Enemy enemy)
    {
        this.enemy = enemy;
    }
}