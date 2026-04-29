using UnityEngine;

[CreateAssetMenu(fileName = "HealthUpgrade", menuName = "Scriptable Objects/Player Upgrades/Health Upgrade")]
public class HealthUpgrade : Upgrade
{
    [SerializeField] public float multiplier = 1f;

    public override void Apply(PlayerUpgradeSystem upgradeSystem)
    {
        upgradeSystem.Health.IncreaseMaxHealthByFactor(multiplier);
    }

    public override void Unapply(PlayerUpgradeSystem upgradeSystem)
    {
        upgradeSystem.Health.DecreaseMaxHealthByFactor(multiplier);
    }
}
