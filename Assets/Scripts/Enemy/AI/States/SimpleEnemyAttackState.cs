using UnityEngine;

public class SimpleEnemyAttackState : EnemyAttackState
{
    private readonly EnemyController enemyController;
    public SimpleEnemyAttackState(EnemyController controller) : base(controller) {}
}