using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Entity
{
    public readonly PlayerEvents events = new();
    protected override EntityEvents Events => events;

    public override void Die()
    {
        //stab
    }
}
