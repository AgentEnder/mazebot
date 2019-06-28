using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver
{
    class BinaryHeap<T> where T : IComparable<T>
    {
        Dictionary<int, T> heap;
        int size;

        public BinaryHeap(){
            heap = new Dictionary<int, T>();
            size = 0;
        }

        int parent(int i) { return (i - 1) / 2; }

        // to get index of left child of node at index i 
        int left(int i) { return (2 * i + 1); }

        // to get index of right child of node at index i 
        int right(int i) { return (2 * i + 2); }

        public void insert(T item)
        {
            int i = size;
            size++;
            heap[i] = item;
            while (i!=0 && heap[parent(i)].CompareTo(heap[i]) == 1)
            {
                Swap(i, parent(i));
                i = parent(i);
            }
        }

        public T extractMin()
        {
            if (size <= 0)
            {
                throw new Exception("Empty Heap");
            }
            if (size == 1)
            {
                size--;
                return heap[0];
            }

            T root = heap[0];
            heap[0] = heap[size-1];
            size--;
            MinHeapify(0);
            return root;
        }

        void MinHeapify(int i)
        {
            int l = left(i);
            int r = right(i);
            int smallest = i;

            if (l < size && heap[l].CompareTo(heap[smallest]) == -1)
            {
                smallest = l;
            }
            if (r < size && heap[r].CompareTo(heap[smallest]) == -1)
            {
                smallest = r;
            }
            if (smallest != i)
            {
                Swap(i, smallest);
                MinHeapify(smallest);
            }
        }

        void Swap(int a, int b)
        {
            T temp = heap[a];
            heap[a] = heap[b];
            heap[b] = temp;
        }
    }
}
