using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IPlayerActions, GameInput.IPauseMenuActions, GameInput.IUpgradeMenuActions, GameInput.ITutorialActions
{
    public event UnityAction<Vector2> MovingAndLooking;
    public event UnityAction Jumping;
    public event UnityAction AttackInitializing;
    public event UnityAction AttackInitialized;
    public event UnityAction TogglingPauseMenu;
    public event UnityAction TogglingUpgradeMenu;
    public event UnityAction Continuing;

    private GameInput gameInput;

    private void OnEnable()
    {
        if (gameInput == null)
        {
            gameInput = new GameInput();
            gameInput.Player.SetCallbacks(this);
            gameInput.PauseMenu.SetCallbacks(this);
            gameInput.UpgradeMenu.SetCallbacks(this);
            gameInput.Tutorial.SetCallbacks(this);
        }
        gameInput.Player.Enable();
        gameInput.PauseMenu.Enable();
        gameInput.UpgradeMenu.Enable();
        gameInput.Tutorial.Enable();

        DebugLogMapsEnabled();
    }

    private void OnDisable()
    {
        gameInput.Player.Disable();
        gameInput.PauseMenu.Disable();
        gameInput.UpgradeMenu.Disable();
        gameInput.Tutorial.Disable();
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

    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TogglingPauseMenu?.Invoke();
        }
    }

    public void OnUpgradeMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TogglingUpgradeMenu?.Invoke();
        }
    }

    public void OnContinue(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Continuing?.Invoke();
        }
    }

    public void ToggleUpgradeMenuMap()
    {
        if (gameInput.PauseMenu.enabled && gameInput.Player.enabled && gameInput.Tutorial.enabled)
        {
            gameInput.PauseMenu.Disable();
            gameInput.Player.Disable();
            gameInput.Tutorial.Disable();
        } else
        {
            gameInput.PauseMenu.Enable();
            gameInput.Player.Enable();
            gameInput.Tutorial.Enable();
        }
        //DebugLogMapsEnabled();
    }

    public void TogglePauseMenuMap()
    {
        if (gameInput.UpgradeMenu.enabled && gameInput.Player.enabled && gameInput.Tutorial.enabled)
        {
            gameInput.UpgradeMenu.Disable();
            gameInput.Player.Disable();
            gameInput.Tutorial.Disable();
        } else
        {
            gameInput.UpgradeMenu.Enable();
            gameInput.Player.Enable();
            gameInput.Tutorial.Enable();
        }
        //DebugLogMapsEnabled();
    }

    public void ToggleTutorial()
    {
        if (gameInput.UpgradeMenu.enabled && gameInput.Player.enabled && gameInput.PauseMenu.enabled)
        {
            gameInput.UpgradeMenu.Disable();
            gameInput.Player.Disable();
            gameInput.PauseMenu.Disable();
        } else
        {
            gameInput.UpgradeMenu.Enable();
            gameInput.Player.Enable();
            gameInput.PauseMenu.Enable();
        }
    }

    private void DebugLogMapsEnabled()
    {
        Debug.Log($"Player map: {gameInput.Player.enabled}");
        Debug.Log($"UpgradeMenu map: {gameInput.UpgradeMenu.enabled}");
        Debug.Log($"PauseMenu map: {gameInput.PauseMenu.enabled}");
    }

}
