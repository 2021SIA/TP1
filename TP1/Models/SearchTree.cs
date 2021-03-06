using System;
using System.Collections.Generic;
using System.Text;

namespace TP1.Models
{
    public abstract class SearchTree
    {
        public Node Root { get; private set; }
        public class Node
        {
            public Node Parent { get; }
            public State State { get; set; }
            
            public Node(Node parent, State state)
            {
                Parent = parent;
                State = state;
            }

        }

        protected abstract Node GetSolution();
    }
}
