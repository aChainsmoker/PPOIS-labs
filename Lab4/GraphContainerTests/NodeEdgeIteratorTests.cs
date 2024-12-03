using GraphContainer;

namespace GraphContainerTests
{
    [TestClass]
    public class NodeEdgeIteratorTests
    {
        Graph<int> graph;
        NodeEdgeIterator<int> iterator;
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
            iterator = n1.GetNodeEdgesIterator(graph, false, false);

        }

        [TestCleanup]
        public void TestCleanup()
        {
            graph.Clear();
        }

        [TestMethod]
        public void MoveNext_Test()
        {
            Assert.IsNull(iterator.Current);
            iterator.MoveNext();
            Assert.IsNotNull(iterator.Current);
            Assert.IsTrue(iterator.MoveNext());
            Assert.AreEqual(2, iterator.Current.Node.Content);
            Assert.IsFalse(iterator.MoveNext());
        }
        [TestMethod]
        public void MovePrevious_Test()
        {
            Assert.IsNull(iterator.Current);
            iterator.MovePrevious();
            Assert.IsNotNull(iterator.Current);
            Assert.IsTrue(iterator.MovePrevious());
            Assert.AreEqual(4, iterator.Current.Node.Content);
            Assert.IsFalse(iterator.MovePrevious());
        }

        [TestMethod]
        public void DeleteCurrent_Test()
        {
            iterator.MoveNext();
            WirthEdge<int> edge = iterator.Current;
            iterator.DeleteCurrent();
            Assert.IsNull(graph.FindEdgeBetweenNodes(graph.HeadNode, edge.Node));
        }
    }
}
