using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MovementSpriteFlipper : MonoBehaviour
{
    [SerializeField] private DirectionsController directions;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // TODO: скорее всего стоит перенести в другое место
    }

    void OnEnable()
    {
        directions.MovementDirectionFlipped += FlipSprite;
    }

    void OnDisable()
    {
        directions.MovementDirectionFlipped -= FlipSprite;
    }

    private void FlipSprite()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}