public class EnemyAnimationState : AnimationState
{
    public static readonly EnemyAnimationState Idle = new("Idle");
    public static readonly EnemyAnimationState Run = new("Run");
    public static readonly EnemyAnimationState Attack = new("Attack");
    public static readonly EnemyAnimationState HitReaction = new("Hit_Reaction");

    public EnemyAnimationState(string stateName) : base(stateName) {}
}