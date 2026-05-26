public class DummyAnimationState : AnimationState
{
    public static readonly DummyAnimationState Idle = new("Idle");
    public static readonly DummyAnimationState HitReaction = new("Hit_Reaction");

    public DummyAnimationState(string stateName) : base(stateName) {}
}