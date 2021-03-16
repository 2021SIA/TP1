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
        }

        public override Node GetSolution()
        {
            HashSet<State> statesCache = new HashSet<State>();
            List<Leaf> searchList = new List<Leaf>();
            IDictionary<object, State> posibleActions = null;
            Node solution = null;
            Leaf currentLeaf = new Leaf(){node = Root, h = heuristic(Root)};
            
            searchList.Add(currentLeaf);
            statesCache.Add(currentLeaf.node.State);

            while(solution ==  null)
            {
                // fijo currentNode es el que tiene menor heuristica
                currentLeaf = searchList[0];
                if (!searchList.Any()){
                    Console.WriteLine("there is no solution");
                    return null;
                }
                else if (currentLeaf.node.State.IsGoal)
                {
                    return currentLeaf.node;
                }

                searchList.RemoveAt(0);


                posibleActions = currentLeaf.node.State.PosibleActions();
                foreach(KeyValuePair<object,State> action in posibleActions)
                {
                    var child = new Node(currentLeaf.node, action.Value, action.Key);
                    // si no es un estado repetido ni muerto lo agrega al stack
                    if(!statesCache.Contains(child.State) && !currentLeaf.node.State.IsDead() )
                    {
                        Console.WriteLine("adding leaf");
                        var child_heuristic = currentLeaf.h + heuristic(child);
                        var child_leaf = new Leaf() { node = child, h = child_heuristic};
                        searchList.Add(child_leaf);
                        statesCache.Add(currentLeaf.node.State);
                        searchList = searchList.OrderBy(leaf => leaf.h).ToList();
                    }
                }

            }
            return solution;
        }
            
    }
}
