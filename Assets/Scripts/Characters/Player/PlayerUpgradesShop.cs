using System;
using UnityEngine;

[RequireComponent(typeof(PlayerUpgradeSystem))]
[RequireComponent(typeof(Currency))]
public class PlayerUpgradesShop : MonoBehaviour
{
    [SerializeField] private UIChannel uiChannel;
    [SerializeField] private PurchasableUpgrades upgrades;

    private PlayerUpgradeSystem upgradeSystem;
    private Currency currency;

    void Awake()
    {
        upgradeSystem = GetComponent<PlayerUpgradeSystem>();
        currency = GetComponent<Currency>();
    }

    void OnEnable()
    {
        uiChannel.ApplyUpgradeRequested += OnApplyUpgradeRequested;
    }

    void OnDisable()
    {
        uiChannel.ApplyUpgradeRequested -= OnApplyUpgradeRequested;
    }

    private void OnApplyUpgradeRequested(Upgrade upgrade, Action<bool> callback)
    {
        bool success = TryPurchaseUpgrade(upgrade);
        callback?.Invoke(success);
    }

    private bool TryPurchaseUpgrade(Upgrade upgrade)
    {
        var cost = upgrades.GetCost(upgrade);
        if (cost == null)
        {
            Debug.LogWarning($"PlayerUpgradesShop: There is no upgrade \"{upgrade.Name}\" in purchasable upgrades!");
            return false;
        }

        Debug.Log($"{currency.Amount}, {cost}");
        
        if (currency.Amount >= cost)
        {
            upgradeSystem.AddUpgrade(upgrade);
            currency.Amount -= (int)cost;
            return true;
        }
        return false;
    }
}