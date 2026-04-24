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

    private class AttackInfo
    {
        public List<float> HitAngles {get; private set;} // TODO: сделать приватным, и добавить сюда методы, через которые и изменять HitAngles
        public float DamageCoef {get; private set;}

        private readonly PlayerAttackData data;
        
        public AttackInfo(float firstHitAngle, PlayerAttackData data)
        {
            this.data = data;

            HitAngles = new() { firstHitAngle };
            DamageCoef = data.MinDamageCoef;
        }

        // (minCoef, maxCoef): clamps damage accumulation coef to this interval
        // thresholdAngle: if difference of last angles < then this value then coef decreases, else increases
        public void RegisterHit(float angle)
        {
            // Always have at least two hits before recalculating (from constructor + first call of this RegisterHit method)
            HitAngles.Add(angle);
            if (HitAngles.Count > data.MaxHitAngles)
                HitAngles.RemoveAt(0);
            RecalculateDamageCoef();
        }

        private void RecalculateDamageCoef()
        {
            float lastAngle = HitAngles[^1];
            float preLastAngle = HitAngles[^2];
            float angleDiff = Mathf.Abs(Mathf.DeltaAngle(preLastAngle, lastAngle)); // TODO: поменять, если не то что надо
            float acuteAngleDiff = angleDiff < Angles.RightAngle ? angleDiff : Angles.StraightAngle - angleDiff; 

            
            if (acuteAngleDiff > data.DamageThresholdAngle)
            {
                var normalizedDiff = (acuteAngleDiff - data.DamageThresholdAngle) / (Angles.StraightAngle - data.DamageThresholdAngle);
                float baseGrowth = Mathf.Lerp(0f, data.DamageReward, normalizedDiff);

                float repetitionMultiplier = CalculateRepetitionMultiplier();
                float growth = baseGrowth * repetitionMultiplier;

                DamageCoef += growth;
                
                Debug.Log($"✓ Last angle: {lastAngle}° Diff:{acuteAngleDiff:F1}° Base:{baseGrowth:F3} Repetition:{repetitionMultiplier:F3} Final:{growth:F3} → {DamageCoef:F3}");
            }
            else
            {
                var normalizedAngleDiff = 1f - (acuteAngleDiff / data.DamageThresholdAngle);
                float penalty = Mathf.Lerp(0f, data.DamagePenalty, normalizedAngleDiff);
                DamageCoef -= penalty;
                
                Debug.Log($"✗ Last angle: {lastAngle}° Diff:{acuteAngleDiff:F1}° Penalty:{penalty:F3} → {DamageCoef:F3}");
            }
            
            DamageCoef = Mathf.Clamp(DamageCoef,
                                     data.MinDamageCoef,
                                     data.MaxDamageCoef);
        }

        private float CalculateRepetitionMultiplier()
        {
            float lastAngle = HitAngles[^1];
            float totalPenalty = 0f;
            
            var anglesCountWithoutLastTwo = HitAngles.Count - 2;
            for (int i = 0; i < anglesCountWithoutLastTwo; i++)
            {
                float historicAngle = HitAngles[i];
                float diff = Mathf.Abs(Mathf.DeltaAngle(lastAngle, historicAngle));
                
                if (diff < data.DamageThresholdAngle)
                {
                    float similarity = 1f - (diff / data.DamageThresholdAngle);
                    
                    int positionFromEnd = anglesCountWithoutLastTwo - i;
                    float decayFactor = Mathf.Exp(-positionFromEnd * 0.3f); // TODO: поменять волшебное число
                    
                    totalPenalty += similarity * decayFactor;
                }
            }
            
            float multiplier = 1f - Mathf.Clamp01(totalPenalty) * Mathf.Clamp01(data.RepetitionPenalty);
            return multiplier;
        }
    }

    [SerializeField] private PlayerAttackData data;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject hitPrefab;

    private Collider2D playerCollider;
    private DirectionsController directions;

    private Vector2 attackVectorStartCoords = Vector2.zero;
    private Vector2 attackVectorEndCoords = Vector2.zero;

    private Dictionary<GameObject, AttackInfo> objectsAttackInfo = new(); // There would be hit angles for every hit object

    void Awake()
    {
        Debug.Assert(data.MaxHitAngles > 1, "Max hit angles value is too small! Must be at least 2.");
        Debug.Assert(data.DamageThresholdAngle is > 0f and < 180f, "Damage threshold angle must be > 0 and < 180!");

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
                var objAttackInfo = AddHitAngleForObject(collider.gameObject, hit.Angle);
                hittable.ReceiveHit(transform, data.BaseDamage * objAttackInfo.DamageCoef);
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
        if (currentAngles.Count < data.ShieldBreakAngles.Count)
            return false;
        var lastAngles = currentAngles.GetRange(currentAngles.Count - data.ShieldBreakAngles.Count, data.ShieldBreakAngles.Count);
        for (int i = 0; i < lastAngles.Count; ++i)
        {
            if (Mathf.Abs(lastAngles[i] - data.ShieldBreakAngles[i]) > data.AngleTolerance)
                return false;
        }
        return true;
    }

    private AttackInfo AddHitAngleForObject(GameObject obj, float angle)
    {
        if (objectsAttackInfo.TryGetValue(obj, out var attackInfo))
            attackInfo.RegisterHit(angle);
        else
            attackInfo = new(angle, data);
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
            yield return new WaitForSeconds(data.ObjectsAttackInfoCleanupInterval);
        }
    }

    public void ResetObjectsHitAngles() // TODO: использовать при загрузке другой сцены?
    {
        objectsAttackInfo = new();
    }
}

