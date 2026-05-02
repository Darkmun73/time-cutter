using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PurchasableUpgrades", menuName = "Scriptable Objects/Purchasable Upgrades")]
public class PurchasableUpgrades : ScriptableObject
{
    [Serializable]
    private struct UpgradeCost
    {
        public Upgrade upgrade;
        public int cost;
    }

    [SerializeField] private List<UpgradeCost> upgradesCost;
    private List<Upgrade> upgrades;

    void OnEnable()
    {
        if (upgradesCost == null) return;

        upgrades = upgradesCost.Select(upgradeCost => upgradeCost.upgrade).ToList();
    }

    public List<Upgrade> Get()
    {
        return upgrades;
    }

    public int? GetCost(Upgrade upgrade)
    {
        if (!upgradesCost.Any(upgradeCost => upgradeCost.upgrade == upgrade)) return null;

        var foundUpgradeCost = upgradesCost.Find(upgradeCost => upgradeCost.upgrade == upgrade);
        return foundUpgradeCost.cost;
    }
}