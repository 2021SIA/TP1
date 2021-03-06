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
            private State State { get; set; }
            IEnumerable<Node> Children { get => children.AsReadOnly(); }
            
            private List<Node> children;

            public Node(State state)
            {
                State = state;
                children = new List<Node>();
            }

            public void AddChildren(Node node)
            {
                children.Add(node);
            }
        }

        protected abstract Node NextNode();
    }
}
