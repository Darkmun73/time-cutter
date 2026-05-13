using UnityEngine;
using UnityEngine.Events;

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

        public TargetInfo(GameObject target)
        {
            var targetCollider = target.GetComponent<Collider2D>();
            Transform = target.transform;
            if (targetCollider != null)
                Size = targetCollider.bounds.size;
            else
                Size = Vector2.zero;
        }
    }

    [SerializeField] private Transform attackPoint;

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

    public event UnityAction ChasingStarted;
    public event UnityAction ChasingStopped;

    void Awake()
    {
        Movement = GetComponent<Movement>();
        Directions = GetComponent<DirectionsController>();
        Navigator = GetComponent<Navigator>();
        Combat = GetComponent<EnemyCombat>();
        knockbackController = GetComponent<KnockbackController>();
        
        var player = FindFirstObjectByType<Player>();
        Target = new TargetInfo(player.gameObject);

        attackRadius = Combat.GetHitRadius() + Combat.DistanceToAttackPoint();

        idleState = new EnemyIdleState(this);
        chasingState = new EnemyChasingState(this);
        attackState = new EnemyAttackState(this);
        
        CurrentState = idleState;
    }

    void OnEnable()
    {
        Directions.MovementDirectionFlipped += FlipAttackPoint;
        chasingState.ChasingStarted += OnChasingStarted;
        chasingState.ChasingStopped += OnChasingStopped;
    }

    void OnDisable()
    {
        Directions.MovementDirectionFlipped -= FlipAttackPoint;
        chasingState.ChasingStarted -= OnChasingStarted;
        chasingState.ChasingStopped -= OnChasingStopped;
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

    private void OnChasingStarted()
    {
        ChasingStarted?.Invoke();
    }

    private void OnChasingStopped()
    {
        ChasingStopped?.Invoke();
    }

    private void HandleStateTransition()
    {
        //Vector2.Distance(Physics2D.ClosestPoint(), Target.position); // TODO: для attack state использовать не distance to target
        var distanceToTargetCenter = Vector2.Distance(Target.Transform.position, transform.position);
        var distanceToNearestTargetEdge = distanceToTargetCenter - Target.Size.x / 2;
        
        if (CurrentState != attackState && distanceToNearestTargetEdge <= attackRadius * 0.75f)
            CurrentState = attackState;
        else if (CurrentState != chasingState && distanceToNearestTargetEdge > attackRadius && distanceToTargetCenter <= detectionRadius)
            CurrentState = chasingState;
        else if (CurrentState != idleState && distanceToTargetCenter > detectionRadius)
            CurrentState = idleState;
    }

    private void FlipAttackPoint()
    {
        Debug.Log(attackPoint.localPosition);
        var currPosition = attackPoint.localPosition;
        currPosition.x = -currPosition.x;
        attackPoint.localPosition = currPosition;
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
