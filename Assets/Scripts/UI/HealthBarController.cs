using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBarController : MonoBehaviour
{
    private Slider healthBar;
    [SerializeField] private bool showHPNumbers = false;
    [SerializeField] private TextMeshProUGUI hpNumbersTextField;

    [SerializeField] private Health health;

    void Awake()
    {
        healthBar = GetComponent<Slider>();
    }

    void Start()
    {
        SetMaxHealth(health.MaxHealth);
        SetHealth(health.CurrentHealth);
    }

    void OnEnable()
    {
        health.MaxHealthChanged += SetMaxHealth;
        health.HealthChanged += SetHealth;
    }

    void OnDisable()
    {
        health.MaxHealthChanged -= SetMaxHealth;
        health.HealthChanged -= SetHealth;
    }

    void SetMaxHealth(float value)
    {
        healthBar.maxValue = value;
        if (showHPNumbers)
            hpNumbersTextField.text = $"{healthBar.value}/{healthBar.maxValue}";
    }

    void SetHealth(float value)
    {
        healthBar.value = value;
        if (showHPNumbers)
            hpNumbersTextField.text = $"{healthBar.value}/{healthBar.maxValue}";
    }
}
