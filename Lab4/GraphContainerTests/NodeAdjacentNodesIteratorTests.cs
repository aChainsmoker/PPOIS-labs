using GraphContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphContainerTests
{
    [TestClass]
    public class NodeAdjacentNodesIteratorTests
    {
        Graph<int> graph;
        NodeAdjacentNodesIterator<int> iterator;

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
            iterator = n1.GetNodeAdjacentNodesIterator(graph, false, false);

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
            Assert.AreEqual(2, iterator.Current.Content);
            Assert.IsFalse(iterator.MoveNext());
        }
        [TestMethod]
        public void MovePrevious_Test()
        {
            Assert.IsNull(iterator.Current);
            iterator.MovePrevious();
            Assert.IsNotNull(iterator.Current);
            Assert.IsTrue(iterator.MovePrevious());
            Assert.AreEqual(4, iterator.Current.Content);
            Assert.IsFalse(iterator.MovePrevious());
        }

        [TestMethod]
        public void DeleteCurrent_Test()
        {
            iterator.MoveNext();
            WirthNode<int> node = iterator.Current;
            iterator.DeleteCurrent();
            Assert.IsFalse(graph.CheckNodeBelonging(node));
        }
    }
}
