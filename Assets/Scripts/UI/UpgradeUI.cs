using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private UIChannel uiChannel;
    
    [SerializeField] private TextMeshProUGUI nameTextField;
    [SerializeField] private TextMeshProUGUI descriptionTextField;
    [SerializeField] private Button applyButton;

    private bool isApplied = false;

    public void Initialize(Upgrade upgrade)
    {
        nameTextField.text = upgrade.Name;
        descriptionTextField.text = upgrade.Description;
        
        applyButton.onClick.AddListener(() => HandleUpgrade(upgrade));
    }

    private void HandleUpgrade(Upgrade upgrade)
    {
        string buttonText;
        if (!isApplied)
        {
            uiChannel.RequestApplyUpgrade(upgrade);
            buttonText = "Unapply";
        }
        else
        {
            uiChannel.RequestUnapplyUpgrade(upgrade);
            buttonText = "Apply";
        }
        isApplied = !isApplied;

        var applyButtonTextField = applyButton.GetComponentInChildren<TextMeshProUGUI>();
        applyButtonTextField.text = buttonText;
    }
}
