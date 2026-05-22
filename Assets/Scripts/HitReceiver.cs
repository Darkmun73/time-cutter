using UnityEngine;
using UnityEngine.Events;

public class HitReceiver : MonoBehaviour
{
    public event UnityAction<HitInfo> HitReceived;

    public Transform LastHitSource {get; private set;}

    private IHitInfoVisitor[] visitors;

    void Awake()
    {
        visitors = GetComponents<IHitInfoVisitor>();
    }

    public void ReceiveHit(HitInfo hitInfo)
    {
        LastHitSource = hitInfo.Source;

        foreach (var visitor in visitors)
            hitInfo.Accept(visitor);
        
        HitReceived?.Invoke(hitInfo);
    }
}