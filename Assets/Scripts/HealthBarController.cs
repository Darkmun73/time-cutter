using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private Slider healthBar;

    [SerializeField] private PlayerData playerData;
    [SerializeField] private PlayerEvents playerEvents;

    void Awake()
    {
        healthBar = GetComponent<Slider>();
        SetMaxHealth(playerData.MaxHealth);
    }

    void OnEnable()
    {
        playerEvents.HealthChanged += SetHealth;
    }

    void OnDisable()
    {
        playerEvents.HealthChanged -= SetHealth;
    }

    void SetMaxHealth(float value)
    {
        healthBar.maxValue = value;
    }

    void SetHealth(float value)
    {
        healthBar.value = value;
    }




}
