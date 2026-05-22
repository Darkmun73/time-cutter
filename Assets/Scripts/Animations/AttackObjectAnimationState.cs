public class AttackObjectAnimationState : AnimationState
{
    public static readonly PlayerAnimationState Hit = new("Hit");

    public AttackObjectAnimationState(string stateName) : base(stateName) {}
}