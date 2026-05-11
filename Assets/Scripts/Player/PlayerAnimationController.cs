using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerCombat))]
[RequireComponent(typeof(Movement))]
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerAttackData attackData;

    [Header("Air animations")] // TODO: убрать этот хардкод и выставлять значения в зависимости от параметров прыжка
    [SerializeField] private ValueInterval ascentVelocityInterval;
    [SerializeField] private ValueInterval midAirVelocityInterval;
    [SerializeField] private ValueInterval fallVelocityInterval;

    private Animator animator;
    private PlayerController playerController;
    private PlayerCombat playerCombat;
    private Movement movement;

    private readonly int idleStateHash = Animator.StringToHash("Idle");
    private readonly int runStateHash = Animator.StringToHash("Run");
    private readonly int attackTopBottomStateHash = Animator.StringToHash("Attack_Top_Bottom");
    private readonly int attackRightLeftStateHash = Animator.StringToHash("Attack_Right_Left");

    // jumping, air, landing
    private readonly int jumpStartStateHash = Animator.StringToHash("Jump_Start");
    private readonly int ascentStateHash = Animator.StringToHash("Ascent");
    private readonly int midAirStateHash = Animator.StringToHash("Mid_Air");
    private readonly int fallStateHash = Animator.StringToHash("Fall");
    private readonly int landStateHash = Animator.StringToHash("Land");

    private int currentState = -1;
    private int currentLockedState = -1;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        playerCombat = GetComponent<PlayerCombat>();
        movement = GetComponent<Movement>();

        currentState = idleStateHash;

        animator.SetFloat("HitSpeedMultiplier", 1/attackData.HitDuration);
    }

    void OnEnable()
    {
        playerController.RunningStarted += HandleStartMoving;
        playerController.RunningStopped += HandleStopMoving;
        playerCombat.HitPerforming += HandleHitPerforming;
        movement.Jumped += HandleJump;
    }

    void OnDisable()
    {
        playerController.RunningStarted -= HandleStartMoving;
        playerController.RunningStopped -= HandleStopMoving;
        playerCombat.HitPerforming -= HandleHitPerforming;
        movement.Jumped -= HandleJump;
    }

    void Update()
    {
        if (ascentVelocityInterval.Contains(movement.GetYVelocity()) &&
            currentLockedState != jumpStartStateHash &&
            currentLockedState != ascentStateHash)
        {
            PlayAscent(true);
        }
        if (midAirVelocityInterval.Contains(movement.GetYVelocity()) &&
            currentLockedState == ascentStateHash &&
            currentLockedState != midAirStateHash)
        {
            PlayMidAir(true);
        }
        if (fallVelocityInterval.Contains(movement.GetYVelocity()) &&
            currentLockedState == midAirStateHash &&
            currentLockedState != fallStateHash)
        {
            PlayFall(true);
        }
        if (movement.IsTouchingGround &&
            currentLockedState == fallStateHash &&
            currentLockedState != landStateHash)
        {
            PlayLand(true);
        }
    }

    private void HandleStartMoving()
    {
        currentState = runStateHash;
        if (currentLockedState != -1) return;

        PlayRun();
    }

    private void HandleStopMoving()
    {
        currentState = idleStateHash;
        if (currentLockedState != -1) return;

        PlayIdle();
    }

    private void HandleHitPerforming(PlayerCombat.HitInfo hitInfo)
    {
        if (currentLockedState != -1) return;

        if (Mathf.Abs(hitInfo.Angle) is >= 45 and <= 135)
            PlayAttackTopBottom(true);
        else
            PlayAttackRightLeft(true);
    }

    private void HandleHitPerformed()
    {
        currentLockedState = -1;
        animator.Play(currentState, 0, 0f);
    }

    private void HandleJump()
    {
        Debug.Log("handle jump");
        if (currentLockedState != -1 && currentLockedState != landStateHash) return;

        PlayJumpStart(true);
    }

    private void HandleAscentAfterJump()
    {
        Debug.Log("handle ascent after jump");
        PlayAscent(true);
    }

    private void HandleLanded()
    {
        currentLockedState = -1;
        animator.Play(currentState, 0, 0f);
    }
    
    private bool IsAttackState(int stateHash)
    {
        return stateHash == attackTopBottomStateHash ||
               stateHash == attackRightLeftStateHash;
    }

    private void PlayIdle()
    {
        animator.Play(idleStateHash, 0, 0f);
    }

    private void PlayRun()
    {
        animator.Play(runStateHash, 0, 0f);
    }

    private void PlayAttackTopBottom(bool toLock)
    {
        if (toLock)
            currentLockedState = attackTopBottomStateHash;

        animator.Play(attackTopBottomStateHash, 0, 0f);
    }

    private void PlayAttackRightLeft(bool toLock)
    {
        if (toLock)
            currentLockedState = attackRightLeftStateHash;

        animator.Play(attackRightLeftStateHash, 0, 0f);
    }

    private void PlayJumpStart(bool toLock)
    {
        Debug.Log("jump start");
        if (toLock)
            currentLockedState = jumpStartStateHash;

        animator.Play(jumpStartStateHash, 0, 0f);
    }
    
    private void PlayAscent(bool toLock)
    {
        Debug.Log("ascent");
        if (toLock)
            currentLockedState = ascentStateHash;

        animator.Play(ascentStateHash, 0, 0f);
    }

    private void PlayMidAir(bool toLock)
    {
        Debug.Log("mid air");
        if (toLock)
            currentLockedState = midAirStateHash;

        animator.Play(midAirStateHash, 0, 0f);
    }

    private void PlayFall(bool toLock)
    {
        Debug.Log("fall");
        if (toLock)
            currentLockedState = fallStateHash;

        animator.Play(fallStateHash, 0, 0f);
    }

    private void PlayLand(bool toLock)
    {
        Debug.Log("land");
        if (toLock)
            currentLockedState = landStateHash;

        animator.Play(landStateHash, 0, 0f);
    }
}
