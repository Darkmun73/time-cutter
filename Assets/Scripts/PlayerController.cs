using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private PlayerData playerData;
    [SerializeField] private PlayerEvents playerEvents;

    public bool IsTouchingGround => rb.IsTouching(playerData.Ground);
    public bool IsFacingRight {get; private set;} = true;

    private float horizontalMove;
    private float jumpForce;

    private float health;
    public float Health
    {
        get { return health; }
        set { health = value; playerEvents.OnHealthChanged(value);}
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        playerData.InputReader.Moving += SetHorizontalMove;
        playerData.InputReader.Jumping += Jump;

        SetUpJumpForce();
        Health = playerData.MaxHealth;
    }

    void FixedUpdate()
    {
        if ((IsFacingRight && horizontalMove < 0) || 
            (!IsFacingRight && horizontalMove > 0))
        {
            ChangeDirection();
        }

        rb.linearVelocityX = horizontalMove * playerData.MovementSpeed;
        if (rb.linearVelocityY < -playerData.MaxFallSpeed)
            rb.linearVelocityY = -playerData.MaxFallSpeed;
        
    }

    private void SetUpJumpForce()
    {
        jumpForce = 4 * playerData.JumpHeight / playerData.JumpTime;
        rb.gravityScale = 8 * playerData.JumpHeight / Mathf.Pow(playerData.JumpTime, 2) / (-Physics2D.gravity.y); 
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
