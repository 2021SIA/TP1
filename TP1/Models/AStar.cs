using System;
using System.Collections.Generic;
using System.Text;

namespace TP1.Models
{
    public class AStar : SearchTree
    {
        public HeuristicFunction Heuristic { get; }

        public AStar(State root, HeuristicFunction heuristic) : base(root)
        {
            Heuristic = heuristic;
        }

        public override Node GetSolution()
        {
            var explored = new HashSet<State>();
            var found = new HashSet<State>();
            var heap = new SortedSet<NodePair>(new NodeComparer(Heuristic));

            heap.Add(new (Root, 0));
            while(heap.Count > 0)
            {
                NodePair current = heap.Min;
                heap.Remove(current);
                explored.Add(current.Node.State);
                if (current.Node.State.IsGoal)
                    return current.Node;

                foreach(var action in current.Node.State.PosibleActions())
                {
                    Node child = new(current.Node, action.Value, action.Key);
                    if (!explored.Contains(child.State) && !found.Contains(child.State))
                    {
                        found.Add(child.State);
                        heap.Add(new(child, current.Depth + 1));
                    }
                }
            }
            return null;
        }

        private record NodePair
        {
            private static int count = 0;

            public NodePair(Node node, int depth)
            {
                Node = node;
                Depth = depth;
                Order = count++;
            }

            public int Order { get; }
            public Node Node { get; init; }
            public int Depth { get; init; }

            public override int GetHashCode() => Node.GetHashCode();
        }

        private class NodeComparer : Comparer<NodePair>
        {
            public NodeComparer(HeuristicFunction heuristic)
            {
                Heuristic = heuristic;
            }

            public HeuristicFunction Heuristic { get; }
            public override int Compare(NodePair x, NodePair y)
            {
                double hx = Heuristic(x.Node.State), hy = Heuristic(y.Node.State);
                double fx = x.Depth + hx, fy = y.Depth + hy;
                if (fx.CompareTo(fy) != 0)
                    return fx.CompareTo(fy);
                else if (hx.CompareTo(hy) != 0)
                    return hx.CompareTo(hy);
                else
                    return x.Order.CompareTo(y.Order);
            }
        }
    }
}
