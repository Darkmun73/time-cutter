using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DebugTools : MonoBehaviour
{
    public event UnityAction DebugToolsEnabled;
    public event UnityAction DebugToolsDisabled;

    [SerializeField] private GameObject simpleEnemyPrefab;
    [SerializeField] private GameObject playerPrefab;

    [Header("Keys")]
    [SerializeField] private Key toggleDebugToolsKey; 
    [SerializeField] private Key spawnEnemyAtCursorKey; 

    private InputAction debugSpawnEnemyAction = null;
    private InputAction debugToolsToggle = null;

    private bool isDebugToolsEnabled = false;

    private Currency playerCurrency;
    private Movement playerMovement;

    void Awake()
    {
        var player = FindFirstObjectByType<Player>();

        playerCurrency = player.GetComponent<Currency>();
        playerMovement = player.GetComponent<Movement>();
    }

    private void OnEnable()
    {
        CreateDebugInput();
        EnableDebugTools();
        EnableDebugToolsToggle();
    }

    private void OnDisable()
    {
        DisableDebugTools();
        DisableDebugToolsToggle();
    }

    private void EnableDebugTools()
    {
        EnableDebugInput();
        isDebugToolsEnabled = true;
        DebugToolsEnabled?.Invoke();
    }

    private void DisableDebugTools()
    {
        DisableDebugInput();
        isDebugToolsEnabled = false;
        DebugToolsDisabled?.Invoke();
    }

    private void EnableDebugToolsToggle()
    {
        debugToolsToggle.Enable();
    }

    private void DisableDebugToolsToggle()
    {
        debugToolsToggle.Disable();
    }

    private void CreateDebugInput()
    {
        debugToolsToggle = new InputAction("DebugToolsToggle", type: InputActionType.Button);
        debugToolsToggle.AddBinding(Keyboard.current[toggleDebugToolsKey]);
        debugToolsToggle.performed += OnDebugToolsToggle;

        debugSpawnEnemyAction = new InputAction("DebugSpawnEnemy", type: InputActionType.Button);
        debugSpawnEnemyAction.AddBinding(Keyboard.current[spawnEnemyAtCursorKey]);
        debugSpawnEnemyAction.performed += OnDebugSpawnEnemy;
    }

    private void EnableDebugInput()
    {
        debugSpawnEnemyAction.Enable();
    }

    private void DisableDebugInput()
    {
        debugSpawnEnemyAction.Disable();
    }

    private void OnDebugSpawnEnemy(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SpawnSimpleEnemyAtCursor();
        }
    }

    private void OnDebugToolsToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleDebugTools();
        }
    }

    private void ToggleDebugTools()
    {
        if (isDebugToolsEnabled)
            DisableDebugTools();
        else
            EnableDebugTools();
    }

    private void SpawnSimpleEnemyAtCursor()
    {
        if (simpleEnemyPrefab == null)
        {
            Debug.LogWarning("SimpleEnemy prefab is not assigned in DebugTools.");
            return;
        }

        if (Pointer.current == null)
        {
            Debug.LogWarning("Pointer device is unavailable for debug enemy spawn.");
            return;
        }

        Camera currentCamera = Camera.main;
        if (currentCamera == null)
        {
            Debug.LogWarning("Main Camera was not found for debug enemy spawn.");
            return;
        }

        Vector2 cursorScreenPosition = Pointer.current.position.ReadValue();
        Vector3 cursorWorldPosition = currentCamera.ScreenToWorldPoint(
            new Vector3(cursorScreenPosition.x, cursorScreenPosition.y, currentCamera.nearClipPlane));
        cursorWorldPosition.z = simpleEnemyPrefab.transform.position.z;

        Instantiate(simpleEnemyPrefab, cursorWorldPosition, Quaternion.identity);
    }

    public void IncreaseAmountOfPlayerCurrency(int value)
    {
        playerCurrency.Amount += value;
    }

    public void DecreaseAmountOfPlayerCurrency(int value)
    {
        playerCurrency.Amount -= value;
    }

    public void MakePlayerJump()
    {
        playerMovement.TryJump();
    }

    public void LaunchPlayerUp(float force)
    {
        playerMovement.LaunchUp(force);
    }

    public void SpawnPlayer()
    {
        Instantiate(playerPrefab, new Vector3(-12.71f, -2.938f, 0), Quaternion.identity);
    }

    public void KillPlayer()
    {
        FindFirstObjectByType<Player>().GetComponent<PlayerLifecycleHandler>().Die();
    }

    public void RevivePlayer()
    {
        FindFirstObjectByType<Player>().GetComponent<PlayerLifecycleHandler>().Revive();
    }
}