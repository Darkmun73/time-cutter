using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackData", menuName = "Scriptable Objects/Enemy Attack Data")]
public class EnemyAttackData : AttackData
{
    [field: SerializeField] public float Radius {get; private set;} = 2f;
}
