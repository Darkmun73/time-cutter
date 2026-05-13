using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HitAnimationController : AnimationController
{
    [SerializeField] private PlayerAttackData playerAttackData;

    void Awake()
    {
        animator = GetComponent<Animator>();
        
        animator.SetFloat("HitSpeedMultiplier", 1/playerAttackData.HitDuration);
        currentState = EnemyAnimationState.Idle;
        Play(HitAnimationState.Hit);
    }
}
