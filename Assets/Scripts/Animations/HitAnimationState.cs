public class HitAnimationState : AnimationState
{
    public static readonly PlayerAnimationState Hit = new("Hit");

    public HitAnimationState(string stateName) : base(stateName) {}
}