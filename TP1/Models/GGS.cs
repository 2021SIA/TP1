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

        public override Node GetSolution()
        {
            HashSet<State> statesCache = new HashSet<State>();
            SortedList<double, Node> searchList = new SortedList<double, Node>();
            IDictionary<object, State> posibleActions = null;
            Node currentNode = null;
            Node solution = null;
            double currentLimit;

            currentNode = Root;
            currentLimit = heuristic(Root);
            searchList.Add(currentLimit, currentNode);
            statesCache.Add(currentNode.State);

            while(solution ==  null)
            {
                // fijo currentNode es el que tiene menor heuristica
                currentNode = searchList.ElementAt(0).Value;
                currentLimit = searchList.ElementAt(0).Key;
                if(!searchList.Any()){
                    Console.WriteLine("there is no solution");
                    return null;
                }
                else if (currentNode.State.IsGoal)
                {
                    return currentNode;
                }


                posibleActions = currentNode.State.PosibleActions();
                foreach(KeyValuePair<object,State> action in posibleActions)
                {
                    var child = new Node(currentNode, action.Value, action.Key);
                    // si no es un estado repetido ni muerto lo agrega al stack
                    if(!statesCache.Contains(child.State) && !currentNode.State.IsDead() )
                    {
                        var child_heuristic = currentLimit + heuristic(child);
                        searchList.Add(child_heuristic, child);
                        statesCache.Add(currentNode.State);
                    }
                }

            }
            return solution;
        }
            
    }
}
