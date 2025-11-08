using System;
using System.Collections;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    /// <summary>
    /// Custom Min Heap implementation for priority-based service request management
    /// Used for efficient priority queuing of service requests
    /// </summary>
    public class MinHeap<T> : IEnumerable<T> where T : IComparable<T>
    {
        private List<T> heap;
        private int capacity;

        public int Count => heap.Count;
        public bool IsEmpty => heap.Count == 0;

        public MinHeap(int capacity = 10)
        {
            this.capacity = capacity;
            heap = new List<T>(capacity);
        }

        public void Insert(T item)
        {
            heap.Add(item);
            HeapifyUp(heap.Count - 1);
        }

        public T ExtractMin()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Heap is empty");

            T min = heap[0];
            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            HeapifyDown(0);
            return min;
        }

        public T PeekMin()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Heap is empty");

            return heap[0];
        }

        public bool Contains(T item)
        {
            return heap.Contains(item);
        }

        public void Clear()
        {
            heap.Clear();
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                if (heap[parentIndex].CompareTo(heap[index]) <= 0)
                    break;

                Swap(parentIndex, index);
                index = parentIndex;
            }
        }

        private void HeapifyDown(int index)
        {
            int lastIndex = heap.Count - 1;
            while (index < lastIndex)
            {
                int leftChild = 2 * index + 1;
                int rightChild = 2 * index + 2;
                int smallest = index;

                if (leftChild <= lastIndex && heap[leftChild].CompareTo(heap[smallest]) < 0)
                    smallest = leftChild;

                if (rightChild <= lastIndex && heap[rightChild].CompareTo(heap[smallest]) < 0)
                    smallest = rightChild;

                if (smallest != index)
                {
                    Swap(index, smallest);
                    index = smallest;
                }
                else
                {
                    break;
                }
            }
        }

        private void Swap(int i, int j)
        {
            T temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return heap.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}