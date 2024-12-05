using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphContainer
{
    public class GraphEdgeIterator<T> : IIterator<WirthEdge<T>>, IDisposable
    {
        private WirthEdge<T> current;
        private WirthNode<T> headNode;
        private WirthNode<T> tailNode;
        private WirthNode<T> currentNode;
        private Graph<T> graph;
        private bool reverse;
        private bool constModifier;

        public WirthEdge<T> Current { get => current; set { if (constModifier) throw new Exception("You cannot modify constant iterator"); else current = value; } }

        public GraphEdgeIterator(Graph<T> graph, WirthNode<T> headNode,WirthNode<T> tailNode, bool reverse, bool constModifier)
        {
            this.graph = graph;
            this.headNode = headNode;
            this.tailNode = tailNode;
            currentNode = headNode;
            this.reverse = reverse;
            this.constModifier = constModifier;
            graph.KeyNodesChanged += ChangeKeynodes;
            Reset();
        }

        public bool MoveNext()
        {
            if(reverse == true)
            {
                current = current == null ? FindLastEdge() : FindPreviousEdge(currentNode, current);
            }
            else
            {
                current = current == null ? FindFirstEdge() : FindNextEdge(current);
            }
            return current != null;
        }


        public bool MovePrevious()
        {
            if(reverse == true)
            {
                current = current == null ? FindFirstEdge() : FindNextEdge(current);
            }
            else
            {
                current = current == null ? FindLastEdge() : FindPreviousEdge(currentNode, current);
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

        private void ChangeKeynodes() { headNode = graph.HeadNode; tailNode = graph.TailNode; }


        public void Dispose() { graph.KeyNodesChanged -= ChangeKeynodes; }


        private WirthEdge<T> FindFirstEdge()
        {
            WirthNode<T> eximinedNode = headNode;
            while(eximinedNode != tailNode && eximinedNode.EdgeToRecipient == null)
                eximinedNode = eximinedNode.NextWirthNode;
            currentNode = eximinedNode; 
            return eximinedNode.EdgeToRecipient;
        }

        private WirthEdge<T> FindLastEdge()
        {
            currentNode = tailNode;
            while (FindPreviousNode(currentNode) != null && GetLastEdge(FindPreviousNode(currentNode)) == null)
                currentNode = FindPreviousNode(currentNode);

            currentNode = FindPreviousNode(currentNode);
            return GetLastEdge(currentNode);
        }
        private WirthNode<T>? FindPreviousNode(WirthNode<T>? node)
        {
            if (node == null || node == headNode)
                return null;

            WirthNode<T>? PrevExaminedNode = null;
            WirthNode<T>? examinedNode = headNode;

            while (examinedNode != null && examinedNode != node)
            {
                PrevExaminedNode = examinedNode;
                examinedNode = examinedNode.NextWirthNode;
            }

            return PrevExaminedNode;
        }

        private WirthEdge<T> FindNextEdge(WirthEdge<T>  edge)
        {
            if (edge.NextEdge == null)
            {
                do
                {
                    currentNode = currentNode.NextWirthNode;
                    edge = currentNode.EdgeToRecipient;
                }
                while (currentNode.EdgeToRecipient == null && currentNode != tailNode);
            }
            else
                return edge.NextEdge;
            return edge;
        }

        private WirthEdge<T>? FindPreviousEdge(WirthNode<T>? node, WirthEdge<T>? edge)
        {
            if (node == headNode && edge == node.EdgeToRecipient)
                return null;
            else if(node != headNode && edge == node.EdgeToRecipient)
            {
                do 
                {
                    currentNode = FindPreviousNode(currentNode);
                }
                while (currentNode != null && currentNode.EdgeToRecipient == null) ;

                return GetLastEdge(currentNode);
            }
            else
            {
                WirthEdge<T>? prevExaminedEdge = null;
                WirthEdge<T>? examinedEdge = currentNode.EdgeToRecipient;

                while (examinedEdge != null && examinedEdge != edge)
                {
                    prevExaminedEdge = examinedEdge;
                    examinedEdge = examinedEdge.NextEdge;
                }
                return prevExaminedEdge;

            }
        }

        private WirthEdge<T>? GetLastEdge(WirthNode<T>? node)
        {
            if (node == null)
                return null;
            WirthEdge<T>? examinedEdge = node.EdgeToRecipient;
            if (examinedEdge == null)
                return null;
            while(examinedEdge.NextEdge != null)
            {
                examinedEdge = examinedEdge.NextEdge;
            }
            return examinedEdge;

        }

    }
}
