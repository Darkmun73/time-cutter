using UnityEngine;
using UnityEngine.Events;

public interface IHittable
{
    public event UnityAction<Transform> HitReceived;
    void ReceiveHit(Transform from, float damage); // MAYBE TODO: структуру HitInfo
}