using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(DirectionsController))]
public class PlayerCombat : MonoBehaviour
{
    private Player player;
    private DirectionsController directions;
    [SerializeField] private AttackData data;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject hitPrefab;
    private Vector2 attackVectorStartCoords = Vector2.zero;
    private Vector2 attackVectorEndCoords = Vector2.zero;

    private ContactFilter2D enemiesFilter;

    void Awake()
    {
        player = GetComponent<Player>();
        directions = GetComponent<DirectionsController>();
        
        enemiesFilter = new();
        enemiesFilter.SetLayerMask(LayerMask.GetMask("Enemies"));
    }

    void OnEnable()
    {
        inputReader.AttackInitializing += StartAttackInit;
        inputReader.AttackInitialized += EndAttackInit;
        inputReader.AttackInitialized += Attack;
    }

    void OnDisable()
    {
        inputReader.AttackInitializing -= StartAttackInit;
        inputReader.AttackInitialized -= EndAttackInit;
        inputReader.AttackInitialized -= Attack;
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
        Vector2 playerLookDirectionVector = directions.LookDirection.ToVector();

        Vector2 hitDirection = end - start;
        float zRotation = Vector2.SignedAngle(Vector2.right, hitDirection);
        Quaternion rotation = Quaternion.Euler(0, 0, zRotation);

        Vector3 hitPosition = new(transform.position.x + playerLookDirectionVector.x * 1.5f, transform.position.y + playerLookDirectionVector.y * 1.5f, transform.position.z);
        GameObject hitObject = Instantiate(hitPrefab, hitPosition, rotation, transform);
        StartCoroutine(PhysicsHitOccured(hitObject));
        //player.events.OnHitOccured(hitObject);
        Destroy(hitObject, 0.2f);
    }

    // MAYBE TODO: если будет задержка что-нибудь придумать без енумератора
    private IEnumerator PhysicsHitOccured(GameObject hitObject)
    {
        yield return new WaitForFixedUpdate();
        ApplyHitToEnemies(hitObject);
    }

    private void ApplyHitToEnemies(GameObject hitObject)
    {
        List<Collider2D> enemiesColliders = new();
        var hitCollider = hitObject.GetComponent<Collider2D>();
        hitCollider.Overlap(enemiesFilter, enemiesColliders);
        foreach (var enemyCollider in enemiesColliders) // TODO: При задевании колайдера-тригера врага тоже будет проходить удар?
        {   
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            enemy.ReceiveHit(transform, data.BaseDamage);
        }
    }
}
