using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphContainer
{
    public class NodeEdgeIterator<T> : IIterator<WirthEdge<T>>
    {
        private Graph<T> graph;
        private WirthNode<T> currentNode;
        private WirthEdge<T> current;
        private bool reverse;
        private bool constModifier;

        public WirthEdge<T> Current { get => current; set { if (constModifier) return; else current = value; } }

        public NodeEdgeIterator(Graph<T> graph, WirthNode<T> node, bool reverse, bool constModifier)
        {
            this.graph = graph;
            this.currentNode = node;
            this.reverse = reverse;
            this.constModifier = constModifier;
            Reset();
        }

        public bool MoveNext()
        {
            if (reverse == true)
                current = current == null ? GetLastRecipientsEdge(currentNode) : FindPreviousEdge(currentNode, current);
            else
            {
                current = current == null ? GetFirstEdge(currentNode) : GetNextEdge(current);
            }
            return current != null;
        }

        public bool MovePrevious()
        {
            if (reverse == true)
            {
                current = current == null ? GetFirstEdge(currentNode) : GetNextEdge(current);
            }
            else
            {
                current = current == null ? GetLastRecipientsEdge(currentNode) : FindPreviousEdge(currentNode, current);
            }
            return current != null;
        }

        public void Reset()
        {
            current = null;
        }
        public void DeleteCurrent()
        {
            if (constModifier == true)
                throw new InvalidOperationException("You cannot delete Current from constant iterator");

            WirthNode<T> nodeToDelete = current.Node;
            this.MoveNext();
            graph.RemoveEdge(currentNode, nodeToDelete);
        }


        private WirthEdge<T> GetNextEdge(WirthEdge<T> edge)
        {
            if (edge == GetLastSendersEdge(currentNode))
                return currentNode.EdgeToRecipient;
            else
                return current.NextEdge;
            
        }
        private WirthEdge<T> GetFirstEdge(WirthNode<T> node)
        {

            if (node.EdgeFromSenders != null)
                return node.EdgeFromSenders;
            if(node.EdgeToRecipient != null)
                return node.EdgeToRecipient;
            return null;
        }
        private WirthEdge<T>? FindPreviousEdge(WirthNode<T>? node, WirthEdge<T>? edge)
        {
            if(edge == node.EdgeFromSenders || edge == null)
                return null;
            
            else if(edge == node.EdgeToRecipient)
            {
                return GetLastSendersEdge(node);
            }
            else
            {
                WirthEdge<T>? examinedEdge = node.EdgeFromSenders;
                while (examinedEdge != null && examinedEdge.NextEdge != edge)
                {
                    examinedEdge = examinedEdge.NextEdge;
                }
                examinedEdge = node.EdgeToRecipient;
                while (examinedEdge != null && examinedEdge.NextEdge != edge)
                {
                    examinedEdge = examinedEdge.NextEdge;
                }
                return examinedEdge;
            }


        }

        private WirthEdge<T>? GetLastRecipientsEdge(WirthNode<T>? node)
        {
            WirthEdge<T>? examinedEdge = node.EdgeToRecipient;
            while (examinedEdge != null && examinedEdge.NextEdge != null)
            {
                examinedEdge = examinedEdge.NextEdge;
            }
            return examinedEdge;
        }
        private WirthEdge<T>? GetLastSendersEdge(WirthNode<T>? node)
        {
            WirthEdge<T>? examinedEdge = node.EdgeFromSenders;
            while (examinedEdge != null && examinedEdge.NextEdge != null)
            {
                examinedEdge = examinedEdge.NextEdge;
            }
            return examinedEdge;
        }

    }
}
