using System;
using System.Collections.Generic;
using System.Text;

namespace TP1.Models
{
    public class DFS : SearchTree
    {
        public int StartingDepth { get; set; }
        public DFS()
        {
            StartingDepth = startingDepth;
        }
        
        private Node search(){

        }
        private Node searchSolution(Node n, HashSet<State> statesCache)
        {
            //Si es un estado repetido, retorno null.
            if(statesCache.Contains(n.State))
            {
                return null;
            }
            //Si llego a un estado que es solucion, retorno el nodo.
            else if (n.State.IsGoal())
            {
                return n;
            }
            //Agrego el estado al cache para verificar estados repetidos.
            statesCache.Add(n.State);
            //Obtengo las posibles acciones a partir del estado actual.
            IDictionary<object, State> posibleActions = n.State.PosibleActions();
            Node solution = null;
            List<Node> posibleChildren = null;
            //Me fijo si alguno de sus hijos es la solucion
            foreach(KeyValuePair<object,State> action in posibleActions)
            {
                child = new Node(n, action.Value);
                if(child.State.IsGoal()){
                    return child;
                }
                else if(!statesCache.Contains(child.State))
                {
                    posibleChildren.Add(child);
                    statesCache.Add(child.State);
                }
            }
            //Expando los hijos
            foreach(Node child in posibleChildren)
            {
                //Busco la solucion a partir de los estados siguientes.
                solution = searchSolution(child, statesCache);
                //Si obtuve una solucion, dejo de buscar para retornarla.
                if (solution != null) break;
            }
            return solution;
        }
        protected override Node GetSolution()
        {
            Node solution = null;
            HashSet<State> statesCache = new HashSet<State>();
            
            //Busco la solucion a partir de la raiz.
            solution = searchSolution(Root, statesCache);
            return solution;
        }
    }
}
