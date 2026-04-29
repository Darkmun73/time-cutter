using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    [field: SerializeField] public string Name {get; private set;}
    [field: SerializeField] public string Description {get; private set;}
    
    public abstract void Apply(PlayerUpgradeSystem upgradeSystem);
    public abstract void Unapply(PlayerUpgradeSystem upgradeSystem);
}
