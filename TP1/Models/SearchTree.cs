using System;
using System.Collections.Generic;
using System.Text;

namespace TP1.Models
{
    public abstract class SearchTree
    {
        public delegate double HeuristicFunction(State s);

        public Node Root { get; private set; }
        protected SearchTree(State root)
        {
            Root = new Node(null, root);
        }
        public abstract Node GetSolution();

        public class Node
        {
            public Node Parent { get; }
            public State State { get; set; }
            public object Action { get; set; }
            
            public Node(Node parent, State state)
            {
                Parent = parent;
                State = state;
            }
            public Node(Node parent, State state, object action) : this(parent, state)
            {
                Action = action;
            }
        }
    }
}
