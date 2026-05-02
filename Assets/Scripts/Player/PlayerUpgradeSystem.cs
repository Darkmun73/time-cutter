using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerCombat))]
[RequireComponent(typeof(PlayerAnglesCombinationEffects))]
public class PlayerUpgradeSystem : MonoBehaviour
{
    [SerializeField] private UIChannel uiChannel;
    private readonly List<Upgrade> appliedUpgrades = new();

    public Health Health {get; private set;}
    public PlayerCombat Combat {get; private set;}
    public PlayerAnglesCombinationEffects CombinationEffects {get; private set;}

    void Awake()
    {
        Health = GetComponent<Health>();
        Combat = GetComponent<PlayerCombat>();
        CombinationEffects = GetComponent<PlayerAnglesCombinationEffects>();
    }

    // void OnEnable()
    // {
    //     uiChannel.ApplyUpgradeRequested += AddUpgrade;
    //     uiChannel.UnapplyUpgradeRequested += RemoveUpgrade;
    // }

    // void OnDisable()
    // {
    //     uiChannel.ApplyUpgradeRequested -= AddUpgrade;
    //     uiChannel.UnapplyUpgradeRequested -= RemoveUpgrade;
    // }

    public void AddUpgrade(Upgrade upgrade)
    {
        if (upgrade == null || appliedUpgrades.Contains(upgrade))
            return;
        
        appliedUpgrades.Add(upgrade);
        upgrade.Apply(this);
    }

    public void RemoveUpgrade(Upgrade upgrade)
    {
        if (upgrade == null || !appliedUpgrades.Remove(upgrade))
            return;

        upgrade.Unapply(this);
    }

    public bool HasUpgrade(Upgrade upgrade)
    {
        return upgrade != null && appliedUpgrades.Contains(upgrade);
    }
}
