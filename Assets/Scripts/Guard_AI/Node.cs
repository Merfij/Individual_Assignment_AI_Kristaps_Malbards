using System.Collections.Generic;
using UnityEngine;
namespace Pathfinding.BehaviorTree
{
    public class BehaviourTree : Node
    {
        public BehaviourTree(string name) : base(name) { }

        public override NodeStatus Process()
        {
            while(currentChild < children.Count)
            {
                var status = children[currentChild].Process();
                if (status != NodeStatus.Success)
                {
                    return status;
                }
                currentChild++;
            }
            return NodeStatus.Success;
        }
    }

    public class Leaf : Node
    {
        readonly IStrategy strategy;

        public Leaf(string name, IStrategy strategy) : base(name)
        {
             this.strategy = strategy;
        }

        public override NodeStatus Process() => strategy.Process();

        public override void Reset() => strategy.Reset();
    }
    public class Node
    {
        public enum NodeStatus
        {
            Success,
            Failure,
            Running,
        }

        public readonly string name;

        public readonly List<Node> children = new();
        protected int currentChild;

        public Node(string name = "Node")
        {
            this.name = name;
        }

        public void AddChild(Node child) => children.Add(child);

        public virtual NodeStatus Process() => children[currentChild].Process();

        public virtual void Reset()
        {
            currentChild = 0;

            foreach (Node child in children)
            {
                child.Reset();
            }
        }
    }
}
