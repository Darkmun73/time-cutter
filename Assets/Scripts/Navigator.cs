using System.Collections;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(DirectionsController))]
public class Navigator : MonoBehaviour
{
    private Movement movement;
    private DirectionsController directions;
    private Seeker seeker;
    
    private Transform target;
    private Path path;
    private int currentWaypoint;
    
    private Coroutine updatePathCoroutine;

    [SerializeField] private PathfindingData data;

    void Awake()
    {
        movement = GetComponent<Movement>();
        directions = GetComponent<DirectionsController>();
        seeker = GetComponent<Seeker>();
    }

    public void StartUpdatingPath()
    {
        updatePathCoroutine = StartCoroutine(RepeatUpdatePath(data.PathUpdateTime));
    }
    
    public void StopUpdatingPath()
    {
        if (updatePathCoroutine != null)
        {
            StopCoroutine(updatePathCoroutine);
            updatePathCoroutine = null;
        }
    }

    private IEnumerator RepeatUpdatePath(float interval)
    {
        while (true)
        {
            UpdatePath();
            yield return new WaitForSeconds(interval);
        }
    } 

    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
    }

    public void FollowPath()
    {
        if (path == null)
            return;

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            //Debug.Log("reached end");
            return;
        }

        // Direction Calculation
        Vector2 direction = ((Vector2)(path.vectorPath[currentWaypoint] - transform.position)).normalized;

        // Jump
        if (movement.IsTouchingGround &&
            direction.y > data.JumpNodeHeightRequirement)
            movement.Jump();

        // Movement
        movement.MoveHorizontal(directions.MovementDirection);

        // Next Waypoint
        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < data.NextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private void OnPathComplete(Path path)
    {
        if (!path.error)
        {
            this.path = path;
            currentWaypoint = 0;
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}