using UnityEngine;

[CreateAssetMenu(fileName = "HealthUpgrade", menuName = "Scriptable Objects/Upgrades/Health Upgrade")]
public class HealthUpgrade : Upgrade
{
    [field: SerializeField] public float Multiplier { get; private set; } = 1f;

    public override void Apply(PlayerUpgradeSystem upgradeSystem)
    {
        upgradeSystem.Health.IncreaseMaxHealthByFactor(Multiplier);
    }

    public override void Unapply(PlayerUpgradeSystem upgradeSystem)
    {
        upgradeSystem.Health.DecreaseMaxHealthByFactor(Multiplier);
    }
}
