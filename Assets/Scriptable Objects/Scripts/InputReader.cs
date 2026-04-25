using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IPlayerActions
{
    public event UnityAction<Vector2> MovingAndLooking;
    public event UnityAction Jumping;
    public event UnityAction AttackInitializing;
    public event UnityAction AttackInitialized;
    public event UnityAction DebugSpawnEnemyRequested;

    private GameInput gameInput;
    private InputAction debugSpawnEnemyAction = null;

    private void OnEnable()
    {
        if (gameInput == null)
        {
            gameInput = new GameInput();
            gameInput.Player.SetCallbacks(this);
        }
        gameInput.Player.Enable();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        EnableDebugInput();
#endif
    }

    private void OnDisable()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        DisableDebugInput();
#endif

        gameInput.Player.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Jumping?.Invoke();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            AttackInitializing?.Invoke();
        }
        else if (context.canceled)
        {
            AttackInitialized?.Invoke();
        }
    }

    public void OnMoveAndLook(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            MovingAndLooking?.Invoke(context.ReadValue<Vector2>());
        }
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private void EnableDebugInput()
    {
        debugSpawnEnemyAction = new InputAction("DebugSpawnEnemy", binding: "<Mouse>/rightButton");
        debugSpawnEnemyAction.performed += OnDebugSpawnEnemy;
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
            DebugSpawnEnemyRequested?.Invoke();
        }
    }
#endif
}
