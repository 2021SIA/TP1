using System;
using System.Collections.Generic;
using System.Text;

namespace TP1.Models
{
    public class BFS : SearchTree
    {
        public BFS(State root) : base(root) { }
        public override Node GetSolution()
        {
            var queue = new Queue<(Node node, int depth)>();
            var explored = new HashSet<State>();
            int currentDepth = 0;

            queue.Enqueue((Root, 0));
            while(queue.Count > 0)
            {
                (Node current, int depth) = queue.Dequeue();
                explored.Add(current.State);
                if(depth > currentDepth)
                {
                    currentDepth = depth;
                    Console.WriteLine($"Current depth: {currentDepth}");
                }
                if (current.State.IsGoal)
                    return current;

                foreach(var action in current.State.PosibleActions())
                {
                    Node child = new Node(current, action.Value, action.Key);
                    if (!explored.Contains(child.State))
                        queue.Enqueue((child, depth+1));
                }
            }
            return null;
        }
    }
}
