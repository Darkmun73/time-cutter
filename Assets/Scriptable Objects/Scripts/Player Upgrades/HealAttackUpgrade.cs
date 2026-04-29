using UnityEngine;

[CreateAssetMenu(fileName = "HealAttackUpgrade", menuName = "Scriptable Objects/Player Upgrades/Heal Attack Upgrade")]
public class HealAttackUpgrade : Upgrade
{
    public override void Apply(PlayerUpgradeSystem upgradeSystem)
    {
        upgradeSystem.Combat.AddCombinationEffect(upgradeSystem.CombinationEffects.HealEffect);
    }

    public override void Unapply(PlayerUpgradeSystem upgradeSystem)
    {
        upgradeSystem.Combat.RemoveCombinationEffect(upgradeSystem.CombinationEffects.HealEffect);
    }
}
