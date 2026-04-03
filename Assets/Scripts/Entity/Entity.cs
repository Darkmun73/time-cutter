using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Entity : MonoBehaviour
{

    private Rigidbody2D rigidBody;
    public Rigidbody2D RigidBody => rigidBody;

    [field: SerializeField] public EntityData Data {get; private set;}
    protected abstract EntityEvents Events {get;}

    public bool IsTouchingGround => rigidBody.IsTouching(Data.Ground);

    private Direction moveDirection = Direction.Right;
    public Direction MoveDirection {
        get { return moveDirection; }
        set
        {
            if ((moveDirection == Direction.Left && value == Direction.Right) ||
                (moveDirection == Direction.Right && value == Direction.Left))
                HorizontalFlip();
            moveDirection = value;
            LookDirection = value;
        }
    }

    public Direction LookDirection { get; set; } = Direction.Right;

    public float JumpForce {get; private set;}

    private float health;
    public float Health
    {
        get { return health; }
        set
        {
            if (value <= 0)
            {
                health = 0;
                Die();
            }
            else if (value > Data.MaxHealth)
                health = Data.MaxHealth;
            else
                health = value;
            Events.OnHealthChanged(value);
        }
    }

    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        Health = Data.MaxHealth;
        SetUpJumpForce();
    }

    private void SetUpJumpForce()
    {
        JumpForce = 4 * Data.JumpHeight / Data.JumpTime;
        rigidBody.gravityScale = 8 * Data.JumpHeight / Mathf.Pow(Data.JumpTime, 2) / (-Physics2D.gravity.y); 
    }

    private void HorizontalFlip() // TODO: не касается движения, перенести отдельно?
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void ChangeMoveDirection()
    {
        HorizontalFlip();
        switch (MoveDirection)
        {
            
            case Direction.Left:
                MoveDirection = Direction.Right;
                break;
            case Direction.Right:
                MoveDirection = Direction.Left;
                break;
        }

        if (LookDirection != Direction.Up && LookDirection != Direction.Down)
            LookDirection = MoveDirection;
    }

    public abstract void Die();

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }

    public void HandleHit(Transform from, float damage) // TODO: переименовать в Receive и в PlayerCombat переименовать функцию
    {
        TakeDamage(damage);
        Events.OnGetHitted(from);
    }
}
