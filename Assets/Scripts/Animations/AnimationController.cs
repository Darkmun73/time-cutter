using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class AnimationController : MonoBehaviour
{
    protected Animator animator;

    protected PlayerAnimationState currentState = PlayerAnimationState.NoState;
    protected PlayerAnimationState currentLockedState = PlayerAnimationState.NoState;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected void Play(PlayerAnimationState state)
    {
        animator.Play(state.GetHash(), 0, 0f);
    }
    
    protected void PlayAndLock(PlayerAnimationState state)
    {
        currentLockedState = state;
        animator.Play(state.GetHash(), 0, 0f);
    }

    protected void Unlock()
    {
        currentLockedState = PlayerAnimationState.NoState;
        Play(currentState);
    }

    protected bool IsCurrentStateLocked()
    {
        return currentLockedState != PlayerAnimationState.NoState;
    }
}