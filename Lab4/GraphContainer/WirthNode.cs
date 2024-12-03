using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphContainer
{
    public class WirthNode<T>
    {
        private T content;
        int amountOfRecipients; 
        int amountOfSenders; 
        WirthEdge<T> edgeFromSender; 
        WirthEdge<T> edgeToRecipient; 
        WirthNode<T> nextWirthNode;

        public T Content { get { return content; } set { content = value; } }
        public WirthNode<T> NextWirthNode { get { return nextWirthNode; } set { nextWirthNode = value; } }
        public WirthEdge<T> EdgeToRecipient {  get { return edgeToRecipient; } set { edgeToRecipient = value; } }
        public WirthEdge<T> EdgeFromSenders { get { return edgeFromSender; } set { edgeFromSender = value; } }
        public int AmountOfRecipients { get { return amountOfRecipients; } set { amountOfRecipients = value; } }
        public int AmountOfSenders { get { return amountOfSenders; } set {amountOfSenders = value; } }

        public NodeAdjacentNodesIterator<T> GetNodeAdjacentNodesIterator(Graph<T> graph, bool isItReverse, bool isItConst)
        {
            if (graph.CheckNodeBelonging(this) == false)
                throw new InvalidOperationException("Node does not belong to the graph");
            return new NodeAdjacentNodesIterator<T>(graph, this, isItReverse, isItConst);
        }
        public NodeEdgeIterator<T> GetNodeEdgesIterator(Graph<T> graph, bool isItReverse, bool isItConst)
        {
            if (graph.CheckNodeBelonging(this) == false)
                throw new InvalidOperationException("Node does not belong to the graph");
            return new NodeEdgeIterator<T>(graph, this, isItReverse, isItConst);
        }

        public IEnumerable<WirthNode<T>> GetAdjacentNodes(Graph<T> graph, bool isItReverse, bool isItConst)
        {
            NodeAdjacentNodesIterator<T> iterator = GetNodeAdjacentNodesIterator(graph, isItReverse, isItConst);
            while (iterator.MoveNext())
                yield return iterator.Current;
            yield break;
        }
        public IEnumerable<WirthEdge<T>> GetEdges(Graph<T> graph, bool isItReverse, bool isItConst)
        {
            NodeEdgeIterator<T> iterator = GetNodeEdgesIterator(graph, isItReverse, isItConst);
            while (iterator.MoveNext())
                yield return iterator.Current;
            yield break;
        }

        public void Clear()
        {
            if (nextWirthNode != null)
            {
                nextWirthNode.Clear();
                nextWirthNode = null;
            }
            ClearEdges();
        }
        public void ClearEdges()
        {
            if (edgeFromSender != null)
            {
                edgeFromSender.Clear();
                edgeFromSender = null;
            }
            if (edgeToRecipient != null)
            {
                edgeToRecipient.Clear();
                edgeToRecipient = null;
            }
        }
    }
}
