using GraphContainer;

namespace GraphContainerTests
{
    [TestClass]
    public class GraphEdgeIteratorTests
    {
        Graph<int> graph;
        GraphEdgeIterator<int> iterator;
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
            iterator = graph.GetEdgeIterator(false, false);
            
        }

        [TestCleanup]
        public void TestCleanup()
        {
            graph.Clear();
            iterator.Dispose();
        }

        [TestMethod]
        public void MoveNext_Test()
        {
            Assert.IsNull(iterator.Current);
            iterator.MoveNext();
            Assert.IsNotNull(iterator.Current);
            iterator.MoveNext();
            Assert.AreEqual(2, iterator.Current.Node.Content);
            Assert.IsTrue(iterator.MoveNext());
            Assert.IsFalse(iterator.MoveNext());
            Assert.IsNull(iterator.Current);
        }

        [TestMethod]
        public void MovePrevious_Test()
        {
            Assert.IsNull(iterator.Current);
            iterator.MovePrevious();
            Assert.IsNotNull(iterator.Current);
            iterator.MovePrevious();
            Assert.AreEqual(2, iterator.Current.Node.Content);
            Assert.IsTrue(iterator.MovePrevious());
            Assert.IsFalse(iterator.MovePrevious());
            Assert.IsNull(iterator.Current);
        }

        [TestMethod]
        public void DeleteCurrent_Test()
        {
            iterator.MoveNext();
            WirthEdge<int> edge = iterator.Current;
            iterator.DeleteCurrent();
            Assert.IsNull(graph.FindEdgeBetweenNodes(graph.HeadNode,edge.Node));
        }
    }
}
