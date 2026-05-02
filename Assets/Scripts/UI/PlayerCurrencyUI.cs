using TMPro;
using UnityEngine;

public class PlayerCurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyField;

    private Currency playerCurrency;

    void Awake()
    {
        playerCurrency = FindFirstObjectByType<Player>().GetComponent<Currency>();
        SetCurrencyAmount(playerCurrency.Amount);
    }

    void OnEnable()
    {
        playerCurrency.CurrencyAmountChanged += SetCurrencyAmount;
    }

    void OnDisable()
    {
        playerCurrency.CurrencyAmountChanged -= SetCurrencyAmount;
    }

    private void SetCurrencyAmount(int amount)
    {
        currencyField.text = amount.ToString();
    }
}
