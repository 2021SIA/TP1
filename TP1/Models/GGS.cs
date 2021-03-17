using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TP1.Models
{
    public class GGS : SearchTree
    {
        HeuristicFunction heuristic;

        public GGS(State root, HeuristicFunction heuristic) : base(root)
        {
            this.heuristic = heuristic;
        }
        public class Leaf
        {
            public Node node { get; set; }
            public double h { get; set; }
            public int depth { get; set; }
        }

        public override Node GetSolution()
        {
            IDictionary<State, int> statesCache = new Dictionary<State, int>();
            List<Leaf> searchList = new List<Leaf>();
            IDictionary<object, State> posibleActions = null;
            Node solution = null;
            Leaf currentLeaf;
            searchList.Add(new Leaf() { node = Root, h = heuristic(Root.State), depth = 0 });
            statesCache.Add(searchList[0].node.State, 0);
            while(solution ==  null && searchList.Count > 0)
            {
                currentLeaf = searchList[0];
                searchList.RemoveAt(0);
                if (currentLeaf.node.State.IsGoal)
                {
                   solution = currentLeaf.node;
                }
                else
                {
                    posibleActions = currentLeaf.node.State.PosibleActions();
                    foreach (KeyValuePair<object, State> action in posibleActions)
                    {
                        var child = new Node(currentLeaf.node, action.Value, action.Key);
                        if ((!statesCache.TryGetValue(child.State,out int repeatedDepth) || repeatedDepth > currentLeaf.depth + 1) && !child.State.IsDead())
                        {
                            var child_heuristic = heuristic(child.State);
                            var child_leaf = new Leaf() { node = child, h = child_heuristic, depth = currentLeaf.depth + 1 };
                            statesCache[child.State] = child_leaf.depth;
                            bool added = false;
                            for(int i = 0; i < searchList.Count; i++)
                            {
                                if(searchList[i].h > child_leaf.h)
                                {
                                    searchList.Insert(i, child_leaf);
                                    added = true;
                                    break;
                                }
                            }
                            if (!added) { searchList.Add(child_leaf); }
                        }
                    }
                }

            }
            return solution;
        }
            
    }
}
