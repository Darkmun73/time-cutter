using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ChildObjectsActivator))]
public class DeathMenu : MonoBehaviour
{
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;

    [SerializeField] private PlayerLifecycleHandler lifecycleHandler;
    private ChildObjectsActivator childObjectsActivator;

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
        lifecycleHandler.Died += OnDied;
        lifecycleHandler.Revived += OnRevived;
        playAgainButton.onClick.AddListener(lifecycleHandler.Revive);
        mainMenuButton.onClick.AddListener(SceneLoader.LoadMainMenu);
    }

    void OnDisable()
    {
        lifecycleHandler.Died -= OnDied;
        lifecycleHandler.Revived -= OnRevived;
        playAgainButton.onClick.RemoveListener(lifecycleHandler.Revive);
        mainMenuButton.onClick.RemoveListener(SceneLoader.LoadMainMenu);
    }

    private void OnDied()
    {
        childObjectsActivator.SetActive(true);
    }

    private void OnRevived()
    {
        childObjectsActivator.SetActive(false);
    }
}