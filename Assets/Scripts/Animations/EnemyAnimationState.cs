public class EnemyAnimationState : AnimationState
{
    public static readonly PlayerAnimationState Idle = new("Idle");
    public static readonly PlayerAnimationState Run = new("Run");
    public static readonly PlayerAnimationState Attack = new("Attack");

    public EnemyAnimationState(string stateName) : base(stateName) {}
}