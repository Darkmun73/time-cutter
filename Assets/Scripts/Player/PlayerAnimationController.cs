using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerCombat))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(DirectionsController))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimationController : AnimationController
{
    [SerializeField] private PlayerAttackData attackData;

    [Header("Air animations")] // TODO: убрать этот хардкод и выставлять значения в зависимости от параметров прыжка
    [SerializeField] private ValueInterval ascentVelocityInterval;
    [SerializeField] private ValueInterval midAirVelocityInterval;
    [SerializeField] private ValueInterval fallVelocityInterval;

    private PlayerController playerController;
    private PlayerCombat playerCombat;
    private Movement movement;
    private DirectionsController directions;
    private SpriteRenderer spriteRenderer;

    private bool skipFrame = true;

    protected override void Awake()
    {
        base.Awake();

        playerController = GetComponent<PlayerController>();
        playerCombat = GetComponent<PlayerCombat>();
        movement = GetComponent<Movement>();
        directions = GetComponent<DirectionsController>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // TODO: скорее всего стоит перенести в другое место
        
        currentState = PlayerAnimationState.Idle;

        animator.SetFloat("HitSpeedMultiplier", 1/attackData.HitDuration);
    }

    void OnEnable()
    {
        playerController.RunningStarted += HandleStartMoving;
        playerController.RunningStopped += HandleStopMoving;
        playerCombat.HitPerforming += HandleHitPerforming;
        movement.Jumped += HandleJump;
        directions.MovementDirectionFlipped += FlipSprite;
    }

    void OnDisable()
    {
        playerController.RunningStarted -= HandleStartMoving;
        playerController.RunningStopped -= HandleStopMoving;
        playerCombat.HitPerforming -= HandleHitPerforming;
        movement.Jumped -= HandleJump;
        directions.MovementDirectionFlipped -= FlipSprite;
    }

    void FixedUpdate()
    {
        // IsTouchingGround always false on the first frame so we're skiping it
        if (skipFrame) { skipFrame = false; return; } // TODO: поправить костыль

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
        }
    }

    private void FlipSprite()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    private void HandleStartMoving()
    {
        currentState = PlayerAnimationState.Run;
        if (IsCurrentStateLocked()) return;

        Play(PlayerAnimationState.Run);
    }

    private void HandleStopMoving()
    {
        currentState = PlayerAnimationState.Idle;
        if (IsCurrentStateLocked()) return;

        Play(PlayerAnimationState.Idle);
    }

    private void HandleHitPerforming(PlayerCombat.HitInfo hitInfo)
    {
        Debug.Log("hit performing");
        if (IsCurrentStateLocked()) return;

        if (Mathf.Abs(hitInfo.Angle) is >= 45 and <= 135)
            PlayAndLock(PlayerAnimationState.AttackTopBottom);
        else
            PlayAndLock(PlayerAnimationState.AttackRightLeft);
    }

    private void HandleJump()
    {
        Debug.Log("handle jump");
        if (currentLockedState != PlayerAnimationState.NoState &&
            currentLockedState != PlayerAnimationState.Land)
            return;

        PlayAndLock(PlayerAnimationState.JumpStart);
    }

    // private void HandleAscentAfterJump()
    // {
    //     Debug.Log("handle ascent after jump");
    //     PlayAndLock(PlayerAnimationState.Ascent);
    // }

    // private void PlayIdle()
    // {
    //     animator.Play(idleStateHash, 0, 0f);
    // }

    // private void PlayRun()
    // {
    //     animator.Play(runStateHash, 0, 0f);
    // }
}
