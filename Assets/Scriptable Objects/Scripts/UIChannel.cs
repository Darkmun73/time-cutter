using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UIChannel", menuName = "Scriptable Objects/UI Channel")]
public class UIChannel : ScriptableObject
{
    public event UnityAction<Upgrade> ApplyUpgradeRequested;
    public event UnityAction<Upgrade> UnapplyUpgradeRequested;

    public void RequestApplyUpgrade(Upgrade upgrade)
    {
        ApplyUpgradeRequested?.Invoke(upgrade);
    }

    public void RequestUnapplyUpgrade(Upgrade upgrade)
    {
        UnapplyUpgradeRequested?.Invoke(upgrade);
    }
}
