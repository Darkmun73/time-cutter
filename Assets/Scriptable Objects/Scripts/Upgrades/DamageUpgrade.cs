using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Scriptable Objects/Upgrades/Damage Upgrade")]
public class DamageUpgrade : Upgrade
{
    [field: SerializeField] public float Multiplier { get; private set; } = 1f;

    public override void Apply(PlayerUpgradeSystem upgradeSystem)
    {
        upgradeSystem.Combat.IncreaseDamageMultiplierByFactor(Multiplier);
    }


    public override void Unapply(PlayerUpgradeSystem upgradeSystem)
    {
        upgradeSystem.Combat.DecreaseDamageMultiplierByFactor(Multiplier);
    }

}
