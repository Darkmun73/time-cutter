using System;
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
    private readonly struct HitInfo
    {
        public GameObject Obj {get;}
        public float Angle {get;}
        
        public HitInfo(GameObject obj, float angle)
        {
            Obj = obj;
            Angle = angle;
        }
    }

    [SerializeField] private PlayerAttackData attackData;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject hitPrefab;

    private Collider2D playerCollider;
    private DirectionsController directions;

    private Vector2 attackVectorStartCoords = Vector2.zero;
    private Vector2 attackVectorEndCoords = Vector2.zero;
    private bool canAttack = true;
    private float damageMultiplier = 1f;

    private Dictionary<GameObject, AttackInfo> objectsAttackInfo = new(); // There would be hit angles for every hit object
    private HashSet<AnglesCombinationEffect> anglesCombinationEffects = new();

    void Awake()
    {
        Debug.Assert(attackData.MaxHitAngles > 1, "Max hit angles value is too small! Must be at least 2.");
        Debug.Assert(attackData.DamageThresholdAngle is > 0f and < 180f, "Damage threshold angle must be > 0 and < 180!");

        playerCollider = GetComponent<Collider2D>();
        directions = GetComponent<DirectionsController>();

        StartCoroutine(CleanupObjectsHitAngles());
    }

    void OnEnable()
    {
        inputReader.AttackInitializing += StartAttackInit;
        inputReader.AttackInitialized += EndAttackInit;
        inputReader.AttackInitialized += TryAttack;
    }

    void OnDisable()
    {
        inputReader.AttackInitializing -= StartAttackInit;
        inputReader.AttackInitialized -= EndAttackInit;
        inputReader.AttackInitialized -= TryAttack;
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

    private void TryAttack()
    {
        if (canAttack)
            Attack();
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

        Vector3 hitPosition = new(transform.position.x + playerLookDirectionVector.x * 1.5f,
                                  transform.position.y + playerLookDirectionVector.y * 1.5f,
                                  transform.position.z);
        GameObject hitObject = Instantiate(hitPrefab, hitPosition, rotation, transform);
        StartCoroutine(PhysicsHitOccured(new(hitObject, zRotation)));
        Destroy(hitObject, 0.2f);
        StartCooldown();
    }

    // MAYBE TODO: если будет задержка что-нибудь придумать без енумератора
    private IEnumerator PhysicsHitOccured(HitInfo hit)
    {
        yield return new WaitForFixedUpdate();
        ApplyHit(hit);
    }

    public void StartCooldown()
    {
        StartCoroutine(ProhibitAttack(attackData.Cooldown));
    }

    private IEnumerator ProhibitAttack(float seconds)
    {
        canAttack = false;
        yield return new WaitForSeconds(seconds);
        canAttack = true;
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
                var objAttackInfo = AddHitAngleForObject(collider.gameObject, hit.Angle);
                ApplyCombinationEffects(objAttackInfo);
                hittable.ReceiveHit(transform, attackData.BaseDamage * objAttackInfo.DamageCoef * damageMultiplier);
                // Debug.Log(CanBreakShield(collider.gameObject));
                // Debug.Log(string.Join("; ", objectsHitAngles[collider.gameObject]));
                if (collider.TryGetComponent<Shield>(out var shield) &&
                    CanBreakShield(collider.gameObject))
                {
                    shield.Break();
                }
            }
        }
    }

    // obj: object with shield
    private bool CanBreakShield(GameObject objectWithShield) 
    {
        var currentAngles = objectsAttackInfo[objectWithShield].HitAngles;
        return Angles.EndsWithPatternWithinTolerance(currentAngles,
                                                     attackData.ShieldBreakAngles,
                                                     attackData.AngleTolerance);
    }

    private AttackInfo AddHitAngleForObject(GameObject obj, float angle)
    {
        if (objectsAttackInfo.TryGetValue(obj, out var attackInfo))
            attackInfo.RegisterHit(angle);
        else
            attackInfo = new(angle, attackData);
            objectsAttackInfo[obj] = attackInfo;

        return attackInfo;
    }

    private IEnumerator CleanupObjectsHitAngles()
    {
        while (true)
        {
            var destroyedObjects = objectsAttackInfo.Keys
            .Where(obj => obj == null)
            .ToList();

            foreach (var destroyedObject in destroyedObjects)
            {
                objectsAttackInfo.Remove(destroyedObject);
            }
            yield return new WaitForSeconds(attackData.ObjectsAttackInfoCleanupInterval);
        }
    }

    public void ResetObjectsHitAngles() // TODO: использовать при загрузке другой сцены?
    {
        objectsAttackInfo = new();
    }

    public void AddCombinationEffect(AnglesCombinationEffect combinationEffect)
    {
        Debug.Assert(combinationEffect != null, "PlayerCombat: Angles combination effect must be not null!");

        anglesCombinationEffects.Add(combinationEffect);
    }

    public void RemoveCombinationEffect(AnglesCombinationEffect combinationEffect)
    {
        Debug.Assert(combinationEffect != null, "PlayerCombat: Angles combination effect must be not null!");
        
        anglesCombinationEffects.Remove(combinationEffect);
    }

    private void ApplyCombinationEffects(AttackInfo attackInfo)
    {
        foreach (var combinationEffect in anglesCombinationEffects)
        {
            combinationEffect.TryApply(attackInfo.HitAngles, attackData.AngleTolerance);
        }
    }

    public void IncreaseDamageMultiplierByFactor(float factor)
    {
        if (factor < 1f)
        {
            Debug.LogWarning("Factor must be >= 1!");
            return;
        }
        damageMultiplier *= factor;
    }

    public void DecreaseDamageMultiplierByFactor(float factor)
    {
        if (factor < 1f)
        {
            Debug.LogWarning("Factor must be >= 1!");
            return;
        }
        damageMultiplier /= factor;
    }
}

