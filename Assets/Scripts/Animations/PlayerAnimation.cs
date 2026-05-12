using UnityEngine;

public class PlayerAnimationState
{
    public static readonly PlayerAnimationState NoState = new("");

    public static readonly PlayerAnimationState Idle = new("Idle");
    public static readonly PlayerAnimationState Run = new("Run");
    public static readonly PlayerAnimationState AttackTopBottom = new("Attack_Top_Bottom");
    public static readonly PlayerAnimationState AttackRightLeft = new("Attack_Right_Left");

    // jumping, air, landing
    public static readonly PlayerAnimationState JumpStart = new("Jump_Start");
    public static readonly PlayerAnimationState Ascent = new("Ascent");
    public static readonly PlayerAnimationState MidAir = new("Mid_Air");
    public static readonly PlayerAnimationState Fall = new("Fall");
    public static readonly PlayerAnimationState Land = new("Land");

    private readonly int hash;

    public PlayerAnimationState(string stateName)
    {
        hash = Animator.StringToHash(stateName);
    }

    public int GetHash()
    {
        return hash;
    }

    public bool IsAirState()
    {
        return this == JumpStart ||
               this == Ascent ||
               this == MidAir ||
               this == Fall ||
               this == Land;
    }
    
    public bool IsAttackState()
    {
        return this == AttackTopBottom ||
               this == AttackRightLeft;
    }
}