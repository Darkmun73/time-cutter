using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(DirectionsController))]
[RequireComponent(typeof(KnockbackController))]
[RequireComponent(typeof(EnemyCombat))]
[RequireComponent(typeof(Navigator))]
public class EnemyController : StateMachine
{
    public readonly struct TargetInfo // TODO: GameObject в конструктор
    {
        public Transform Transform {get;}
        public Vector2 Size {get;}

        public TargetInfo(Transform transform, Vector2 size)
        {
            Transform = transform;
            Size = size;
        }
    }

    public Movement Movement {get; private set;}
    public DirectionsController Directions {get; private set;}
    public Navigator Navigator {get; private set;}
    public EnemyCombat Combat {get; private set;}

    private TargetInfo target;
    public TargetInfo Target {
        get => target;
        private set
        {
            target = value;
            Navigator.SetTarget(target.Transform);
        }
    }

    [SerializeField] private float detectionRadius = 5f;
    private float attackRadius;

    private KnockbackController knockbackController;

    private EnemyIdleState idleState;
    private EnemyChasingState chasingState;
    private EnemyAttackState attackState;

    void Awake()
    {
        Movement = GetComponent<Movement>();
        Directions = GetComponent<DirectionsController>();
        Navigator = GetComponent<Navigator>();
        Combat = GetComponent<EnemyCombat>();
        var player = FindFirstObjectByType<Player>();
        var playerCollider = player.GetComponent<Collider2D>();
        Target = new TargetInfo(player.transform, playerCollider.bounds.size);
        knockbackController = GetComponent<KnockbackController>();

        attackRadius = Combat.GetHitRadius() + Combat.DistanceToAttackPoint();
    }

    void Start()
    {
        idleState = new EnemyIdleState(this);
        chasingState = new EnemyChasingState(this);
        attackState = new EnemyAttackState(this);
        
        CurrentState = idleState;
    }

    protected override void Update()
    {
        HandleStateTransition();
        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (!knockbackController.IsKnockedBack)
            base.FixedUpdate();
    }

    private void HandleStateTransition()
    {
        //Vector2.Distance(Physics2D.ClosestPoint(), Target.position); // TODO: для attack state использовать не distance to target
        var distanceToTargetCenter = Vector2.Distance(Target.Transform.position, transform.position);
        var distanceToNearestTargetEdge = distanceToTargetCenter - Target.Size.x / 2;
        
        if (CurrentState != attackState && distanceToNearestTargetEdge <= attackRadius)
            CurrentState = attackState;
        else if (CurrentState != chasingState && distanceToNearestTargetEdge > attackRadius && distanceToTargetCenter <= detectionRadius)
            CurrentState = chasingState;
        else if (CurrentState != idleState && distanceToTargetCenter > detectionRadius)
            CurrentState = idleState;
    }

    void OnDrawGizmosSelected()
    {
        var previousColor = Gizmos.color;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = previousColor;
    }
}
