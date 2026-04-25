using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/Attack Data")]
public class AttackData : ScriptableObject
{
    [field: SerializeField] public float BaseDamage {get; private set;} = 10f;
}
