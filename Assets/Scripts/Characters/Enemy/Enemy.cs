using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(HitReceiver))]
[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(ChildObjectsActivator))]
public class Enemy : MonoBehaviour, IMortal
{
    private Health health;
    private HitReceiver hitReceiver;
    private EnemyController enemyController;
    private ChildObjectsActivator childObjectsActivator;
    private PlayerLifecycleHandler playerLifecycleHandler;

    [SerializeField] private int currencyReward;
    private Vector3 lastSavePosition;
    public bool DestroyOnDeath {get; set;} = false;

    public bool IsDead {get; private set;} = false;
    
    public event UnityAction Died;
    public event UnityAction Revived;

    void Awake()
    {
        health = GetComponent<Health>();
        hitReceiver = GetComponent<HitReceiver>();
        enemyController = GetComponent<EnemyController>();
        childObjectsActivator = GetComponent<ChildObjectsActivator>();
        playerLifecycleHandler = FindFirstObjectByType<Player>().GetComponent<PlayerLifecycleHandler>();
        lastSavePosition = transform.position;
    }

    void OnEnable()
    {
        health.HealthDepleted += Die;
        playerLifecycleHandler.Revived += OnPlayerRevived;
    }

    void OnDisable()
    {
        health.HealthDepleted -= Die;
        playerLifecycleHandler.Revived -= OnPlayerRevived;
    }

    public void Die()
    {
        Debug.Log("Enemy died");
        if (hitReceiver.LastHitSource != null && hitReceiver.LastHitSource.TryGetComponent<Player>(out var player))
        {
            var playerCurrency = player.GetComponent<Currency>();
            Debug.Assert(playerCurrency != null, "Enemy: Player must have currency!");
            playerCurrency.Amount += currencyReward;
        }

        IsDead = true;
        enemyController.Reset();
        childObjectsActivator.SetActive(false);
        gameObject.SetAllComponentsEnabled(false, this);

        Died?.Invoke();
        if (DestroyOnDeath)
        {
            Destroy(gameObject);
        }
    }

    public void Revive()
    {
        Debug.Log("reviving");
        IsDead = false;
        gameObject.SetAllComponentsEnabled(true);
        childObjectsActivator.SetActive(true);
        health.Reset();
        //SceneLoader.LoadLevel1();
        Revived?.Invoke();
    }

    private void OnPlayerRevived()
    {
        transform.position = lastSavePosition;
        if (IsDead)
            Revive();
        else
            health.Reset();
    }
}
