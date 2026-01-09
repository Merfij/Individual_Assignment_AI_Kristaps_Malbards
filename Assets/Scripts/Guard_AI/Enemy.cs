using System.Collections.Generic;
using Pathfinding.BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] List<Transform> waypoints = new();

    private NavMeshAgent agent;
    private Rigidbody rb;
    BehaviourTree tree;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        tree = new BehaviourTree("Enemy");
        tree.AddChild(new Leaf("Patrol", new PatrolStrategy(transform, agent, waypoints)));
    }

    public void Update()
    {
        rb.freezeRotation = true;
        tree.Process();
    }
}
