using UnityEngine;

public class EnemyHitInfo : HitInfo
{
    public EnemyHitInfo(Transform source, float damage) : base(source, damage) {}

    public override void Accept(IHitInfoVisitor visitor)
    {
        visitor.Visit(this);
    }
}