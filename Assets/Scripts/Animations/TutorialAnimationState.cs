public class TutorialAnimationState : AnimationState
{
    public static readonly TutorialAnimationState Empty = new("Empty");

    public TutorialAnimationState(string stateName) : base(stateName) {}
}