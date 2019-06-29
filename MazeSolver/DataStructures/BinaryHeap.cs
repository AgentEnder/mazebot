using System;
using System.Collections.Generic;

namespace MazeSolver
{
    class BinaryHeap<T> where T : IComparable<T>
    {
        /// This Class uses a dictionary because list[0] would throw an error given
        /// that the list did not yet contain any objects. This is normally wanted, but
        /// we wanted to allow for a growing container and swapping based on index. A 
        /// dictionary is weird for this, but it works.
        Dictionary<int, T> heap;
        int size; //Number of elements in heap

        public BinaryHeap(){
            heap = new Dictionary<int, T>();
            size = 0;
        }

        //get index of the parent of node at index i
        int parent(int i) { return (i - 1) / 2; }

        // to get index of left child of node at index i 
        int left(int i) { return (2 * i + 1); }

        // to get index of right child of node at index i 
        int right(int i) { return (2 * i + 2); }

        //Add an item to the queue and maintain heap
        public void Insert(T item)
        {
            int i = size;
            size++;
            heap[i] = item;
            while (i!=0 && heap[parent(i)].CompareTo(heap[i]) == 1) //heap[i] < heap[parent(i)]
            {
                swap(i, parent(i));
                i = parent(i);
            }
        }

        public T ExtractMin()
        {
            if (size <= 0)
            {
                throw new Exception("Empty Heap"); //Tried to get element from empty heap
            }
            if (size == 1)
            {
                size--;
                return heap[0]; //Get the min element
            }

            T root = heap[0]; //Store the value
            heap[0] = heap[size-1]; //Delete its index
            size--;
            minHeapify(0); //Maintain heap
            return root; //return value
        }

        void minHeapify(int i)
        {
            int l = left(i); //index of the left child of element at index i
            int r = right(i); //index of the right child of element at index i
            int smallest = i; //index of the smallest element

            if (l < size && heap[l].CompareTo(heap[smallest]) == -1) //heap[l] is smaller
            {
                smallest = l;
            }
            if (r < size && heap[r].CompareTo(heap[smallest]) == -1) //heap[r] is smaller
            {
                smallest = r;
            }
            if (smallest != i) //Heap broke
            {
                swap(i, smallest); //Make a change
                minHeapify(smallest); //See if still broke
            }
        }

        void swap(int a, int b)
        {
            T temp = heap[a];
            heap[a] = heap[b];
            heap[b] = temp;
        }
    }
}
