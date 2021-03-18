using System;
using System.Collections.Generic;
using System.Text;

namespace TP1.Models
{
    public class IDA : SearchTree
    {
        HeuristicFunction heuristic;

        public IDA(State root, HeuristicFunction heuristic) : base(root)
        {
            this.heuristic = heuristic;
        }

        public override Node GetSolution(out int expandedNodes, out int frontierCount)
        {
            IDictionary<State, int> statesCache = new Dictionary<State, int>();
            Stack<(Node n, int depth)> searchStack = new Stack<(Node n, int depth)>();
            Stack<(Node n, int depth)> frontier = new Stack<(Node n, int depth)>();
            frontier.Push((Root,0));
            expandedNodes = 0;
            double currentLimit = heuristic(Root.State), newLimit;
            (Node n, int depth) solution = (null, -1), currentNode;

            while (solution.n == null)
            {
                newLimit = -1;
                statesCache.Clear();
                while (frontier.Count > 0) searchStack.Push(frontier.Pop());
                //Busco una solucion hasta que el stack quede vacio (se llego al limite)
                while (searchStack.Count > 0)
                {
                    currentNode = searchStack.Pop();
                    //Si es un estado repetido o muerto no lo expando.
                    if((statesCache.TryGetValue(currentNode.n.State, out int repeatedDepth) && repeatedDepth <= currentNode.depth) || currentNode.n.State.IsDead())
                    {
                        continue;
                    }
                    double heuristicValue = heuristic(currentNode.n.State);
                    //Si se llego al limite de la heuristica, obtengo el nuevo limite.
                    if (currentNode.depth + heuristicValue > currentLimit)
                    {
                        //Guardo el nuevo limite para la siguiente iteracion en caso de que no se encuentre solucion.
                        if (newLimit == -1 || currentNode.depth + heuristicValue < newLimit)
                        {
                            newLimit = currentNode.depth + heuristicValue;
                        }
                        frontier.Push(currentNode);
                    }
                    //Si encuentro una solucion, termino la busqueda.
                    else if (currentNode.n.State.IsGoal)
                    {
                        solution = currentNode;
                        break;
                    }
                    else
                    {
                        //Agrego el estado al cache para verificar estados repetidos.
                        statesCache[currentNode.n.State] = currentNode.depth;
                        //Obtengo las posibles acciones a partir del estado actual y las agrego al stack de busqueda.
                        IDictionary<object, State> posibleActions = currentNode.n.State.PosibleActions();
                        foreach (KeyValuePair<object, State> action in posibleActions)
                        {
                            searchStack.Push((new Node(currentNode.n, action.Value, action.Key), currentNode.depth + 1));
                        }
                        //Aumento la cantidad de nodos expandidos.
                        expandedNodes++;
                    }
                }
                currentLimit = newLimit;
            }
            frontierCount = frontier.Count;
            return solution.n;
        }
    }
}
