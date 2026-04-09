using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DirectionsController))]
public class Movement : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    [SerializeField] private MovementData data;
    public bool IsTouchingGround => rigidBody.IsTouching(data.Ground);
    private float jumpForce;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        SetUpJumpForce();
    }

    private void SetUpJumpForce()
    {
        jumpForce = 4 * data.JumpHeight / data.JumpTime;
        rigidBody.gravityScale = 8 * data.JumpHeight / Mathf.Pow(data.JumpTime, 2) / (-Physics2D.gravity.y); 
    }

    public void Jump()
    {
        if (IsTouchingGround)
        {
            rigidBody.linearVelocityY = jumpForce;
        }
    }

    // If direction up or down, then movement stoping
    public void MoveHorizontal(Direction direction)
    {
        rigidBody.linearVelocityX = direction.ToVector().x * data.MovementSpeed;
    }

    public void AdjustHorizontalSpeed(Direction direction, float delta)
    {
        rigidBody.linearVelocityX += direction.ToVector().x * delta;
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
