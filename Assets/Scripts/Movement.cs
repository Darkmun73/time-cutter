using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Directions))]
public class Movement : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Directions directions;

    [SerializeField] private MovementData data;
    public bool IsTouchingGround => rigidBody.IsTouching(data.Ground);
    private float jumpForce;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        directions = GetComponent<Directions>();
        SetUpJumpForce();
    }

    private void SetUpJumpForce()
    {
        jumpForce = 4 * data.JumpHeight / data.JumpTime;
        rigidBody.gravityScale = 8 * data.JumpHeight / Mathf.Pow(data.JumpTime, 2) / (-Physics2D.gravity.y); 
    }

    public void SetUpDirections(float horizontal, float vertical)
    {
        directions.SetUpDirections(horizontal, vertical);
    }

    public Direction GetMovementDirection()
    {
        return directions.MovementDirection;
    }

    public void Jump()
    {
        if (IsTouchingGround)
        {
            rigidBody.linearVelocityY = jumpForce;
        }
    }

    public void MoveHorizontal()
    {
        rigidBody.linearVelocityX = directions.MovementDirection.ToVector().x * data.MovementSpeed;
    }

    public void AdjustHorizontalSpeed(float delta)
    {
        rigidBody.linearVelocityX += directions.MovementDirection.ToVector().x * delta;
    }

    public void StopHorizontalMovement()
    {
        rigidBody.linearVelocityX = 0;
    }

    public void ClampFallSpeed()
    {
        if (rigidBody.linearVelocityY < -data.MaxFallSpeed)
            rigidBody.linearVelocityY = -data.MaxFallSpeed;
    }
}
