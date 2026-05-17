using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
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
        health = GetComponentInParent<Health>();
        Debug.Assert(health != null, "HealthColorChanger: parent object must have health!");
    }
    
    void OnEnable()
    {
        if (health != null)
            health.HealthChanged += ChangeColorOnHealthChanged;
    }

    void OnDisable()
    {
        if (health != null)
            health.HealthChanged -= ChangeColorOnHealthChanged;
    }
    
    private void ChangeColorOnHealthChanged(float healthValue)
    {
        Color newColor = Color.Lerp(toColor, fromColor, healthValue / health.MaxHealth);
        spriteRenderer.color = newColor;
    }
}