using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // components
    private Rigidbody2D rb;

    // other
    [SerializeField] private ContactFilter2D ground;
    [field: SerializeField] public InputReader InputReader {get; private set;}

    public bool IsTouchingGround => rb.IsTouching(ground);
    public bool IsFacingRight {get; private set;}

    [SerializeField] private float maxFallSpeed = 10f;

    // move
    [SerializeField] private float movementSpeed = 10f;
    private float horizontalMove;

    // jump
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpTime = 1f;
    private float jumpForce;

    void Awake()
    {
        SetUpJumpForce();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        InputReader.moveEvent += SetHorizontalMove;
        InputReader.jumpEvent += Jump;
    }

    void FixedUpdate()
    {
        if ((IsFacingRight && horizontalMove < 0) || 
            (!IsFacingRight && horizontalMove > 0))
        {
            ChangeDirection();
        }

        rb.linearVelocityX = horizontalMove * movementSpeed;
        if (rb.linearVelocityY < -maxFallSpeed)
            rb.linearVelocityY = -maxFallSpeed;
        
    }

    private void SetUpJumpForce()
    {
        jumpForce = 4 * jumpHeight / jumpTime;
        rb.gravityScale = 8 * jumpHeight / Mathf.Pow(jumpTime, 2) / (-Physics2D.gravity.y); 
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void ChangeDirection()
    {
        Flip();
        IsFacingRight = !IsFacingRight;
    }

    private void SetHorizontalMove(float value)
    {
        horizontalMove = value;
    }

    private void Jump()
    {
        if (IsTouchingGround)
            rb.linearVelocityY = jumpForce;
    }
}
