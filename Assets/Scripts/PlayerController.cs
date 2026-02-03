using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // components
    private Rigidbody2D rb;
    [SerializeField] private InputReader inputReader;

    // other
    [SerializeField] private float speed = 10f;
    private float horizontalMove;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        inputReader.moveEvent += SetHorizontalMove;
    }

    void FixedUpdate()
    {
        rb.linearVelocityX = horizontalMove * speed;
    }

    private void SetHorizontalMove(float value)
    {
        horizontalMove = value;
    }
}
