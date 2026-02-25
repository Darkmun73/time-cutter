using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerEvents", menuName = "Scriptable Objects/Player Events")]
public class PlayerEvents : ScriptableObject
{
    public event UnityAction<float> HealthChanged;
    public event UnityAction PlayerDied;

    public void OnHealthChanged(float value)
    {
        HealthChanged.Invoke(value);
    }

    public void OnPlayerDied()
    {
        PlayerDied.Invoke();
    }
}
