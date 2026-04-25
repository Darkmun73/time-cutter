using UnityEngine;

[CreateAssetMenu(fileName = "HealthData", menuName = "Scriptable Objects/Health Data")]
public class HealthData : ScriptableObject
{
    [field: SerializeField] public float MaxHealth {get; private set;} = 100f;
}
