using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DummyAnimationController : AnimationController
{
    [SerializeField] private HitReceiver hitReceiver;

    void Awake()
    {
        animator = GetComponent<Animator>();

        currentState = DummyAnimationState.Idle;
    }

    void OnEnable()
    {
        hitReceiver.HitReceived += HandleHitReaction;
    }

    void OnDisable()
    {
        hitReceiver.HitReceived -= HandleHitReaction;
    }

    private void HandleHitReaction(HitInfo hitInfo)
    {
        PlayAndLock(DummyAnimationState.HitReaction);
    }
    
}
