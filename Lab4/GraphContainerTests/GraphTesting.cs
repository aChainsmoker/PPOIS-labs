using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraphContainer;
using System;
using System.Xml.Linq;

namespace GraphContainerTests
{
    [TestClass]
    public class GraphTests
    {
        private Graph<int> graph;

        [TestInitialize]
        public void TestInitialize()
        {
            graph = new Graph<int>();
            WirthNode<int> n1 = graph.AddNode(1);
            WirthNode<int> n2 = graph.AddNode(2);
            WirthNode<int> n3 = graph.AddNode(3);
            WirthNode<int> n4 = graph.AddNode(4);
            graph.AddEdge(n1, n4);
            graph.AddEdge(n3, n2);
            graph.AddEdge(n1, n2);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            graph.Clear();
        }

        [TestMethod]
        public void AddNode_Testt()
        {
            int initialCount = graph.GetAmountOfNodes();
            graph.AddNode(0);
            Assert.AreEqual(initialCount + 1, graph.GetAmountOfNodes());
        }

        [TestMethod]
        public void AddEdge_Test()
        {
            var node1 = graph.FindNode(1);
            var node2 = graph.FindNode(2);
            int initialEdgeCount = graph.GetAmountOfEdges();
            graph.AddEdge(node1, node2);
            Assert.AreEqual(initialEdgeCount + 1, graph.GetAmountOfEdges());
        }

        [TestMethod]
        public void FindNode_Test()
        {
            var foundNode = graph.FindNode(1);
            Assert.IsNotNull(foundNode);
        }

        [TestMethod]
        public void FindEdge_Test()
        {
            var node1 = graph.FindNode(1);
            var node2 = graph.FindNode(3);
            var edge = graph.AddEdge(node1, node2);
            var foundEdge = graph.FindEdgeBetweenNodes(node1, node2);
            Assert.IsNotNull(foundEdge);
            Assert.AreEqual(edge, foundEdge);
        }

        [TestMethod]
        public void CheckNodeBelonging_Test()
        {
            var node = graph.FindNode(1);
            bool belongs = graph.CheckNodeBelonging(node);
            Assert.IsTrue(belongs);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddEdgeException_Test()
        {
            var graph1 = new Graph<int>();
            var graph2 = new Graph<int>();
            var node1 = graph1.AddNode(1);
            var node2 = graph2.AddNode(2);
            graph1.AddEdge(node1, node2);
        }

        [TestMethod]
        public void IsEmpty_Test()
        {
            Assert.IsFalse(graph.isEmpty());
            graph.Clear();
            Assert.IsTrue(graph.isEmpty());
        }

        [TestMethod]
        public void Clear_Test()
        {
            graph.Clear();
            Assert.AreEqual(0, graph.GetAmountOfNodes());
            Assert.IsTrue(graph.isEmpty());
        }

        [TestMethod]
        public void DeleteNode_Test()
        {
            WirthNode<int> n1 = graph.FindNode(1);
            WirthNode<int> n2 = graph.FindNode(2);
            WirthNode<int> n3 = graph.FindNode(3);
            WirthNode<int> n4 = graph.FindNode(4);
            WirthNode<int> n5 = graph.AddNode(5);
            graph.RemoveNode(n2);
            Assert.IsFalse(graph.CheckNodeBelonging(n2));
            graph.RemoveNode(n5);
            Assert.IsFalse(graph.CheckNodeBelonging(n5));
            graph.RemoveNode(n3);
            Assert.IsFalse(graph.CheckNodeBelonging(n3));
            graph.RemoveNode(n1);
            Assert.IsFalse(graph.CheckNodeBelonging(n1));
            graph.RemoveNode(n4);
            Assert.IsFalse(graph.CheckNodeBelonging(n4));
        }

        [TestMethod]
        public void DeleteEdge_Test()
        {
            WirthNode<int> n1 = graph.FindNode(1);
            WirthNode<int> n2 = graph.FindNode(2);
            WirthNode<int> n3 = graph.FindNode(3);
            WirthNode<int> n4 = graph.FindNode(4);
            graph.RemoveEdge(n3, n2);
            Assert.IsNull(graph.FindEdgeBetweenNodes(n3, n2));
            graph.RemoveEdge(n1, n2);
            Assert.IsNull(graph.FindEdgeBetweenNodes(n1, n2));
            graph.RemoveEdge(n1, n4);
            Assert.IsNull(graph.FindEdgeBetweenNodes(n1, n4));
        }

        [TestMethod]
        public void Operator_Test()
        {
            Graph<int> graph1 = new Graph<int>(graph);
            Assert.IsTrue(graph1 == graph);
            graph1.RemoveNode(graph1.HeadNode);
            Assert.IsTrue(graph1 != graph);
            Assert.IsTrue(graph1 <= graph);
            Assert.IsTrue(graph >= graph1);
        }

        [TestMethod]
        public void EdgeDegree_Test()
        {
            WirthNode<int> n1 = graph.FindNode(1);
            WirthNode<int> n2 = graph.FindNode(2);
            WirthNode<int> n3 = graph.FindNode(3);
            WirthNode<int> n4 = graph.FindNode(4);
            WirthEdge<int> edge = graph.FindEdgeBetweenNodes(n3, n2);
            Assert.AreEqual(2, graph.GetEdgeDegree(edge));
        }

        [TestMethod]
        public void NodeDegree_Test()
        {
            WirthNode<int> n1 = graph.FindNode(1);
            WirthNode<int> n2 = graph.FindNode(2);
            WirthNode<int> n3 = graph.FindNode(3);
            WirthNode<int> n4 = graph.FindNode(4);
            Assert.AreEqual(2, graph.GetVertexDegree(n1));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RepeatingContent_Test()
        {
            var node1 = graph.AddNode(1);
        }

        [TestMethod]
        public void ForEach_Test()
        {
            WirthNode<int> n1 = graph.FindNode(1);
            GraphEdgeIterator<int> graphEdgeIter = graph.GetEdgeIterator(false, false);
            GraphNodeIterator<int> graphNodeIter = graph.GetVertexIterator(false, false);
            NodeAdjacentNodesIterator<int> nodeNodeIter = n1.GetNodeAdjacentNodesIterator(graph, false, false);
            NodeEdgeIterator<int> nodeEdgeIter = n1.GetNodeEdgesIterator(graph, false, false);
            foreach (var node in graph.GetVertexes(false, true))
            {
                graphNodeIter.MoveNext();
                Assert.AreEqual(graphNodeIter.Current, node);

            }
            foreach (var edge in graph.GetEdges(false, false))
            {
                graphEdgeIter.MoveNext();
                Assert.AreEqual(graphEdgeIter.Current, edge);
            }
            foreach (var edge in n1.GetEdges(graph, false, false))
            {
                nodeEdgeIter.MoveNext();
                Assert.AreEqual(nodeEdgeIter.Current, edge);
            }
            Console.WriteLine();
            foreach (var node in n1.GetAdjacentNodes(graph, false, false))
            {
                nodeNodeIter.MoveNext();
                Assert.AreEqual(nodeNodeIter.Current, node);
            }
        }
    }
}
