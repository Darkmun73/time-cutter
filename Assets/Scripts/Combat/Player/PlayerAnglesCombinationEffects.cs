using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerAnglesCombinationEffects : MonoBehaviour
{
    [SerializeField] private PlayerAnglesCombinationEffectsData data;
    [SerializeField] private UIChannel uIChannel; // TODO: переделать

    private Health health;

    public AnglesCombinationEffect HealEffect {get; private set;}

    void Awake()
    {
        health = GetComponent<Health>();
        HealEffect = new(data.HealHitAnglesCombination, HealEffectApply);
    }

    void OnEnable()
    {
        uIChannel.ComboAnglesRequested += OnComboAnglesRequested;
    }

    void OnDisable()
    {
        uIChannel.ComboAnglesRequested -= OnComboAnglesRequested;
    }

    private void OnComboAnglesRequested(ComboUpgrade upgrade, Action<List<float>> callback)
    {
        callback?.Invoke(HealEffect.AnglesCombination); // TODO: Ну не только же HealEffect
    }

    private void HealEffectApply()
    {
        health.Heal(data.HealValue);
        Debug.Log("Healed");
    }
}
