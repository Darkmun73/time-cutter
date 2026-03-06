using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerCombat : MonoBehaviour
{
    private Player player;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject hitPrefab;
    private Vector2 attackVectorStartCoords = Vector2.zero;
    private Vector2 attackVectorEndCoords = Vector2.zero;

    void Start()
    {
        player = GetComponent<Player>();

        inputReader.AttackInitializing += StartAttackInit;
        inputReader.AttackInitialized += EndAttackInit;
        inputReader.AttackInitialized += Attack;
    }

    private void StartAttackInit()
    {
        attackVectorStartCoords = Pointer.current.position.ReadValue();
    }

    private void EndAttackInit()
    {
        attackVectorEndCoords = Pointer.current.position.ReadValue();
    }

    // Make attack with initialized attack vector's start and end coordinates
    private void Attack()
    {
        Attack(attackVectorStartCoords, attackVectorEndCoords);
    }

    // Make attack with vector from start to end
    private void Attack(Vector2 start, Vector2 end)
    {
        Vector2 playerLookDirectionVector = player.LookDirection switch
        {
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            _ => Vector2.zero
        };

        Vector2 hitDirection = end - start;
        float zRotation = Vector2.SignedAngle(playerLookDirectionVector, hitDirection);
        Quaternion rotation = Quaternion.Euler(0, 0, zRotation);

        Vector3 hitPosition = new(transform.position.x + playerLookDirectionVector.x * 2, transform.position.y, transform.position.z);
        GameObject hitObject = Instantiate(hitPrefab, hitPosition, rotation);
        Destroy(hitObject, 1);
    }
}
