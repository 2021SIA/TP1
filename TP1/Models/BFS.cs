using System;
using System.Collections.Generic;
using System.Text;

namespace TP1.Models
{
    public class BFS : SearchTree
    {
        public BFS(State root) : base(root) { }
        public override Node GetSolution(out int expanded, out int frontier)
        {
            var queue = new Queue<(Node node, int depth)>();
            var explored = new HashSet<State>();
            int currentDepth = 0;
            expanded = 0;
            Node current = null;

            queue.Enqueue((Root, 0));
            while(queue.Count > 0)
            {
                int depth;
                (current, depth) = queue.Dequeue();
                explored.Add(current.State);
                if(depth > currentDepth)
                {
                    currentDepth = depth;
                    //Console.WriteLine($"Current depth: {currentDepth}");
                }
                if (current.State.IsGoal)
                    break;

                expanded++;
                foreach(var action in current.State.PosibleActions())
                {
                    Node child = new Node(current, action.Value, action.Key);
                    if (!explored.Contains(child.State))
                        queue.Enqueue((child, depth+1));
                }
            }
            frontier = queue.Count;
            return current;
        }
    }
}
