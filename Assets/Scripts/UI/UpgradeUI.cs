using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private UIChannel uiChannel;
    
    [SerializeField] private TextMeshProUGUI nameTextField;
    [SerializeField] private TextMeshProUGUI descriptionTextField;
    [SerializeField] private Button applyButton;

    public void Initialize(Upgrade upgrade)
    {
        nameTextField.text = upgrade.Name;
        descriptionTextField.text = upgrade.Description;
        
        applyButton.onClick.AddListener(() => HandleUpgradeApplying(upgrade));
    }

    private void HandleUpgradeApplying(Upgrade upgrade)
    {
        var applyButtonTextField = applyButton.GetComponentInChildren<TextMeshProUGUI>();
        uiChannel.RequestApplyUpgrade(upgrade, isApplied =>
        {
            if (isApplied)
            {
                applyButtonTextField.text = "Applied";
                applyButton.interactable = false;
            }
        });
    }
}
