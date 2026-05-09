using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private UIChannel uiChannel;
    
    [SerializeField] private TextMeshProUGUI nameTextField;
    [SerializeField] private TextMeshProUGUI descriptionTextField;
    [SerializeField] private TextMeshProUGUI priceField;
    [SerializeField] private Button applyButton;

    public void Initialize(Upgrade upgrade, int price)
    {
        nameTextField.text = upgrade.Name;
        descriptionTextField.text = upgrade.Description;
        priceField.text = $"Стоимость: {price}"; // TODO: исправить захардкоженную строку
        
        applyButton.onClick.AddListener(() => HandleUpgradeApplying(upgrade));
    }

    private void HandleUpgradeApplying(Upgrade upgrade)
    {
        var applyButtonTextField = applyButton.GetComponentInChildren<TextMeshProUGUI>();
        uiChannel.RequestApplyUpgrade(upgrade, isApplied =>
        {
            if (isApplied)
            {
                applyButtonTextField.text = "Applied"; // TODO: исправить захардкоженную строку (и также в тексте UI)
                applyButton.interactable = false;
            }
        });
    }
}
