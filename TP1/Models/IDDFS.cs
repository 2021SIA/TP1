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

        private (Node n, int depth) searchSolution(int depthLimit, Stack<(Node n, int depth)> frontier, out int expanded)
        {
            IDictionary<State, int> statesCache = new Dictionary<State, int>();
            Stack<(Node n,int depth)> searchStack = new Stack<(Node n,int depth)>();
            expanded = 0;
            while(frontier.Count > 0) searchStack.Push(frontier.Pop());

            (Node n, int depth) solution = (null, -1), currentNode;
            //Busco una solucion hasta que el stack quede vacio (se llego al limite de profundidad)
            while (solution.n == null && searchStack.Count > 0)
            {
                currentNode = searchStack.Pop();
                //Si se llego al limite de profundidad o es un estado repetido, no lo sigo expandiendo.
                if (currentNode.depth > depthLimit)
                {
                    frontier.Push(currentNode);
                }
                else if (currentNode.n.State.IsGoal)
                {
                    solution = currentNode;
                }
                else if (!statesCache.TryGetValue(currentNode.n.State, out int repeatedDepth) || repeatedDepth > currentNode.depth)
                {
                    expanded++;
                    //Agrego el estado al cache para verificar estados repetidos.
                    statesCache[currentNode.n.State] = currentNode.depth;
                    //Obtengo las posibles acciones a partir del estado actual y las agrego al stack de busqueda.
                    IDictionary<object, State> posibleActions = currentNode.n.State.PosibleActions();
                    foreach (KeyValuePair<object, State> action in posibleActions)
                    {
                        searchStack.Push((new Node(currentNode.n, action.Value, action.Key), currentNode.depth + 1));
                    }
                }
            }
            while (searchStack.Count > 0) frontier.Push(searchStack.Pop());
            return solution;
        }
        public override Node GetSolution(out int expanded, out int frontierCount)
        {
            int currentDepthLimit = StartingDepth, topLimit = StartingDepth, bottomLimit = 0;
            (Node n, int depth) solution;
            Node optimalSolution = null;
            Stack<(Node n, int depth)> frontier = new Stack<(Node n, int depth)>();
            expanded = 0;

            frontier.Push((Root, 0));
            do
            {
                //Busco la solucion con el limite de profundidad actual.
                solution = searchSolution(currentDepthLimit,frontier, out int expandedLast);
                expanded += expandedLast;
                //Si encuentro una solucion, obtengo el limite superior en el cual se puede encontrar la solucion optima.
                if (solution.n != null)
                {
                    optimalSolution = solution.n;
                    topLimit = solution.depth;
                }
                //Si no encuentro una solucion, obtengo el limite inferior en el cual se puede encontrar la solucion optima.
                else
                {
                    bottomLimit = currentDepthLimit;
                    //Si no encontre nunca una solucion todavia, aumento el limite de profundidad y vuelvo a intentar de vuelta.
                    if (optimalSolution == null)
                    {
                        topLimit = currentDepthLimit = currentDepthLimit + StartingDepth;
                        continue;
                    }
                }
                currentDepthLimit = bottomLimit + (topLimit - bottomLimit) / 2;

            //Busco una solucion hasta converger en la solucion optima.
            } while (topLimit - 1 > bottomLimit);

            frontierCount = frontier.Count;
            return optimalSolution;
        }
    }
}
