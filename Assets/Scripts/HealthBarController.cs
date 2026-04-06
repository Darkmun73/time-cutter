using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private Slider healthBar;

    private Health playerHealth;

    void Awake()
    {
        healthBar = GetComponent<Slider>();

        playerHealth = FindFirstObjectByType<Player>().GetComponent<Health>();


        SetMaxHealth(playerHealth.GetMaxHealth());
        SetHealth(playerHealth.GetMaxHealth());
    }

    void OnEnable()
    {
        playerHealth.HealthChanged += SetHealth;
    }

    void OnDisable()
    {
        playerHealth.HealthChanged -= SetHealth;
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
