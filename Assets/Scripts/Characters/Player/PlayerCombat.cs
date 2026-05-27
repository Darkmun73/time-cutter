using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(DirectionsController))]
[RequireComponent(typeof(Collider2D))]
public class PlayerCombat : MonoBehaviour
{
    public readonly struct AttackInfo
    {
        public float Angle {get;}

        public AttackInfo(float angle)
        {
            Angle = angle;
        }
    }

    [SerializeField] private PlayerAttackData attackData;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject attackObjectPrefab;

    private Collider2D playerCollider;
    private DirectionsController directions;

    private Vector2 attackVectorStartCoords = Vector2.zero;
    private Vector2 attackVectorEndCoords = Vector2.zero;
    private bool canAttack = true;
    private float damageMultiplier = 1f;

    public bool IsAttacking {get; private set;} = false;

    private Dictionary<GameObject, AttackSequenceInfo> objectsAttackSequence = new(); // There would be hit angles for every hit object
    private HashSet<AnglesCombinationEffect> anglesCombinationEffects = new();

    public event UnityAction<AttackInfo> AttackStarted;

    void Awake()
    {
        Debug.Assert(attackData.MaxHitAngles > 1, "Max hit angles value is too small! Must be at least 2.");
        Debug.Assert(attackData.DamageThresholdAngle is > 0f and < 180f, "Damage threshold angle must be > 0 and < 180!");

        playerCollider = GetComponent<Collider2D>();
        directions = GetComponent<DirectionsController>();

        StartCoroutine(CleanupObjectsAttackSequence());
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

        Vector2 attackDirection = end - start;
        float zRotation = Vector2.SignedAngle(Vector2.right, attackDirection);
        Quaternion rotation = Quaternion.Euler(0, 0, zRotation);

        Vector3 attackObjectPosition =
        new(transform.position.x + playerLookDirectionVector.x * 0.5f,
            transform.position.y + playerLookDirectionVector.y * 0.5f,
            transform.position.z); // TODO: разобраться с магическими числами
        GameObject attackObject = Instantiate(attackObjectPrefab, attackObjectPosition, rotation, transform);

        StartCoroutine(PhysicsAttackObjectOccured(attackObject, zRotation));
        StartCoroutine(DestroyAttackObject(attackObject, attackData.AttackDuration));
        StartCooldown();

        IsAttacking = true;
        AttackInfo attackInfo = new(zRotation);
        AttackStarted?.Invoke(attackInfo);
    }

    // MAYBE TODO: если будет задержка что-нибудь придумать без енумератора
    private IEnumerator PhysicsAttackObjectOccured(GameObject attackObject, float angle)
    {
        yield return new WaitForFixedUpdate();
        ApplyHit(attackObject, angle);
    }

    private IEnumerator DestroyAttackObject(GameObject attackObject, float interval)
    {
        yield return new WaitForSeconds(interval);
        Destroy(attackObject);
        IsAttacking = false;
    }

    public void StartCooldown()
    {
        ProhibitAttack(attackData.Cooldown);
    }

    public void AllowAttack()
    {
        canAttack = true;
    }

    public void ProhibitAttack()
    {
        canAttack = false;
    }

    public void ProhibitAttack(float seconds)
    {
        StartCoroutine(ProhibitAttackRoutine(seconds));
    }

    private IEnumerator ProhibitAttackRoutine(float seconds)
    {
        ProhibitAttack();
        yield return new WaitForSeconds(seconds);
        AllowAttack();
    }

    private void ApplyHit(GameObject attackObject, float angle)
    {
        List<Collider2D> colliders = new();
        var attackCollider = attackObject.GetComponent<Collider2D>();
        attackCollider.Overlap(colliders);
        foreach (var collider in colliders) if (playerCollider != collider) // TODO: При задевании колайдера-тригера тоже будет проходить удар?
        {   
            if (collider.TryGetComponent<HitReceiver>(out var hitReceiver))
            {
                //Debug.Log(angle);
                var objAttackSequence = AddAttackAngleForObject(collider.gameObject, angle);
                ApplyCombinationEffects(objAttackSequence);
                float hitDamage = attackData.BaseDamage * objAttackSequence.DamageCoef * damageMultiplier;
                PlayerHitInfo hitInfo = new(transform, hitDamage, attackObject, angle);
                hitReceiver.ReceiveHit(hitInfo);
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
        var currentAngles = objectsAttackSequence[objectWithShield].AttackAngles;
        return Angles.EndsWithPatternWithinTolerance(currentAngles,
                                                     attackData.ShieldBreakAngles,
                                                     attackData.AngleTolerance);
    }

    private AttackSequenceInfo AddAttackAngleForObject(GameObject obj, float angle)
    {
        if (objectsAttackSequence.TryGetValue(obj, out var attackInfo))
            attackInfo.RegisterHit(angle);
        else
            attackInfo = new(angle, attackData);
            objectsAttackSequence[obj] = attackInfo;

        return attackInfo;
    }

    private IEnumerator CleanupObjectsAttackSequence()
    {
        while (true)
        {
            var destroyedObjects = objectsAttackSequence.Keys
            .Where(obj => obj == null)
            .ToList();

            foreach (var destroyedObject in destroyedObjects)
            {
                objectsAttackSequence.Remove(destroyedObject);
            }
            yield return new WaitForSeconds(attackData.ObjectsAttackSequenceCleanupInterval);
        }
    }

    public void ResetObjectsAttackSequence() // TODO: использовать при загрузке другой сцены?
    {
        objectsAttackSequence = new();
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

    private void ApplyCombinationEffects(AttackSequenceInfo attackInfo)
    {
        foreach (var combinationEffect in anglesCombinationEffects)
        {
            combinationEffect.TryApply(attackInfo.AttackAngles, attackData.AngleTolerance);
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

    public float GetAttackDuration()
    {
        return attackData.AttackDuration;
    }
}

