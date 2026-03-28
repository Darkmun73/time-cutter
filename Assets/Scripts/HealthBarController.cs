using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private Slider healthBar;

    private Player player;

    void Awake()
    {
        healthBar = GetComponent<Slider>();

        player = FindFirstObjectByType<Player>();

        SetMaxHealth(player.Data.MaxHealth);
    }

    void OnEnable()
    {
        player.events.HealthChanged += SetHealth;
    }

    void OnDisable()
    {
        player.events.HealthChanged -= SetHealth;
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
