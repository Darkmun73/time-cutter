public class PlayerAnimationState : AnimationState
{

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

    public PlayerAnimationState(string stateName) : base(stateName) {}


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