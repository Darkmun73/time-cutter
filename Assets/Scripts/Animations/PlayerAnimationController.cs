using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerCombat))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(KnockbackController))]
public class PlayerAnimationController : AnimationController
{
    [SerializeField] private PlayerAttackData attackData;
    [SerializeField] private KnockbackData knockbackData; // TODO: нормально ли сюда передавать data?

    [Header("Air animations")] // TODO: убрать этот хардкод и выставлять значения в зависимости от параметров прыжка
    [SerializeField] private ValueInterval ascentVelocityInterval;
    [SerializeField] private ValueInterval midAirVelocityInterval;
    [SerializeField] private ValueInterval fallVelocityInterval;

    private PlayerController playerController;
    private PlayerCombat playerCombat;
    private Movement movement;
    private KnockbackController knockbackController;

    private bool skipFrame = true;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        playerCombat = GetComponent<PlayerCombat>();
        movement = GetComponent<Movement>();
        knockbackController = GetComponent<KnockbackController>();
        
        currentState = PlayerAnimationState.Idle;

        animator.SetFloat("HitSpeedMultiplier", 1/attackData.AttackDuration);
        animator.SetFloat("HitReactionSpeedMultiplier", 1/knockbackData.Duration);
    }

    void OnEnable()
    {
        playerController.RunningStarted += HandleStartRunning;
        playerController.RunningStopped += HandleStopRunning;
        playerCombat.AttackStarted += HandleAttackStarted;
        movement.Jumped += HandleJump;
        knockbackController.KnockedBack += HandleHitReaction;
    }

    void OnDisable()
    {
        playerController.RunningStarted -= HandleStartRunning;
        playerController.RunningStopped -= HandleStopRunning;
        playerCombat.AttackStarted -= HandleAttackStarted;
        movement.Jumped -= HandleJump;
        knockbackController.KnockedBack -= HandleHitReaction;
    }

    void FixedUpdate()
    {
        //Debug.Log(movement.GetYVelocity());
        // IsTouchingGround always false on the first frame so we're skiping it
        if (skipFrame) { skipFrame = false; return; } // TODO: поправить костыль

        if (IsCurrentStateLocked() && !(currentLockedState as PlayerAnimationState).IsAirState()) return; // TODO: тоже костыль?

        if (!movement.IsTouchingGround)
        {
            if (ascentVelocityInterval.Contains(movement.GetYVelocity()) &&
            //currentLockedState == PlayerAnimationState.JumpStart &&
            currentLockedState != PlayerAnimationState.Ascent)
            {
                Debug.Log("ascent");
                PlayAndLock(PlayerAnimationState.Ascent);
            }
            else if (midAirVelocityInterval.Contains(movement.GetYVelocity()) &&
                //currentLockedState == PlayerAnimationState.Ascent &&
                currentLockedState != PlayerAnimationState.MidAir)
            {
                Debug.Log(movement.IsTouchingGround);
                Debug.Log("midAir");
                PlayAndLock(PlayerAnimationState.MidAir);
            }
            else if (fallVelocityInterval.Contains(movement.GetYVelocity()) &&
                //currentLockedState == PlayerAnimationState.MidAir &&
                currentLockedState != PlayerAnimationState.Fall)
            {
                Debug.Log("fall");
                PlayAndLock(PlayerAnimationState.Fall);
            }
            //Debug.Log(movement.GetYVelocity());
        }
        else
        {
            if (currentLockedState == PlayerAnimationState.Fall &&
                currentLockedState != PlayerAnimationState.Land)
            {
                PlayAndLock(PlayerAnimationState.Land);
            }
             // TODO: исправить костыль. иногда игрок переходит в MidAir из-за того, что перестает касаться ground 
            else if (currentLockedState == PlayerAnimationState.MidAir)
            {
                Unlock();
            }
        }
    }

    private void HandleStartRunning()
    {
        currentState = PlayerAnimationState.Run;
        if (IsCurrentStateLocked()) return;

        Play(PlayerAnimationState.Run);
    }

    private void HandleStopRunning()
    {
        currentState = PlayerAnimationState.Idle;
        if (IsCurrentStateLocked()) return;

        Play(PlayerAnimationState.Idle);
    }

    private void HandleAttackStarted(PlayerCombat.AttackInfo attackInfo)
    {
        //Debug.Log("hit performing");
        if (IsCurrentStateLocked()) return;

        if (Mathf.Abs(attackInfo.Angle) is >= 45 and <= 135)
            PlayAndLock(PlayerAnimationState.AttackTopBottom);
        else
            PlayAndLock(PlayerAnimationState.AttackRightLeft);
    }

    private void HandleJump()
    {
        //Debug.Log("handle jump");
        if (currentLockedState != PlayerAnimationState.NoState &&
            currentLockedState != PlayerAnimationState.Land)
            return;

        PlayAndLock(PlayerAnimationState.JumpStart);
    }

    private void HandleHitReaction()
    {
        PlayAndLock(PlayerAnimationState.HitReaction);
    }
}
