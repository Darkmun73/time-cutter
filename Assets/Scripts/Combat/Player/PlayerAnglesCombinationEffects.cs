using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerAnglesCombinationEffects : MonoBehaviour
{
    [SerializeField] PlayerAnglesCombinationEffectsData data;

    private Health health;

    public AnglesCombinationEffect HealEffect {get; private set;}

    void Awake()
    {
        health = GetComponent<Health>();

        HealEffect = new(data.HealHitAnglesCombination, () => health.Heal(data.HealValue));
    }
}
