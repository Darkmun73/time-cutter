using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DirectionsController))]
public class Movement : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    [SerializeField] private MovementData data;

    public bool IsTouchingGround => rigidBody.IsTouching(data.Ground);
    private bool isTouchingGroundPreviousFrame = false;

    private bool canJump = true;
    private bool CanJump
    {
        get => IsTouchingGround && canJump; // not obvious
        set => canJump = value;
    }

    private float jumpForce;

    public event UnityAction Jumped;
    public event UnityAction GroundTouched;
    public event UnityAction GroundLeft;
    

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        SetUpJumpForce();
        isTouchingGroundPreviousFrame = IsTouchingGround;
    }

    void FixedUpdate()
    {
        if (IsTouchingGround == isTouchingGroundPreviousFrame) return;
        
        isTouchingGroundPreviousFrame = IsTouchingGround;
        
        if (IsTouchingGround)
            GroundTouched?.Invoke();
        else
            GroundLeft?.Invoke();
    }

    private void SetUpJumpForce()
    {
        jumpForce = 4 * data.JumpHeight / data.JumpTime;
        rigidBody.gravityScale = 8 * data.JumpHeight / Mathf.Pow(data.JumpTime, 2) / (-Physics2D.gravity.y); 
    }

    public void TryJump()
    {
        if (CanJump)
            Jump();
    }

    private void Jump()
    {
        rigidBody.linearVelocityY = jumpForce;
        Jumped?.Invoke();
    }

    public void AllowJump()
    {
        CanJump = true;
    }

    public void ProhibitJump()
    {
        CanJump = false;
    }

    public void ProhibitJump(float seconds)
    {
        StartCoroutine(ProhibitJumpRoutine(seconds));
    }

    private IEnumerator ProhibitJumpRoutine(float seconds)
    {
        ProhibitJump();
        yield return new WaitForSeconds(seconds);
        AllowJump();
    }

    public void LaunchUp(float force)
    {
        rigidBody.linearVelocityY = force;
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

    public float GetYVelocity()
    {
        return rigidBody.linearVelocityY;
    }
}
