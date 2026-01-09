using System.Collections.Generic;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    patrolling,
    chasing,
    investigating,
    returnToPatrolling,
}
public class EnemyFSM : MonoBehaviour
{
    [Header("FOV Settings")]
    public float radius;
    [UnityEngine.Range(0, 360)]
    public float angle;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;
    public GameObject playerRef;

    [Header("Enemy Target")]
    public Transform target;

    [Header("Chase Range")]
    public float chaseRange;
    public float loseChase;
    public float chaseSpeed;

    public float waypointTolerance = 0.5f;

    int currentIndex;
    Checkpoints waypoints;
    //public List<Transform> checkpoints;
    //public List<Transform> visitedTargets;
    NavMeshAgent agent;

    public EnemyState currentState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        waypoints = GameObject.FindGameObjectWithTag("Checkpoints_Holder").GetComponent<Checkpoints>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.patrolling:
                UpdatePatrol();
                break;
            case EnemyState.chasing:
                UpdateChase();
                break;
            case EnemyState.returnToPatrolling:
                UpdateReturnToPatrol();
                break;
        }

        FieldOfViewCheck();
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    private void UpdateReturnToPatrol()
    {
        agent.SetDestination(waypoints.checkpoints[currentIndex].position);

        if (Vector3.Distance(transform.position, target.position) < chaseRange || canSeePlayer)
        {
            currentState = EnemyState.chasing;
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < waypointTolerance)
        {
            currentState = EnemyState.patrolling;
            return;
        }
    }

    private void UpdateChase()
    {
        if (canSeePlayer)
        {
            agent.SetDestination(target.position);
        }

        if (!canSeePlayer)
        {
            if (Vector3.Distance(transform.position, target.position) > loseChase)
            {
                currentState = EnemyState.returnToPatrolling;
                agent.SetDestination(waypoints.checkpoints[currentIndex].position);
            }
        }
    }

    private void UpdatePatrol()
    {

        if (waypoints.checkpoints.Count == 0)
        {
            NewIndex();
        }

        if (!agent.pathPending && agent.remainingDistance < waypointTolerance)
        {
            waypoints.visitedTargets.Add(waypoints.checkpoints[currentIndex]);
            waypoints.checkpoints.RemoveAt(currentIndex);
            //random new index
            int newIndex = NewIndex();
            currentIndex = newIndex;
            agent.SetDestination(waypoints.checkpoints[currentIndex].position);
        }

        if (Vector3.Distance(transform.position, target.position) < chaseRange || canSeePlayer == true)
        {
            currentState = EnemyState.chasing;
            return;
        }
    }

    private int NewIndex()
    {
        if (waypoints.checkpoints.Count > 0)
        {
            int newIndex = UnityEngine.Random.Range(0, waypoints.checkpoints.Count);
            return newIndex;
        }
        else
        {
            while (waypoints.visitedTargets.Count > 0)
            {
                for (int i = 0; i < waypoints.visitedTargets.Count; i++)
                {
                    waypoints.checkpoints.Add(waypoints.visitedTargets[i]);
                    waypoints.visitedTargets.RemoveAt(i);
                }
            }
            NewIndex();
        }
        return NewIndex();
    }
}
