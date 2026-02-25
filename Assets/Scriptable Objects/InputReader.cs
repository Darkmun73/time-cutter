using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IPlayerActions
{
    public event UnityAction<float> Moving;
    public event UnityAction Jumping;
    public event UnityAction AttackInitializing;
    public event UnityAction AttackInitialized;

    private GameInput gameInput;

    private void OnEnable()
    {
        if (gameInput == null)
        {
            gameInput = new GameInput();
            gameInput.Player.SetCallbacks(this);
        }
        gameInput.Player.Enable();
    }

    private void OnDisable()
    {
        gameInput.Player.Disable();
    }

    public void OnMoving(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            Moving.Invoke(context.ReadValue<float>());
        }
    }

    public void OnJumping(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Jumping.Invoke();
        }
    }

    public void OnAttacking(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            AttackInitializing.Invoke();
        }
        else if (context.canceled)
        {
            AttackInitialized.Invoke();
        }
    }
}
