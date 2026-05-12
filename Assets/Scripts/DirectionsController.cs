using UnityEngine;
using UnityEngine.Events;

public class DirectionsController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private Direction lookDirection = Direction.Right;
    public Direction LookDirection {get => lookDirection; private set => lookDirection = value;}

    private Direction movementDirection = Direction.Right;
    public Direction MovementDirection {
        get => movementDirection;
        private set
        {
            if ((movementDirection == Direction.Left && value == Direction.Right) ||
                (movementDirection == Direction.Right && value == Direction.Left))
                MovementDirectionFlipped?.Invoke();
            movementDirection = value;
        }
    }

    public event UnityAction MovementDirectionFlipped;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Debug.DrawRay(transform.position, lookDirection.ToVector() * 5f, Color.yellow);
    }

    // horizontal: 1 - right, -1 - left; vertical: 1 - up, -1 - down
    public void SetUpDirections(float horizontal, float vertical)
    {
        if (horizontal == 1f)
        {
            MovementDirection = Direction.Right;
            lookDirection = Direction.Right;
        }
        else if (horizontal == -1f)
        {
            MovementDirection = Direction.Left;
            lookDirection = Direction.Left;
        }

        if (vertical == 1f)
            lookDirection = Direction.Up;
        else if (vertical == -1f)
            lookDirection = Direction.Down;

        if (horizontal == 0f && vertical == 0f)
            lookDirection = movementDirection;
    }

    public void FaceTarget(Transform target)
    {
        var horizontalInput =  transform.position.x > target.position.x ? -1f : 1f;
        Direction newMovementDirection = DirectionHelper.FromVector(new(horizontalInput, 0));
        Direction currenMovementtDirection = MovementDirection;

        // same direction = don't need to change it
        if (newMovementDirection != currenMovementtDirection)
        {
            SetUpDirections(horizontalInput, 0f);
        }
    }
}