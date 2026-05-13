using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CharactersRenderOrderController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private static int sortOrderCounter = 0;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sortingOrder = sortOrderCounter;
        sortOrderCounter++;
        if (sortOrderCounter > short.MaxValue)
        {
            sortOrderCounter = short.MinValue;
        }
    }
}
