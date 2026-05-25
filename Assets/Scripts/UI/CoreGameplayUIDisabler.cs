using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CoreGameplayUIDisabler : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    [SerializeField] private PlayerLifecycleHandler lifecycleHandler;
    [SerializeField] private PauseMenuUI pauseMenu;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        lifecycleHandler.Died += DisableUI;
        lifecycleHandler.Revived += EnableUI;
        pauseMenu.PauseMenuOpened += DisableUI;
        pauseMenu.PauseMenuClosed += EnableUI;
    }

    void OnDisable()
    {
        lifecycleHandler.Died -= DisableUI;
        lifecycleHandler.Revived -= EnableUI;
        pauseMenu.PauseMenuOpened -= DisableUI;
        pauseMenu.PauseMenuClosed -= EnableUI;
    }

    private void DisableUI()
    {
        canvasGroup.SetVisible(false);
    }

    private void EnableUI()
    {
        canvasGroup.SetVisible(true);
    }
}