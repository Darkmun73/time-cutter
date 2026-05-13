using UnityEngine;

[CreateAssetMenu(fileName = "KnockbackData", menuName = "Scriptable Objects/Knockback Data")]
public class KnockbackData : ScriptableObject
{
    [field: SerializeField] public float Force {get; private set;} = 5f;
    [field: SerializeField] public float ForceRandomizationVariance {get; private set;} = 0f;
    [field: SerializeField] public float Duration {get; private set;} = 1f;
}
