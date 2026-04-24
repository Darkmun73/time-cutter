using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Health))]
public class HealthColorChanger : MonoBehaviour
{
    [SerializeField] private Color fromColor;
    [SerializeField] private Color toColor;

    private SpriteRenderer spriteRenderer;
    private Health health;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = fromColor;
        health = GetComponent<Health>();
    }
    
    void OnEnable()
    {
        health.HealthChanged += ChangeColorOnHealthChanged;
    }

    void OnDisable()
    {
        health.HealthChanged -= ChangeColorOnHealthChanged;
    }
    
    private void ChangeColorOnHealthChanged(float healthValue)
    {
        Color newColor = Color.Lerp(toColor, fromColor, healthValue / health.GetMaxHealth());
        spriteRenderer.color = newColor;
    }
}