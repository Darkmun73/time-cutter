using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerController))]
public class PlayerLifecycleHandler : MonoBehaviour // TODO: переделать в более generic LifycycleHandler
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
        gameObject.SetAllComponentsEnabled(false, this);
        Died?.Invoke();
    }

    public void Revive()
    {
        IsDead = false;
        gameObject.SetAllComponentsEnabled(true);
        health.Reset();
        //SceneLoader.LoadLevel1();
        Revived?.Invoke();
    }
}
