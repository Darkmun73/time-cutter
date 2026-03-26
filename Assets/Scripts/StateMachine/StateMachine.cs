public class StateMachine
{
    private BaseState currentState;
    // public BaseState CurrentState
    // {
    //     get { return currentState; }
    //     set
    //     {
    //         currentState.Exit();
    //         currentState = value;
    //         currentState.Enter();
    //     }
    // }

    public StateMachine(BaseState initialState)
    {
        currentState = initialState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState.Update();
    }

    public void FixedUpdate()
    {
        currentState.FixedUpdate();
    }

    public void SetState(BaseState state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    } 
}
