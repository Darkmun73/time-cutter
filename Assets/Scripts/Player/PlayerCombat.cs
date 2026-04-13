using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(DirectionsController))]
[RequireComponent(typeof(Collider2D))]
public class PlayerCombat : MonoBehaviour
{
    private Collider2D playerCollider;
    private DirectionsController directions;
    [SerializeField] private AttackData data;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject hitPrefab;
    private Vector2 attackVectorStartCoords = Vector2.zero;
    private Vector2 attackVectorEndCoords = Vector2.zero;

    void Awake()
    {
        playerCollider = GetComponent<Collider2D>();
        directions = GetComponent<DirectionsController>();
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
        if (Pointer.current != null)
            attackVectorStartCoords = Pointer.current.position.ReadValue();
        else
            Debug.LogError("You need to use pointer device! (eg. mouse)");
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
        ApplyHit(hitObject);
    }

    private void ApplyHit(GameObject hitObject)
    {
        List<Collider2D> colliders = new();
        var hitCollider = hitObject.GetComponent<Collider2D>();
        hitCollider.Overlap(colliders);
        foreach (var collider in colliders) if (playerCollider != collider) // TODO: При задевании колайдера-тригера тоже будет проходить удар?
        {   
            if (collider.TryGetComponent<IHittable>(out var hittable))
                hittable.ReceiveHit(transform, data.BaseDamage);
        }
    }
}
