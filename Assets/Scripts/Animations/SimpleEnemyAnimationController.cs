using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SimpleEnemyAnimationController : AnimationController
{
    [SerializeField] private KnockbackData knockbackData;

    private EnemyController enemyController;
    private EnemyCombat enemyCombat;
    private KnockbackController knockbackController;

    void Awake()
    {
        animator = GetComponent<Animator>();
        enemyController = GetComponentInParent<EnemyController>();
        enemyCombat = GetComponentInParent<EnemyCombat>();
        knockbackController = GetComponentInParent<KnockbackController>();
        
        currentState = EnemyAnimationState.Idle;

        animator.SetFloat("HitReactionSpeedMultiplier", 1/knockbackData.Duration);

        Debug.Assert(enemyController != null, "EnemyAnimationController: Parent object must have EnemyController!");
        Debug.Assert(enemyCombat != null, "EnemyAnimationController: Parent object must have EnemyCombat!");
    }

    void OnEnable()
    {
        enemyController.ChasingStarted += HandleStartRunning;
        enemyController.ChasingStopped += HandleStopRunning;
        enemyCombat.Hit += HandleHitPerformed;
        knockbackController.KnockBacked += HandleHitReaction;
    }

    void OnDisable()
    {
        enemyController.ChasingStarted -= HandleStartRunning;
        enemyController.ChasingStopped -= HandleStopRunning;
        enemyCombat.Hit -= HandleHitPerformed;
        knockbackController.KnockBacked -= HandleHitReaction;
    }

    private void HandleStartRunning()
    {
        //Debug.Log("start moving");
        currentState = EnemyAnimationState.Run;
        if (IsCurrentStateLocked()) return;

        Play(EnemyAnimationState.Run);
    }

    private void HandleStopRunning()
    {
        //Debug.Log("stop moving");
        currentState = EnemyAnimationState.Idle;
        if (IsCurrentStateLocked()) return;

        Play(EnemyAnimationState.Idle);
    }

    private void HandleHitPerformed()
    {
        //Debug.Log("hit performing");
        if (IsCurrentStateLocked()) return;

        PlayAndLock(EnemyAnimationState.Attack);
    }

    private void HandleHitReaction()
    {
        PlayAndLock(EnemyAnimationState.HitReaction);
    }
}
