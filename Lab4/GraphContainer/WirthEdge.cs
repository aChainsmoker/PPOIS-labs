using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphContainer
{
    public class WirthEdge<T>
    {
        private WirthNode<T> node;
        private WirthEdge<T> nextEdge;

        public WirthNode<T> Node { get { return node; } set { node = value; } }
        public WirthEdge<T> NextEdge { get { return nextEdge; } set { nextEdge = value; } }

        public void Clear()
        {
            if (nextEdge != null)
            {
                nextEdge.Clear();
                nextEdge = null;
            }
        }
    }
}
