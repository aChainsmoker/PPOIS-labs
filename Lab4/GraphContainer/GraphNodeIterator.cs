using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphContainer
{
    public class GraphNodeIterator<T> : IIterator<WirthNode<T>>, IDisposable
    {
        private WirthNode<T>? current;
        private WirthNode<T>? headNode;
        private WirthNode<T>? tailNode;
        private Graph<T> graph;

        private bool reverse;
        private bool constModifier;
        public WirthNode<T>? Current { get => current; set { if (constModifier) return; else current = value; } }

        public GraphNodeIterator(Graph<T> graph, WirthNode<T> headNode, WirthNode<T> tailNode, bool reverse, bool constModifier)
        {
            this.graph = graph;
            this.headNode = headNode;
            this.tailNode = tailNode;
            this.reverse = reverse;
            this.constModifier = constModifier;
            graph.KeyNodesChanged += ChangeKeynodes;
            Reset();
        }


        public bool MoveNext()
        {
            if (reverse == true)
            {
                current = current == null ? FindPreviousNode(tailNode) : FindPreviousNode(current);
            }
            else
            {
                current = (current == null) ? headNode : current.NextWirthNode;
            }

            return current != null && current != tailNode;
        }

        public bool MovePrevious()
        {
            if (reverse == true)
            {
                current = current == null ? headNode : current.NextWirthNode;
            }
            else
            {
                current = current == null ? FindPreviousNode(tailNode) : FindPreviousNode(current);
            }

            return current != null && current != tailNode;
        }

        private WirthNode<T>? FindPreviousNode(WirthNode<T>? node)
        {
            if (node == null || node == headNode)
                return null;

            WirthNode<T>? prevExaminedNode = null;
            WirthNode<T>? examinedNode = headNode;

            while (examinedNode != null && examinedNode != node)
            {
                prevExaminedNode = examinedNode;
                examinedNode = examinedNode.NextWirthNode;
            }

            return prevExaminedNode;
        }

        public void Reset()
        {
            current = reverse ? tailNode : null;
        }

        public void DeleteCurrent()
        {
            if (constModifier == true)
                throw new InvalidOperationException("You cannot delete Current from constant iterator");
            if(current == tailNode)
                throw new InvalidOperationException("Trying to delete fictional element");


            WirthNode<T> nodeToDelete = current;
            this.MoveNext();
            graph.RemoveNode(nodeToDelete);
        }

        private void ChangeKeynodes() { headNode = graph.HeadNode; tailNode = graph.TailNode; }


        public void Dispose() { graph.KeyNodesChanged -= ChangeKeynodes; }
    }
}
