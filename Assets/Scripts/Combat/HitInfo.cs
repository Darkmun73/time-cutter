using UnityEngine;

public abstract class HitInfo
{
    public Transform Source {get;}
    public float Damage {get;}
    public abstract void Accept(IHitInfoVisitor visitor);

    protected HitInfo(Transform source, float damage)
    {
        Source = source;
        Damage = damage;
    }
}