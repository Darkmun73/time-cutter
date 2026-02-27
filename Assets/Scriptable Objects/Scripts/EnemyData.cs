using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [field: SerializeField] public float BaseDamage {get; private set;} = 10f;
}
