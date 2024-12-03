using GraphContainer;

namespace GraphContainerTests
{
    [TestClass]
    public class GraphNodeIteratorTests
    {
        Graph<int> graph;
        GraphNodeIterator<int> iterator;
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
            iterator = graph.GetVertexIterator(false, false);

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
            Assert.AreEqual(2, iterator.Current.Content);
            Assert.IsTrue(iterator.MoveNext());
            iterator.MoveNext();
            Assert.IsFalse(iterator.MoveNext());
            Assert.AreEqual(graph.TailNode, iterator.Current);
            iterator.MoveNext();
            Assert.IsNull(iterator.Current);
        }

        [TestMethod]
        public void MovePrevious_Test()
        {
            Assert.IsNull(iterator.Current);
            iterator.MovePrevious();
            Assert.IsNotNull(iterator.Current);
            iterator.MovePrevious();
            Assert.AreEqual(3, iterator.Current.Content);
            Assert.IsTrue(iterator.MovePrevious());
            iterator.MovePrevious();
            Assert.IsFalse(iterator.MovePrevious());
            Assert.IsNull(iterator.Current);
        }

        [TestMethod]
        public void DeleteCurrent_Test()
        {
            iterator.MoveNext();
            iterator.MoveNext();
            WirthNode<int> node = iterator.Current;
            iterator.DeleteCurrent();
            Assert.IsFalse(graph.CheckNodeBelonging(node));
        }
    }
}
