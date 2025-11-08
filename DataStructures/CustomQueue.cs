using System;
using System.Collections;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    /// <summary>
    /// Custom queue implementation from scratch (FIFO)
    /// Used for event registration and request processing
    /// </summary>
    public class CustomQueue<T> : IEnumerable<T>
    {
        private class QueueNode
        {
            public T Data { get; set; }
            public QueueNode Next { get; set; }

            public QueueNode(T data)
            {
                Data = data;
                Next = null;
            }
        }

        private QueueNode front;
        private QueueNode rear;
        private int count;

        public int Count => count;
        public bool IsEmpty => front == null;

        public void Enqueue(T item)
        {
            QueueNode newNode = new QueueNode(item);

            if (rear == null)
            {
                front = rear = newNode;
            }
            else
            {
                rear.Next = newNode;
                rear = newNode;
            }
            count++;
        }

        public T Dequeue()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Queue is empty");

            T data = front.Data;
            front = front.Next;

            if (front == null)
                rear = null;

            count--;
            return data;
        }

        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Queue is empty");

            return front.Data;
        }

        public void Clear()
        {
            front = rear = null;
            count = 0;
        }

        public bool Contains(T item)
        {
            QueueNode current = front;
            while (current != null)
            {
                if (EqualityComparer<T>.Default.Equals(current.Data, item))
                    return true;
                current = current.Next;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            QueueNode current = front;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}