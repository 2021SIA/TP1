using System;
using System.Collections.Generic;
using System.Text;

namespace TP1.Models
{
    public class DFS : SearchTree
    {   
        public DFS(State root) : base(root) { }
        private Node searchSolution(HashSet<State> statesCache, Stack<Node> searchStack)
        {
            //Obtengo las posibles acciones a partir del estado actual.
            IDictionary<object, State> posibleActions = null;
            Node solution = null;
            Node currentNode = null;
            while(searchStack.Count > 0)
            {
                currentNode = searchStack.Pop();
                if (statesCache.Contains(currentNode.State))
                {
                    continue;
                }
                else if (currentNode.State.IsGoal)
                {
                    return currentNode;
                }
                statesCache.Add(currentNode.State);
                posibleActions = currentNode.State.PosibleActions();
                
                foreach(KeyValuePair<object,State> action in posibleActions)
                {
                    var child = new Node(currentNode, action.Value, action.Key);
                    // si no es un estado repetido ni muerto lo agrega al stack
                    if(!statesCache.Contains(child.State) )
                    {
                        searchStack.Push(child);
                    }
                }
            }
            
            return solution;
        }
        public override Node GetSolution()
        {
            Node solution = null;
            HashSet<State> statesCache = new HashSet<State>();
            Stack<Node> searchStack = new Stack<Node>();
            
            //Agrego el estado al cache para verificar estados repetidos.
            searchStack.Push(Root);
            
            //Busco la solucion a partir de la raiz.
            solution = searchSolution(statesCache, searchStack);
            return solution;
        }
    }
}
