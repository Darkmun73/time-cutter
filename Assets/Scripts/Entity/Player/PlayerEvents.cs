using UnityEngine;
using UnityEngine.Events;

public class PlayerEvents : EntityEvents
{
    public event UnityAction<GameObject> HitOccured;

    public void OnHitOccured(GameObject hitObject)
    {
        HitOccured?.Invoke(hitObject);
    }
}
