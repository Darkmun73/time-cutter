using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // components
    private Rigidbody2D rb;

    // other
    [SerializeField] private InputReader inputReader;
    [SerializeField] private ContactFilter2D ground;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float jumpForce = 18;

    public bool IsTouchingGround => rb.IsTouching(ground);
    private bool isFacingRight = true;

    [SerializeField] private float maxFallSpeed = 10f;
    private float horizontalMove;

    // attack
    public GameObject hitPrefab;
    private bool isAttackEnded = true;
    private Vector2 attackStartCoords = Vector2.zero;
    private Vector2 attackEndCoords = Vector2.zero;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        inputReader.moveEvent += SetHorizontalMove;
        inputReader.jumpEvent += Jump;
        inputReader.attackEvent += HandleAttack;
    }

    void FixedUpdate()
    {
        if ((isFacingRight && horizontalMove < 0) || 
            (!isFacingRight && horizontalMove > 0))
        {
            ChangeDirection();
        }

        rb.linearVelocityX = horizontalMove * movementSpeed;
        if (rb.linearVelocityY < -maxFallSpeed)
            rb.linearVelocityY = -maxFallSpeed;

        
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
        isFacingRight = !isFacingRight;
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

    private void HandleAttack()
    {
        if (isAttackEnded)
        {
            attackStartCoords = Pointer.current.position.ReadValue();
        } else
        {
            attackEndCoords = Pointer.current.position.ReadValue();
            Attack(attackStartCoords, attackEndCoords);
        }
        isAttackEnded = !isAttackEnded;
    }

    private void Attack(Vector2 start, Vector2 end)
    {
        Vector2 playerDirection = isFacingRight ? Vector2.right : Vector2.left;

        Vector2 hitDirection = end - start;
        float zRotation = Vector2.SignedAngle(playerDirection, hitDirection);
        Quaternion rotation = Quaternion.Euler(0, 0, zRotation);

        Vector3 hitPosition = new(transform.position.x + playerDirection.x * 2, transform.position.y, transform.position.z);
        GameObject hitObject = Instantiate(hitPrefab, hitPosition, rotation);
        Destroy(hitObject, 1);
    }
}
