using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Entity : MonoBehaviour
{

    [field: SerializeField] public EntityData Data {get; private set;}
    protected abstract EntityEvents Events {get;}

    

    protected virtual void Awake() {}

    protected virtual void Start()
    {
    }

    public abstract void Die();

    public void HandleHit(Transform from, float damage) // TODO: переименовать в Receive и в PlayerCombat переименовать функцию
    {
        //TakeDamage(damage);
        Events.OnGetHitted(from);
    }
}