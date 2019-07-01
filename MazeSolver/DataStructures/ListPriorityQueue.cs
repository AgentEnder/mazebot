using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver
{
    class ListPriorityQueue<T>:IPriorityQueue<T> where T:IComparable<T>
    {
        List<T> data;

        public ListPriorityQueue()
        {
            data = new List<T>();
        }

        public void Clear()
        {
            data.Clear();
        }

        public T ExtractMin()
        {
            if (data.Count == 0)
            {
                throw new IndexOutOfRangeException("Empty Priority Queue");
            }
            data.Sort((x, y) => x.CompareTo(y));
            T ret = data[0];
            data.RemoveAt(0);
            return ret;
        }

        public void Insert(T item)
        {
            data.Add(item);
        }
    }
}
