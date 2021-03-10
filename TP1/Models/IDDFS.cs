using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP1.Models
{
    public class IDDFS : SearchTree
    {
        public int StartingDepth { get; set; }
        public IDDFS(int startingDepth, State root) : base(root)
        {
            StartingDepth = startingDepth;
        }

        private (Node n, int depth) searchSolution(int depthLimit)
        {
            HashSet<State> statesCache = new HashSet<State>();
            Stack<(Node n,int depth)> searchStack = new Stack<(Node n,int depth)>();
            searchStack.Push((Root,0));

            (Node n, int depth) solution = (null, -1), currentNode;
            //Busco una solucion hasta que el stack quede vacio (se llego al limite de profundidad)
            while (solution.n == null && searchStack.Count > 0)
            {
                currentNode = searchStack.Pop();
                //Si se llego al limite de profundidad o es un estado repetido, no lo sigo expandiendo.
                if (currentNode.depth > depthLimit || statesCache.Contains(currentNode.n.State))
                {
                    continue;
                }
                else if (currentNode.n.State.IsGoal())
                {
                    solution = currentNode;
                }
                //Agrego el estado al cache para verificar estados repetidos.
                statesCache.Add(currentNode.n.State);
                //Obtengo las posibles acciones a partir del estado actual y las agrego al stack de busqueda.
                IDictionary<object, State> posibleActions = currentNode.n.State.PosibleActions();
                foreach (KeyValuePair<object, State> action in posibleActions)
                {
                    searchStack.Push((new Node(currentNode.n, action.Value, action.Key),currentNode.depth + 1));
                }
            }
            return solution;
        }
        public override Node GetSolution()
        {
            int currentDepthLimit = StartingDepth, topLimit = StartingDepth, bottomLimit = 0;
            (Node n, int depth) solution;
            Node optimalSolution = null;
            do
            {
                //Busco la solucion con el limite de profundidad actual.
                solution = searchSolution(currentDepthLimit);
                //Si encuentro una solucion, obtengo el limite superior en el cual se puede encontrar la solucion optima.
                if (solution.n != null)
                {
                    optimalSolution = solution.n;
                    topLimit = solution.depth;
                    currentDepthLimit -= (topLimit - bottomLimit) / 2;
                }
                //Si no encuentro una solucion, obtengo el limite inferior en el cual se puede encontrar la solucion optima.
                else
                {
                    bottomLimit = currentDepthLimit;
                    if (optimalSolution != null)
                        currentDepthLimit += (topLimit - bottomLimit) / 2;
                    else
                        topLimit = currentDepthLimit = currentDepthLimit + StartingDepth;
                }

            //Busco una solucion hasta converger en la solucion optima.
            } while (topLimit - 1 > bottomLimit);

            return optimalSolution;
        }
    }
}
