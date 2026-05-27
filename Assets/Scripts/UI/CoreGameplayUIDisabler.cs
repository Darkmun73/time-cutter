using UnityEngine;

// TODO: это не disabler, а hider, выходит. Ведь можно все еще нажать на tab и поставить на паузу игру. Вообще надо паузу из UI меню апгрейдов перенести
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

    public void DisableUI()
    {
        canvasGroup.SetVisible(false);
    }

    public void EnableUI()
    {
        canvasGroup.SetVisible(true);
    }
}