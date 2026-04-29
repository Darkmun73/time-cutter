using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAnglesCombinationEffectsData", menuName = "Scriptable Objects/Player Angles Combination Effects Data")]
public class PlayerAnglesCombinationEffectsData : ScriptableObject
{
    [field: SerializeField, Header("Heal Effect")] public List<float> HealHitAnglesCombination {get; private set;}
    [field: SerializeField] public float HealValue {get; private set;}
}