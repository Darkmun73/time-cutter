using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AttackObjectAnimationController : AnimationController
{
    [SerializeField] private PlayerAttackData playerAttackData;

    void Awake()
    {
        animator = GetComponent<Animator>();
        
        animator.SetFloat("HitSpeedMultiplier", 1/playerAttackData.AttackDuration);
        currentState = EnemyAnimationState.Idle;
        Play(AttackObjectAnimationState.Hit);
    }
}
