using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UpgradeMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject upgradeUIPrefab;

    //[SerializeField] private Text hintText;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private PurchasableUpgrades upgrades;

    private bool isMenuOpen;

    void Awake()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);
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

    void OnEnable()
    {
        inputReader.TogglingUpgradeMenu += ToggleMenu;
    }

    void OnDisable()
    {
        inputReader.TogglingUpgradeMenu -= ToggleMenu;
        if (isMenuOpen)
            ToggleMenu();
    }

    private void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;

        inputReader.ToggleUpgradeMenuMap();
        if (menuPanel != null)
            menuPanel.SetActive(isMenuOpen);

        Time.timeScale = isMenuOpen ? 0f : 1f;
    }
}
