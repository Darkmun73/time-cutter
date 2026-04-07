using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackData", menuName = "Scriptable Objects/Enemy Attack Data")]
public class EnemyAttackData : AttackData
{
    [field: SerializeField] public float HitRadius {get; private set;} = 2f;
    [field: SerializeField] public float Cooldown {get; private set;} = 0.5f;
}
