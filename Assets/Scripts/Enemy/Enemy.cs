using UnityEngine;

public class Enemy : MonoBehaviour
{
    [field: SerializeField] public EnemyData Data {get; private set;}

    private StateMachine stateMachine;

    void Awake()
    {
        var idleState = new EnemyIdleState(this);

        stateMachine = new(idleState);
    }

    void Update()
    {
        stateMachine.Update();
    }

    void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
}
