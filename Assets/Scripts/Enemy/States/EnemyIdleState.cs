using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private Rigidbody2D rigidBody;
    public EnemyIdleState(Enemy enemy, Rigidbody2D rigidBody) : base(enemy)
    {
        this.rigidBody = rigidBody;
    }

    public override void FixedUpdate()
    {
        rigidBody.linearVelocity = Vector2.zero;
    }
}