using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Pathfinding.BehaviorTree
{
        public interface IStrategy
        {
            Node.NodeStatus Process();
            void Reset();
        }

    public class PatrolStrategy : IStrategy
    {
        readonly Transform enemy;
        readonly NavMeshAgent agent;
        readonly List<Transform> patrolPoints;
        readonly float patrolSpeed;
        int currentIndex;
        bool isPathCalculated;

        public PatrolStrategy(Transform enemy, NavMeshAgent agent, List<Transform> patrolPoints, float patrolSpeed = 2f)
        {
            this.enemy = enemy;
            this.agent = agent;
            this.patrolPoints = patrolPoints;
            this.patrolSpeed = patrolSpeed;
        }

        public Node.NodeStatus Process()
        {
            if (currentIndex == patrolPoints.Count)
            {
                return Node.NodeStatus.Success;
            }

            var target = patrolPoints[currentIndex];

            agent.SetDestination(target.position);
            enemy.LookAt(target.position);

            if (isPathCalculated && agent.remainingDistance < 0.1f)
            {
                currentIndex++;
                isPathCalculated = false;
            }

            if (agent.pathPending)
            {
                isPathCalculated = true;
            }

            return Node.NodeStatus.Running;
        }
        public void Reset() => currentIndex = 0;
    }

}