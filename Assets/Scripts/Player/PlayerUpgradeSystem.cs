using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerCombat))]
public class PlayerUpgradeSystem : MonoBehaviour
{
    private readonly List<Upgrade> appliedUpgrades = new();

    public Health Health {get; private set;}
    public PlayerCombat Combat {get; private set;}

    void Awake()
    {
        Health = GetComponent<Health>();
        Combat = GetComponent<PlayerCombat>();
    }

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
}
