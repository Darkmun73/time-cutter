using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttackData", menuName = "Scriptable Objects/Player Attack Data")]
public class PlayerAttackData : AttackData
{
    [field: SerializeField] public List<float> ShieldBreakAngles {get; private set;} = new() {22.5f, 45f, 67.5f};
    [field: SerializeField] public float ObjectsAttackSequenceCleanupInterval {get; private set;} = 2f;
    [field: SerializeField] public float AttackDuration {get; private set;} = 0.5f;
    [field: SerializeField] public float Cooldown {get; private set;} = 0.2f;
    
    [field: SerializeField, Header("Attack Angles")] public int MaxHitAngles {get; private set;} = 7;
    [field: SerializeField] public float AngleTolerance {get; private set;} = 10f;

    [field: SerializeField, Header("Damage Accumulation")] public float MinDamageCoef {get; private set;} = 0.1f;
    [field: SerializeField] public float MaxDamageCoef {get; private set;} = 1f;
    [field: SerializeField] public float DamageThresholdAngle {get; private set;} = 20f;
    [field: SerializeField] public float DamagePenalty {get; private set;} = 0.3f;
    [field: SerializeField] public float DamageReward {get; private set;} = 0.5f;
    [field: SerializeField] public float RepetitionPenalty {get; private set;} = 0.5f;
    [field: SerializeField] public float DecayCoef {get; private set;} = 1f;

    
}
