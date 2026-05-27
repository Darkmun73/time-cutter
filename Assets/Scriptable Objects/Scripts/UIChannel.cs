using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UIChannel", menuName = "Scriptable Objects/UI Channel")]
public class UIChannel : ScriptableObject
{
    public event UnityAction<Upgrade, Action<bool>> ApplyUpgradeRequested;
    public event UnityAction<Upgrade> UnapplyUpgradeRequested;
    public event UnityAction<ComboUpgrade, Action<List<float>>> ComboAnglesRequested; // TODO: переделать

    public void RequestApplyUpgrade(Upgrade upgrade, Action<bool> callback)
    {
        ApplyUpgradeRequested?.Invoke(upgrade, callback);
    }

    public void RequestUnapplyUpgrade(Upgrade upgrade)
    {
        UnapplyUpgradeRequested?.Invoke(upgrade);
    }

    public void RequestComboAngles(ComboUpgrade upgrade, Action<List<float>> callback)
    {
        ComboAnglesRequested?.Invoke(upgrade, callback);
    }
}
