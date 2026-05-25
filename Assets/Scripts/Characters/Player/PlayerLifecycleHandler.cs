using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerController))]
public class PlayerLifecycleHandler : MonoBehaviour
{
    public bool IsDead {get; private set;} = false;

    private Health health;
    private PlayerController playerController;

    public event UnityAction Died;
    public event UnityAction Revived;

    void Awake()
    {
        health = GetComponent<Health>();
        playerController = GetComponent<PlayerController>();
    }

    public void Die()
    {
        IsDead = true;
        playerController.Reset();
        SetAllComponentsEnabled(false);
        Died?.Invoke();
    }

    public void Revive()
    {
        IsDead = false;
        SetAllComponentsEnabled(true);
        health.Reset();
        Revived?.Invoke();
    }

    private void SetAllComponentsEnabled(bool enable)
    {
        var components = GetComponents<Component>();
        foreach (var component in components)
        {
            if (component is Behaviour behaviour)
                behaviour.enabled = enable;
            else if (component is Renderer renderer)
                renderer.enabled = enable;
            else if (component is Rigidbody2D rigidbody2D)
                rigidbody2D.simulated = enable;
        }
    }
}
