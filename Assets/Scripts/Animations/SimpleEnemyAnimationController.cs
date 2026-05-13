using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SimpleEnemyAnimationController : AnimationController
{
    private EnemyController enemyController;
    private EnemyCombat enemyCombat;

    void Awake()
    {
        animator = GetComponent<Animator>();
        enemyController = GetComponentInParent<EnemyController>();
        enemyCombat = GetComponentInParent<EnemyCombat>();
        
        currentState = EnemyAnimationState.Idle;

        Debug.Assert(enemyController != null, "EnemyAnimationController: Parent object must have EnemyController!");
        Debug.Assert(enemyCombat != null, "EnemyAnimationController: Parent object must have EnemyCombat!");
    }

    void OnEnable()
    {
        enemyController.ChasingStarted += HandleStartRunning;
        enemyController.ChasingStopped += HandleStopRunning;
        enemyCombat.Hit += HandleHitPerformed;
    }

    void OnDisable()
    {
        enemyController.ChasingStarted -= HandleStartRunning;
        enemyController.ChasingStopped -= HandleStopRunning;
        enemyCombat.Hit -= HandleHitPerformed;
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
}
