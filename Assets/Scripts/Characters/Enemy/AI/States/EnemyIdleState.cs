public class EnemyIdleState : IState
{
    private readonly EnemyController enemyController;
    public EnemyIdleState(EnemyController controller)
    {
        this.enemyController = controller;
    }

    public void Enter() {}

    public void Exit() {}

    public void LogicUpdate() {}

    public void PhysicsUpdate()
    {
        enemyController.Movement.StopHorizontalMovement();
    }
}