using UnityEngine;
using UnityEngine.Events;

public class EntityEvents
{
    public event UnityAction<float> HealthChanged;
    public event UnityAction Died;
    public event UnityAction<Transform> GetHitted;


    public void OnHealthChanged(float value)
    {
        HealthChanged?.Invoke(value);
    }

    public void OnDied()
    {
        Died?.Invoke();
    }

    public void OnGetHitted(Transform from)
    {
        GetHitted?.Invoke(from);
    }
}
