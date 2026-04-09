using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private IState currentState;
    public IState CurrentState
    {
        get { return currentState; }
        set
        {
            //Debug.Log(value);
            currentState?.Exit();
            currentState = value;
            currentState.Enter();
        }
    }

    protected virtual void Update()
    {
        currentState.LogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        currentState.PhysicsUpdate();
    }
}
