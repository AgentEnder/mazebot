using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver
{
    interface IPriorityQueue<T> where T:IComparable<T>
    {
        T ExtractMin();
        void Insert(T item);
        void Clear();
    }
}
