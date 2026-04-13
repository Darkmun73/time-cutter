using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttackData", menuName = "Scriptable Objects/Player Attack Data")]
public class PlayerAttackData : AttackData
{
    [field: SerializeField] public float ObjectsHitAnglesCleanupInterval {get; private set;} = 2f;
}
