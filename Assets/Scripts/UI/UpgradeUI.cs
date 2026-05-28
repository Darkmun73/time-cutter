using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private UIChannel uiChannel;
    
    [SerializeField] private TextMeshProUGUI nameTextField;
    [SerializeField] private TextMeshProUGUI descriptionTextField;
    [SerializeField] private GameObject comboContainer;
    [SerializeField] private TextMeshProUGUI priceField;
    [SerializeField] private Button applyButton;
    
    [SerializeField] private GameObject attackObjectUIPrefab;

    public void Initialize(Upgrade upgrade, int price)
    {
        nameTextField.text = upgrade.Name;
        descriptionTextField.text = upgrade.Description;
        priceField.text = $"Price: {price}"; // TODO: исправить захардкоженную строку
        comboContainer.SetActive(false);

        if (upgrade is ComboUpgrade comboUpgrade)
        {
            comboContainer.SetActive(true);
            uiChannel.RequestComboAngles(comboUpgrade, comboAngles =>
            {
                foreach (var angle in comboAngles)
                {
                    var attackObjectUI = Instantiate(attackObjectUIPrefab, comboContainer.transform);
                    attackObjectUI.transform.rotation = Quaternion.Euler(0, 0, -45); // TODO: убрать хардкод (45 градусов, потому что так спрайт будет как горизонтальная линия)
                    attackObjectUI.transform.Rotate(Vector3.forward, angle);
                }
            });
        }
        
        //TODO: UI должен отображать логику, а не делать её самим, поэтому нужно писать applied в зависимости от того, применен ли upgrade у игрока, а не просто по клику на кнопку
        applyButton.onClick.AddListener(() => HandleUpgradeApplying(upgrade)); // TODO: придумать, где делать RemoveListener
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
