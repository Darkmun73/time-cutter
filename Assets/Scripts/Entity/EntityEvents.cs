using UnityEngine;
using UnityEngine.Events;

public class EntityEvents
{
    public event UnityAction<float> HealthChanged;
    public event UnityAction Died;

    public void OnHealthChanged(float value)
    {
        HealthChanged?.Invoke(value);
    }

    public void OnDied()
    {
        Died?.Invoke();
    }
}
