using System.Xml.Linq;

namespace GraphContainer
{
    public class Graph<T>
    {
        private WirthNode<T>? headNode;
        private WirthNode<T>? tailNode;


        public WirthNode<T>? HeadNode { get => headNode; }
        public WirthNode<T>? TailNode { get => tailNode; }
        public Action KeyNodesChanged;
        public Graph() { headNode = tailNode = new WirthNode<T>(); }
        public Graph(Graph<T> graph) 
        {
            headNode = tailNode = new WirthNode<T>();
            var graphNodeIter = graph.GetVertexIterator(false, false);

            while (graphNodeIter.MoveNext())
                this.AddNode(graphNodeIter.Current.Content);
            graphNodeIter.MoveNext();
            while(graphNodeIter.MoveNext())
            {
                var nodeIter = graphNodeIter.Current.GetNodeAdjacentNodesIterator(graph, false, false);
                while (nodeIter.MoveNext())
                {
                    if(this.FindEdgeBetweenNodes(graphNodeIter.Current, nodeIter.Current) != null)
                    {
                        WirthNode<T> node1 = FindNode(graphNodeIter.Current.Content);
                        WirthNode<T> node2 = FindNode(nodeIter.Current.Content);
                        this.AddEdge(node1, node2);
                    }
                }
            }
            graphNodeIter.Dispose();
        }


        public WirthNode<T> FindNode(T searchData)
        {
            WirthNode<T> examinedNode = headNode;
            while (examinedNode != tailNode)
            {
                if (object.Equals(examinedNode.Content, searchData))
                    return examinedNode;
                examinedNode = examinedNode.NextWirthNode;
            }
            return null;
        }

        public WirthEdge<T> FindEdgeBetweenNodes(WirthNode<T> node1, WirthNode<T> node2)
        {
            WirthEdge<T> examinedEdge = node1.EdgeToRecipient;
            while (examinedEdge != null)
            {
                if (examinedEdge.Node == node2)
                    return examinedEdge;
                examinedEdge = examinedEdge.NextEdge;
            }
            return null;
        }

        public int GetAmountOfNodes()
        {
            WirthNode<T> examinedNode = headNode;
            int amount = 0;
            while (examinedNode != tailNode)
            {
                ++amount;
                examinedNode = examinedNode.NextWirthNode;
            }
            return amount;
        }

        public int GetAmountOfEdges()
        {
            WirthNode<T> examinedNode = headNode;
            int amount = 0;
            while (examinedNode != tailNode)
            {
                amount += examinedNode.AmountOfRecipients;
                examinedNode = examinedNode.NextWirthNode;
            }
            return amount;
        }

        public int GetVertexDegree(WirthNode<T> node)
        {
            int degree = node.AmountOfRecipients + node.AmountOfSenders;
            return degree;
        }

        public int GetEdgeDegree(WirthEdge<T> edge)
        {
            int degree = edge.Node.AmountOfRecipients + edge.Node.AmountOfSenders;
            return degree;
        }

        public WirthNode<T> AddNode(T content)
        {
            if (FindNode(content) != null)
                throw new InvalidOperationException("Node with such content already exists");

            WirthNode<T> node = new WirthNode<T> { Content = content };
            if (headNode == tailNode)
                headNode = node;
            else
            {
                WirthNode<T> examinedNode = headNode;
                while (examinedNode.NextWirthNode != tailNode)
                    examinedNode = examinedNode.NextWirthNode;
                examinedNode.NextWirthNode = node;
            }
            node.NextWirthNode = tailNode;
            return node;
        }

        public WirthEdge<T> AddEdge(WirthNode<T> node1, WirthNode<T> node2)
        {
            if (CheckNodeBelonging(node1) == false || CheckNodeBelonging(node2) == false)
                throw new InvalidOperationException("Nodes do not belong to the graph");

            WirthEdge<T> edgeToRecipient = new WirthEdge<T> { Node = node2 };
            WirthEdge<T> edgeFromSender = new WirthEdge<T> { Node = node1 };

            WirthEdge<T> examinedEdge = node1.EdgeToRecipient;
            AddingEdgeToTheCollection(ref examinedEdge, edgeToRecipient);
            node1.EdgeToRecipient = examinedEdge;
            node1.AmountOfRecipients++;

            examinedEdge = node2.EdgeFromSenders;
            AddingEdgeToTheCollection(ref examinedEdge, edgeFromSender);
            node2.EdgeFromSenders = examinedEdge;
            node2.AmountOfSenders++;

            return edgeToRecipient;
        }

        private void AddingEdgeToTheCollection(ref WirthEdge<T> edgeCollectionToIncrease, WirthEdge<T> edgeToAdd)
        {
            WirthEdge<T> examinedEdge = edgeCollectionToIncrease;
            if (examinedEdge == null)
                edgeCollectionToIncrease = edgeToAdd;
            else
            {
                while (examinedEdge.NextEdge != null)
                    examinedEdge = examinedEdge.NextEdge;
                examinedEdge.NextEdge = edgeToAdd;
            }
        }



        public void RemoveNode(WirthNode<T> node)
        {
            if (CheckNodeBelonging(node) == false)
                throw new InvalidOperationException("Node does not belong to the graph");
            DeleteEdgeFromRecipients(node);
            DeleteEdgeFromSenders(node);
            //Удаление самого узла
            WirthNode<T> prevNode = null;
            WirthNode<T> examinedNode = headNode;


            while (examinedNode != node)
            {
                prevNode = examinedNode;
                examinedNode = examinedNode.NextWirthNode;
            }

            if (node.NextWirthNode != null && prevNode != null)
            {
                prevNode.NextWirthNode = node.NextWirthNode;
                node = null;
            }
            else if (node.NextWirthNode != null && prevNode == null)
            {
                headNode = node.NextWirthNode;
                KeyNodesChanged?.Invoke();
                node = null;
            }
            else
            {
                node = null;
                node = tailNode;
            }
        }

        private void DeleteEdgeFromRecipients(WirthNode<T> node)
        {
            //Удаление получателей дуг
            DeletingEdgesByDeletingNodes(node, node.EdgeToRecipient, false);
        }
        private void DeleteEdgeFromSenders(WirthNode<T> node)
        {
            //Удаление отправителей дуг
            DeletingEdgesByDeletingNodes(node, node.EdgeFromSenders, true);
        }
        private void DeletingEdgesByDeletingNodes(WirthNode<T> node, WirthEdge<T> edgeCollectionToIterate, bool isDeletingRecipients)
        {
            WirthEdge<T> edgeCollectionToDecrease;
            WirthEdge<T> examinedEdge = edgeCollectionToIterate;
            if (examinedEdge == null)
                return;
            while (examinedEdge != null)
            {
                if (isDeletingRecipients == true)
                {
                    examinedEdge.Node.AmountOfRecipients--;
                    edgeCollectionToDecrease = examinedEdge.Node.EdgeToRecipient;
                }
                else
                {
                    examinedEdge.Node.AmountOfSenders--;
                    edgeCollectionToDecrease = examinedEdge.Node.EdgeFromSenders;
                }
                WirthEdge<T> edgeToDeletePrev = null;
                WirthEdge<T> edgeToDelete = edgeCollectionToDecrease;
                if (edgeCollectionToDecrease == null)
                    return;
                while (edgeToDelete.Node != node)
                {
                    edgeToDeletePrev = edgeToDelete;
                    edgeToDelete = edgeToDelete.NextEdge;
                }
                if (edgeToDelete.NextEdge != null && edgeToDeletePrev != null)
                {
                    edgeToDeletePrev.NextEdge = edgeToDelete.NextEdge;
                    edgeToDelete = null;
                }
                else if (edgeToDelete.NextEdge != null && edgeToDeletePrev == null)
                {
                    edgeCollectionToDecrease = edgeToDelete.NextEdge;
                    edgeToDelete = null;
                }
                else if (edgeToDelete.NextEdge == null && edgeToDeletePrev != null)
                {
                    edgeToDeletePrev.NextEdge = edgeToDelete.NextEdge;
                    edgeToDelete = null;
                }
                else
                {
                    edgeCollectionToDecrease = null;
                    edgeToDelete = null;
                }

                if (isDeletingRecipients == true)
                    examinedEdge.Node.EdgeToRecipient = edgeCollectionToDecrease;
                else
                    examinedEdge.Node.EdgeFromSenders = edgeCollectionToDecrease;
                examinedEdge = examinedEdge.NextEdge;
            }

        }
        public void RemoveEdge(WirthNode<T> node1, WirthNode<T> node2)
        {
            if (FindEdgeBetweenNodes(node1, node2) == null && FindEdgeBetweenNodes(node2, node1) == null)
            {
                throw new InvalidOperationException("There's no edge between from node1 to node2");
            }
            if (CheckNodeBelonging(node1) == false || CheckNodeBelonging(node2) == false)
                throw new InvalidOperationException("Node does not belong to the graph");

            WirthEdge<T> eximinedEdge = node1.EdgeToRecipient;
            RemovingEdge(ref eximinedEdge, node2);
            node1.EdgeToRecipient = eximinedEdge;

            eximinedEdge = node2.EdgeFromSenders;
            RemovingEdge(ref eximinedEdge, node1);
            node2.EdgeFromSenders = eximinedEdge;

            node1.AmountOfRecipients--;
            node2.AmountOfSenders--;

            
        }

        private void RemovingEdge(ref WirthEdge<T> edgeCollectionToDelete, WirthNode<T> destinationNode)
        {
            WirthEdge<T> edgeToDelete = edgeCollectionToDelete;
            WirthEdge<T> edgeToDeletePrev = null;
            if (edgeToDelete == null)
                return;
            while (edgeToDelete.Node != destinationNode)
            {
                edgeToDeletePrev = edgeToDelete;
                edgeToDelete = edgeToDelete.NextEdge;
            }
            if (edgeToDelete.NextEdge != null && edgeToDeletePrev != null)
            {
                edgeToDeletePrev.NextEdge = edgeToDelete.NextEdge;
                edgeToDelete = null;
            }
            else if (edgeToDelete.NextEdge != null && edgeToDeletePrev == null)
            {
                edgeCollectionToDelete = edgeToDelete.NextEdge;
                edgeToDelete = null;
            }
            else if (edgeToDelete.NextEdge == null && edgeToDeletePrev != null)
            {
                edgeToDeletePrev.NextEdge = edgeToDelete.NextEdge;
                edgeToDelete = null;
            }
            else
            {
                edgeCollectionToDelete = null;
            }
        }
        public bool isEmpty()
        {
            if (headNode == tailNode)
                return true;
            else
                return false;
        }

        public void Clear()
        {
            if (headNode != tailNode)
            {
                headNode.Clear();
                headNode = tailNode;
            }
        }

        public bool CheckNodeBelonging(WirthNode<T> node)
        {
            WirthNode<T> eximinedNode = headNode;
            while (eximinedNode != tailNode && eximinedNode != node)
                eximinedNode = eximinedNode.NextWirthNode;

            return eximinedNode == node;
        }
        public IEnumerable<WirthNode<T>> GetVertexes(bool isItReverse, bool isItConst)
        {
            GraphNodeIterator<T> iterator = GetVertexIterator(isItReverse, isItConst);
            while (iterator.MoveNext())
                yield return iterator.Current;
            iterator.Dispose();
            yield break; 
        }

        public IEnumerable<WirthEdge<T>> GetEdges(bool isItReverse, bool isItConst)
        {
            GraphEdgeIterator<T> iterator = GetEdgeIterator(isItReverse, isItConst);
            while (iterator.MoveNext())
                yield return iterator.Current;
            iterator.Dispose();
            yield break;
        }

        public GraphNodeIterator<T> GetVertexIterator(bool isItReverse, bool isItConst)
        {
            return new GraphNodeIterator<T>(this, headNode, tailNode, isItReverse, isItConst);
        }
        public GraphEdgeIterator<T> GetEdgeIterator(bool isItReverse, bool isItConst)
        {
            return new GraphEdgeIterator<T>(this, headNode, tailNode, isItReverse, isItConst);
        }

        public static bool operator ==(Graph<T> graph1, Graph<T> graph2)
        {
            var graphNodeIter1 = graph1.GetVertexIterator(false, false);
            var graphNodeIter2 = graph2.GetVertexIterator(false, false);
            while(graphNodeIter1.MoveNext())
            {
                graphNodeIter2.MoveNext();
                if (object.Equals(graphNodeIter1.Current.Content, graphNodeIter2.Current.Content) == false)
                    return false;
            }

            return true;
        }
        public static bool operator !=(Graph<T> graph1, Graph<T> graph2) 
        {
            var graphNodeIter1 = graph1.GetVertexIterator(false, false);
            var graphNodeIter2 = graph2.GetVertexIterator(false, false);
            while (graphNodeIter1.MoveNext())
            {
                graphNodeIter2.MoveNext();
                if (object.Equals(graphNodeIter1.Current.Content, graphNodeIter2.Current.Content) == false)
                    return true;
            }

            return false;
        }

        public static bool operator >=(Graph<T> graph1, Graph<T> graph2) 
        {
            int graphDegree1 = graph1.GetAmountOfNodes() + graph1.GetAmountOfEdges();
            int graphDegree2 = graph2.GetAmountOfNodes() + graph2.GetAmountOfEdges();

            return graphDegree1 >= graphDegree2;
        }

        public static bool operator <=(Graph<T> graph1, Graph<T> graph2) 
        {
            int graphDegree1 = graph1.GetAmountOfNodes() + graph1.GetAmountOfEdges();
            int graphDegree2 = graph2.GetAmountOfNodes() + graph2.GetAmountOfEdges();

            return graphDegree1 <= graphDegree2;
        }

    }
}
