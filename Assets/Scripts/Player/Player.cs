using UnityEngine;


public enum Direction {Left, Right, Up, Down}

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    private Rigidbody2D rigidBody;

    [field: SerializeField] public PlayerData Data {get; private set;}
    [SerializeField] private PlayerEvents playerEvents;

    [SerializeField] private GameObject look;

    public bool IsTouchingGround => rigidBody.IsTouching(Data.Ground);

    private Direction moveDirection = Direction.Right;
    public Direction MoveDirection {
        get { return moveDirection; }
        set
        {
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
            playerEvents.OnHealthChanged(value);
        }
    }

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Health = Data.MaxHealth;
        SetUpJumpForce();
    }

    private void SetUpJumpForce()
    {
        JumpForce = 4 * Data.JumpHeight / Data.JumpTime;
        rigidBody.gravityScale = 8 * Data.JumpHeight / Mathf.Pow(Data.JumpTime, 2) / (-Physics2D.gravity.y); 
    }

    private void HorizontalFlip()
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

    public void Die()
    {
        // stab
        Debug.Log("died");
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }
}
