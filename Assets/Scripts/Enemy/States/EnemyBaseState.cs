public abstract class EnemyBaseState : BaseState
{
    private Enemy enemy;

    public EnemyBaseState(Enemy enemy)
    {
        this.enemy = enemy;
    }
}