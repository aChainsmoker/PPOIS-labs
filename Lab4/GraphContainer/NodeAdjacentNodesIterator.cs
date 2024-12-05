using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphContainer
{
    public class NodeAdjacentNodesIterator<T> : IIterator<WirthNode<T>>
    {
        private Graph<T> graph;
        private WirthNode<T> mainNode;
        private WirthNode<T> current;
        private bool reverse;
        private bool constModifier;

        public WirthNode<T> Current { get => current; set { if (constModifier) throw new Exception("You cannot modify constant iterator"); else current = value; } }

        public NodeAdjacentNodesIterator(Graph<T>graph, WirthNode<T> mainNode, bool reverse, bool constModifier)
        {
            this.graph = graph;
            this.mainNode = mainNode;
            this.reverse = reverse;
            this.constModifier = constModifier;
            Reset();

        }


        public bool MoveNext()
        {
            if (reverse == true)
            {
                current = current == null ? GetLastAdjacentNode(mainNode) : FindPreviousAdjacentNode(current);
            }
            else
            {
                current = current == null ? GetFirstAdjacentNode(mainNode) : FindNextAdjacentNode(current);
            }
            return current!= null;
        }

        public bool MovePrevious()
        {
            if (reverse == true)
            {
                current = current == null ? GetFirstAdjacentNode(mainNode) : FindNextAdjacentNode(current);
            }
            else
            {
                current = current == null ? GetLastAdjacentNode(mainNode) : FindPreviousAdjacentNode(current);
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

            WirthNode<T> nodeToDelete = current;
            this.MoveNext();
            graph.RemoveNode(nodeToDelete);
            
        }

        private WirthNode<T> FindNextAdjacentNode(WirthNode<T> node)
        {
            if (node == null)
                return null;
            else
            {
                WirthEdge<T> examinedEdge = mainNode.EdgeFromSenders;
                while(examinedEdge != null && examinedEdge.Node != node)
                {
                    examinedEdge = examinedEdge.NextEdge;
                }
                if(examinedEdge == null)
                {
                    examinedEdge = mainNode.EdgeToRecipient;
                    while (examinedEdge != null && examinedEdge.Node != node)
                    {
                        examinedEdge = examinedEdge.NextEdge;
                    }
                }
                if (examinedEdge == null || examinedEdge.NextEdge == null)
                    return null;
                else
                    return examinedEdge.NextEdge.Node;
            }
        }

        private WirthNode<T> GetLastAdjacentNode(WirthNode<T> node)
        {
            WirthNode<T> lastAdjacentNode = GetFirstAdjacentNode(node);
            while(FindNextAdjacentNode(lastAdjacentNode) != null)
            {
                lastAdjacentNode = FindNextAdjacentNode(lastAdjacentNode);
            }
            return lastAdjacentNode;
        }

        private WirthNode<T> GetFirstAdjacentNode(WirthNode<T> node)
        {
            if (node.EdgeFromSenders != null)
                return node.EdgeFromSenders.Node;
            if (node.EdgeToRecipient != null)
                return node.EdgeToRecipient.Node;
            return null;
        }

        private WirthNode<T>? FindPreviousAdjacentNode(WirthNode<T>? node)
        {
            if (node == null)
                return null;
            else
            {
                WirthEdge<T> prevExaminedEdge = null;
                WirthEdge<T> examinedEdge = mainNode.EdgeFromSenders;
                while (examinedEdge != null && examinedEdge.Node != node)
                {
                    prevExaminedEdge = examinedEdge;
                    examinedEdge = examinedEdge.NextEdge;
                }
                if (examinedEdge == null)
                {
                    examinedEdge = mainNode.EdgeToRecipient;
                    while (examinedEdge != null && examinedEdge.Node != node)
                    {
                        prevExaminedEdge = examinedEdge;
                        examinedEdge = examinedEdge.NextEdge;
                    }
                }
                if (prevExaminedEdge == null)
                    return null;
                else
                    return prevExaminedEdge.Node;
            }
        }

    }
}
