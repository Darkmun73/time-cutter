using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TutorialAnimationController : AnimationController
{

    void Awake()
    {
        animator = GetComponent<Animator>();
        
        currentState = TutorialAnimationState.Empty;
    }

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }

    // private void HandleStartRunning()
    // {
    //     //Debug.Log("start moving");
    //     currentState = EnemyAnimationState.Run;
    //     if (IsCurrentStateLocked()) return;

    //     Play(EnemyAnimationState.Run);
    // }

    // private void HandleStopRunning()
    // {
    //     //Debug.Log("stop moving");
    //     currentState = EnemyAnimationState.Idle;
    //     if (IsCurrentStateLocked()) return;

    //     Play(EnemyAnimationState.Idle);
    // }

    // private void HandleHitPerformed()
    // {
    //     //Debug.Log("hit performing");
    //     if (IsCurrentStateLocked()) return;

    //     PlayAndLock(EnemyAnimationState.Attack);
    // }

    // private void HandleHitReaction()
    // {
    //     PlayAndLock(EnemyAnimationState.HitReaction);
    // }
}
