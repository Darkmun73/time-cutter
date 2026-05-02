
using UnityEngine;
using UnityEngine.Events;

public class Currency : MonoBehaviour
{
    public event UnityAction<int> CurrencyAmountChanged;

    private int amount = 0;
    public int Amount
    {
        get => amount;
        set
        {
            if (value < 0)
            {
                Debug.LogWarning("Currency amount cannot be lower than 0!");
                return;
            } else if (value >= 1000000000) return;

            amount = value;
            CurrencyAmountChanged.Invoke(amount);
        }
    }
}