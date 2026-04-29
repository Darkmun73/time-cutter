using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Scriptable Objects/Player Upgrades/Damage Upgrade")]
public class DamageUpgrade : Upgrade
{
    [SerializeField] private float multiplier = 1f;

    public override void Apply(PlayerUpgradeSystem upgradeSystem)
    {
        upgradeSystem.Combat.IncreaseDamageMultiplierByFactor(multiplier);
    }


    public override void Unapply(PlayerUpgradeSystem upgradeSystem)
    {
        upgradeSystem.Combat.DecreaseDamageMultiplierByFactor(multiplier);
    }

}
