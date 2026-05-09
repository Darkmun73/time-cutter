using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UpgradeMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject upgradeUIPrefab;

    //[SerializeField] private Text hintText;
    [SerializeField] private PurchasableUpgrades upgrades;

    [SerializeField] private Key toggleKey = Key.Tab;

    private bool isMenuOpen;

    void Awake()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);

        //RefreshView();
    }

    void Start()
    {
        if (!upgradeUIPrefab.TryGetComponent<UpgradeUI>(out var _))
        {
            Debug.LogError("That is not upgrade ui prefab!");
            return;
        }

        foreach(var upgrade in upgrades.Get())
        {
            var upgradeUIObject = Instantiate(upgradeUIPrefab, menuPanel.transform);
            var upgradeUI = upgradeUIObject.GetComponent<UpgradeUI>();
            upgradeUI.Initialize(upgrade, (int)upgrades.GetCost(upgrade));
        }     
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current[toggleKey].wasPressedThisFrame)
            ToggleMenu();

    }

    void OnDisable()
    {
        if (isMenuOpen)
            Time.timeScale = 1f;
    }

    private void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;

        if (menuPanel != null)
            menuPanel.SetActive(isMenuOpen);

        Time.timeScale = isMenuOpen ? 0f : 1f;
        //RefreshView();
    }

    // private void ApplyUpgrade(Upgrade upgrade)
    // {
    //     if (upgrade == null || upgradeSystem.HasUpgrade(upgrade))
    //         return;

    //     upgradeSystem.AddUpgrade(upgrade);
    //     RefreshView();
    // }

    // private void RefreshView()
    // {
    //     if (hintText != null)
    //         hintText.text = $"Open/close: {toggleKey}";

    //     if (damageInfoText != null)
    //         damageInfoText.text = BuildUpgradeText(damageUpgrade);

    //     bool damageApplied = damageUpgrade != null && upgradeSystem.HasUpgrade(damageUpgrade);

    //     if (damageStatusText != null)
    //         damageStatusText.text = damageApplied ? "Status: applied" : "Status: available";

    //     if (damageApplyButton != null)
    //         damageApplyButton.interactable = !damageApplied;

    //     if (healthInfoText != null)
    //         healthInfoText.text = BuildUpgradeText(healthUpgrade);

    //     bool healthApplied = healthUpgrade != null && upgradeSystem.HasUpgrade(healthUpgrade);

    //     if (healthStatusText != null)
    //         healthStatusText.text = healthApplied ? "Status: applied" : "Status: available";

    //     if (healthApplyButton != null)
    //         healthApplyButton.interactable = !healthApplied;
    // }

    // private string BuildUpgradeText(Upgrade upgrade)
    // {
    //     if (upgrade == null)
    //         return "Missing upgrade reference";

    //     string effect = GetUpgradeDetails(upgrade);
    //     return $"{upgrade.Name}\n{upgrade.Description}\n{effect}";
    // }

    // private static string GetUpgradeDetails(Upgrade upgrade)
    // {
    //     if (upgrade is DamageUpgrade damage)
    //         return $"Effect: damage x{damage.Multiplier:0.##}";

    //     if (upgrade is HealthUpgrade health)
    //         return $"Effect: max HP x{health.Multiplier:0.##}";

    //     return "Effect: basic upgrade";
    // }
}
