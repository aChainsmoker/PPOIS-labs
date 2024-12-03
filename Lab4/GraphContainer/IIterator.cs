using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphContainer
{
    public interface IIterator<T>
    {
        bool MoveNext();
        bool MovePrevious();
        void Reset();


        T Current { get; }

    }
}
