using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttackData", menuName = "Scriptable Objects/Player Attack Data")]
public class PlayerAttackData : AttackData
{
    [field: SerializeField] public float ObjectsHitAnglesCleanupInterval {get; private set;} = 2f;
    [field: SerializeField] public List<float> ShieldBreakAngles {get; private set;} = new() {22.5f, 45f, 67.5f};
    [field: SerializeField] public float AngleTolerance {get; private set;} = 10f;
}
