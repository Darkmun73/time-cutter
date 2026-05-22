using UnityEngine;

public class PlayerHitInfo : HitInfo
{
    public GameObject HitObject {get;}
    public float Angle {get;}

    public PlayerHitInfo(Transform source, float damage, GameObject hitObject, float angle) : base(source, damage)
    {
        HitObject = hitObject;
        Angle = angle;
    }

    public override void Accept(IHitInfoVisitor visitor)
    {
        visitor.Visit(this);
    }
}