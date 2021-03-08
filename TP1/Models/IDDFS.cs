using System;
using System.Collections.Generic;
using System.Text;

namespace TP1.Models
{
    public class IDDFS : SearchTree
    {
        public int StartingDepth { get; set; }
        public IDDFS(int startingDepth)
        {
            StartingDepth = startingDepth;
        }

        private Node searchSolution(Node n, int currentDepth, int depthLimit, HashSet<State> statesCache)
        {
            //Si llego a la profundidad limite o es un estado repetido, retorno null.
            if(currentDepth > depthLimit || statesCache.Contains(n.State))
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
            foreach(KeyValuePair<object,State> action in posibleActions)
            {
                //Busco la solucion a partir de los estados siguientes.
                solution = searchSolution(new Node(n, action.Value), currentDepth + 1, depthLimit, statesCache);
                //Si obtuve una solucion, dejo de buscar para retornarla.
                if (solution != null) break;
            }
            return solution;
        }
        protected override Node GetSolution()
        {
            int currentDepthLimit = StartingDepth, topLimit = StartingDepth, bottomLimit = 0;
            bool foundSolution = false;
            Node solution = null;
            HashSet<State> statesCache = new HashSet<State>();

            //Busco una solucion hasta converger en la solucion optima.
            while (topLimit != bottomLimit)
            {
                //Busco la solucion a partir de la raiz.
                solution = searchSolution(Root, 0, currentDepthLimit, statesCache);

                //Si encuentro una solucion, obtengo el limite superior en el cual se puede encontrar la solucion optima.
                if (solution != null)
                {
                    foundSolution = true;
                    topLimit = currentDepthLimit;
                    currentDepthLimit -= (topLimit - bottomLimit) / 2;
                }
                //Si no encuentro una solucion, obtengo el limite inferior en el cual se puede encontrar la solucion optima.wsd
                else
                {
                    bottomLimit = currentDepthLimit;
                    if (foundSolution)
                        currentDepthLimit += (topLimit - bottomLimit) / 2;
                    else
                        topLimit = currentDepthLimit = currentDepthLimit * 2;
                }
            }
            return solution;
        }
    }
}
