using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // components
    private Rigidbody2D rb;
    [SerializeField] private InputReader inputReader;

    // other
    [SerializeField] private ContactFilter2D ground;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float jumpForce = 18;

    public bool IsTouchingGround => rb.IsTouching(ground);

    // jumping
    //[SerializeField] private float maxJumpHeight = 5f;
    //[SerializeField] private float maxJumpTime = 2f;
    //private float JumpForce => maxJumpHeight * 2f / (maxJumpTime / 2f);

    [SerializeField] private float maxFallSpeed = 10f;
    private float horizontalMove;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.gravityScale = 0f;
        //Physics2D.gravity = new(Physics2D.gravity.x, (float) (maxJumpHeight * -2f / Math.Pow(maxJumpTime / 2f, 2)));
        //Debug.Log(rb.gravityScale);
        //Debug.Log(Physics2D.gravity);

        inputReader.moveEvent += SetHorizontalMove;
        inputReader.jumpEvent += Jump;
    }

    void FixedUpdate()
    {
        //rb.AddForce(new(horizontalMove * speed * Time.fixedDeltaTime, rb.position.y));
        //rb.MovePosition(new(rb.position.x + (horizontalMove * movementSpeed * Time.fixedDeltaTime), rb.position.y));
        rb.linearVelocityX = horizontalMove * movementSpeed;
        //rb.linearVelocityY += (float) (maxJumpHeight * -2f / Math.Pow(maxJumpTime / 2f, 2)) * Time.fixedDeltaTime;
        if (rb.linearVelocityY < -maxFallSpeed)
            rb.linearVelocityY = -maxFallSpeed;

        
    }

    private void SetHorizontalMove(float value)
    {
        horizontalMove = value;
    }

    private void Jump()
    {
        if (IsTouchingGround) // && rb.linearVelocityY == 0
            rb.linearVelocityY = jumpForce;
    }
}
