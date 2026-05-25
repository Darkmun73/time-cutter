using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(ChildObjectsActivator))]
public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;

    [SerializeField] private InputReader inputReader;
    private ChildObjectsActivator childObjectsActivator;

    private bool isMenuOpen = false;

    public UnityAction PauseMenuOpened;
    public UnityAction PauseMenuClosed;

    void Awake()
    {
        childObjectsActivator = GetComponent<ChildObjectsActivator>();
    }

    void Start()
    {
        childObjectsActivator.SetActive(false);
    }

    void OnEnable()
    {
        inputReader.TogglingPauseMenu += ToggleMenu;
        continueButton.onClick.AddListener(ToggleMenu);
        mainMenuButton.onClick.AddListener(SceneLoader.LoadMainMenu);
    }

    void OnDisable()
    {
        inputReader.TogglingPauseMenu -= ToggleMenu;
        continueButton.onClick.RemoveListener(ToggleMenu);
        mainMenuButton.onClick.RemoveListener(SceneLoader.LoadMainMenu);

        if (isMenuOpen)
            ToggleMenu();
    }

    private void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;

        childObjectsActivator.SetActive(isMenuOpen);

        inputReader.TogglePauseMenuMap();
        if (isMenuOpen)
            PauseMenuOpened?.Invoke();
        else
            PauseMenuClosed?.Invoke();
        
        Time.timeScale = isMenuOpen ? 0f : 1f;
    }

}