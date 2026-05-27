using UnityEngine;

public static class CanvasGroupExtensions // TODO: убрать?
{
    public static void SetVisible(this CanvasGroup canvasGroup, bool visible) 
    {
        canvasGroup.alpha = visible ? 1 : 0;
        canvasGroup.blocksRaycasts = visible;
        canvasGroup.interactable = visible;
    }
}