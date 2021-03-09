using System;
using System.Collections.Generic;
using System.Text;

namespace TP1.Models
{
    public class DFS : SearchTree
    {   
        public DFS(State root) : base(root) { }
        private Node searchSolution(Node n, HashSet<State> statesCache)
        {
            //Obtengo las posibles acciones a partir del estado actual.
            IDictionary<object, State> posibleActions = n.State.PosibleActions();
            Node solution = null;
            List<Node> posibleChildren = new List<Node>();
            //Me fijo si alguno de sus hijos es la solucion
            foreach(KeyValuePair<object,State> action in posibleActions)
            {
                var child = new Node(n, action.Value, action.Key);
                if(child.State.IsGoal()){
                    return child;
                }
                // si no lo es, y no es un estado repetido lo agrega a posible children
                else if(!statesCache.Contains(child.State))
                {
                    posibleChildren.Add(child);
                    statesCache.Add(child.State);
                }
            }
            
            foreach(Node child in posibleChildren)
            {
                //Busco la solucion a partir de los estados siguientes.
                solution = searchSolution(child, statesCache);
                //Si obtuve una solucion, dejo de buscar para retornarla.
                if (solution != null) break;
            }
            return solution;
        }
        public override Node GetSolution()
        {
            Node solution = null;
            HashSet<State> statesCache = new HashSet<State>();

            if (Root.State.IsGoal())
            {
                return Root;
            }
            //Agrego el estado al cache para verificar estados repetidos.
            statesCache.Add(Root.State);

            //Busco la solucion a partir de la raiz.
            solution = searchSolution(Root, statesCache);
            return solution;
        }
    }
}
