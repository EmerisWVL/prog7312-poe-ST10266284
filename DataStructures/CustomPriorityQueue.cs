using System;
using System.Collections;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    /// <summary>
    /// Custom priority queue implementation using min-heap
    /// Used for event prioritization and service request management
    /// </summary>
    public class CustomPriorityQueue<T> : IEnumerable<T> where T : IComparable<T>
    {
        private List<T> heap;
        private bool isMinHeap;

        public int Count => heap.Count;
        public bool IsEmpty => heap.Count == 0;

        public CustomPriorityQueue(bool minHeap = true)
        {
            heap = new List<T>();
            isMinHeap = minHeap;
        }

        public void Enqueue(T item)
        {
            heap.Add(item);
            int currentIndex = heap.Count - 1;
            HeapifyUp(currentIndex);
        }

        public T Dequeue()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Priority queue is empty");

            T item = heap[0];
            int lastIndex = heap.Count - 1;
            heap[0] = heap[lastIndex];
            heap.RemoveAt(lastIndex);

            if (!IsEmpty)
                HeapifyDown(0);

            return item;
        }

        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Priority queue is empty");

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
                if (ShouldSwap(parentIndex, index))
                {
                    Swap(parentIndex, index);
                    index = parentIndex;
                }
                else
                {
                    break;
                }
            }
        }

        private void HeapifyDown(int index)
        {
            int lastIndex = heap.Count - 1;
            while (index < lastIndex)
            {
                int leftChild = 2 * index + 1;
                int rightChild = 2 * index + 2;
                int targetIndex = index;

                if (leftChild <= lastIndex && ShouldSwap(targetIndex, leftChild))
                    targetIndex = leftChild;

                if (rightChild <= lastIndex && ShouldSwap(targetIndex, rightChild))
                    targetIndex = rightChild;

                if (targetIndex != index)
                {
                    Swap(index, targetIndex);
                    index = targetIndex;
                }
                else
                {
                    break;
                }
            }
        }

        private bool ShouldSwap(int parent, int child)
        {
            int comparison = heap[parent].CompareTo(heap[child]);
            return isMinHeap ? comparison > 0 : comparison < 0;
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