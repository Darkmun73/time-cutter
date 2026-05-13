using UnityEngine;

public abstract class AnimationController : MonoBehaviour
{
    protected Animator animator;

    protected AnimationState currentState = AnimationState.NoState;
    protected AnimationState currentLockedState = AnimationState.NoState;

    protected void Play(AnimationState state)
    {
        animator.Play(state.GetHash(), 0, 0f);
    }
    
    protected void PlayAndLock(AnimationState state)
    {
        currentLockedState = state;
        animator.Play(state.GetHash(), 0, 0f);
    }

    protected void Unlock()
    {
        currentLockedState = AnimationState.NoState;
        Play(currentState);
    }

    protected bool IsCurrentStateLocked()
    {
        return currentLockedState != AnimationState.NoState;
    }
}