using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(DirectionsController))]
[RequireComponent(typeof(Collider2D))]
public class PlayerCombat : MonoBehaviour
{
    private struct HitInfo
    {
        public GameObject Obj {get;}
        public float Angle {get;}
        
        public HitInfo(GameObject obj, float angle)
        {
            this.Obj = obj;
            this.Angle = angle;
        }
    }

    [SerializeField] private PlayerAttackData data;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject hitPrefab;

    private Collider2D playerCollider;
    private DirectionsController directions;

    private Vector2 attackVectorStartCoords = Vector2.zero;
    private Vector2 attackVectorEndCoords = Vector2.zero;

    private Dictionary<GameObject, List<float>> objectsHitAngles = new(); // There would be hit angles for every hit object

    void Awake()
    {
        playerCollider = GetComponent<Collider2D>();
        directions = GetComponent<DirectionsController>();

        StartCoroutine(CleanupObjectsHitAngles());
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

    // void Update()
    // {
    //     foreach (var kvp in objectsHitAngles)
    //     {
    //         Debug.Log($"Key: {kvp.Key}, Value: {string.Join(", ",kvp.Value)}");
    //     }
    // }

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
        StartCoroutine(PhysicsHitOccured(new(hitObject, zRotation)));
        Destroy(hitObject, 0.2f);
    }

    // MAYBE TODO: если будет задержка что-нибудь придумать без енумератора
    private IEnumerator PhysicsHitOccured(HitInfo hit)
    {
        yield return new WaitForFixedUpdate();
        ApplyHit(hit);
    }

    private void ApplyHit(HitInfo hit)
    {
        List<Collider2D> colliders = new();
        var hitCollider = hit.Obj.GetComponent<Collider2D>();
        hitCollider.Overlap(colliders);
        foreach (var collider in colliders) if (playerCollider != collider) // TODO: При задевании колайдера-тригера тоже будет проходить удар?
        {   
            if (collider.TryGetComponent<IHittable>(out var hittable))
            {
                hittable.ReceiveHit(transform, data.BaseDamage);
                AddHitAngleForObject(collider.gameObject, hit.Angle);
            }
        }
    }

    private void AddHitAngleForObject(GameObject obj, float angle)
    {
        if (objectsHitAngles.ContainsKey(obj))
            objectsHitAngles[obj].Add(angle);
        else
            objectsHitAngles[obj] = new() { angle };
    }

    private IEnumerator CleanupObjectsHitAngles()
    {
        while (true)
        {
            var destroyedObjects = objectsHitAngles.Keys
            .Where(obj => obj == null)
            .ToList();

            foreach (var destroyedObject in destroyedObjects)
            {
                objectsHitAngles.Remove(destroyedObject);
            }
            yield return new WaitForSeconds(data.ObjectsHitAnglesCleanupInterval);
        }
    }

    public void ResetObjectsHitAngles() // TODO: использовать при загрузке другой сцены?
    {
        objectsHitAngles = new();
    }
}
