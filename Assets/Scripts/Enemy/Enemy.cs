using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    [field: SerializeField] public EnemyData Data {get; private set;}
    [SerializeField] private PlayerEvents playerEvents;

    private Rigidbody2D rigidBody;
    private Collider2D coll;
    private KnockbackController knockbackController;
    

    private StateMachine stateMachine;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        knockbackController = GetComponent<KnockbackController>();

        var idleState = new EnemyIdleState(this, rigidBody);

        stateMachine = new(idleState);
    }

    void OnEnable()
    {
        playerEvents.HitOccured += HandlePlayerHit;
    }

    void OnDisable()
    {
        playerEvents.HitOccured -= HandlePlayerHit;
    }

    void Update()
    {
        stateMachine.Update();
    }

    void FixedUpdate()
    {
        if (!(knockbackController != null && knockbackController.IsKnockedBack))
            stateMachine.FixedUpdate();
    }

    private void HandlePlayerHit(GameObject hit)
    {
        // MAYBE TODO: Если будет работать не точно, то мб поменять на IsTouching или подобное
        bool hitTouching = coll.Distance(hit.GetComponent<Collider2D>()).isOverlapped;
        //bool hitTouching = coll.IsTouching(hit.GetComponent<Collider2D>());
        //Debug.Log($"hit touching: {hitTouching}");
        if (hitTouching)
        {
            if (knockbackController != null)
                knockbackController.Knockback(hit.transform.parent.position);
        }
    }
}
