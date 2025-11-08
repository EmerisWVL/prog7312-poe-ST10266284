using System;
using System.Collections;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    /// <summary>
    /// Custom stack implementation from scratch (LIFO)
    /// Used for managing event history and undo operations
    /// </summary>
    public class CustomStack<T> : IEnumerable<T>
    {
        private class StackNode
        {
            public T Data { get; set; }
            public StackNode Next { get; set; }

            public StackNode(T data)
            {
                Data = data;
                Next = null;
            }
        }

        private StackNode top;
        private int count;

        public int Count => count;
        public bool IsEmpty => top == null;

        public void Push(T item)
        {
            StackNode newNode = new StackNode(item);
            newNode.Next = top;
            top = newNode;
            count++;
        }

        public T Pop()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Stack is empty");

            T data = top.Data;
            top = top.Next;
            count--;
            return data;
        }

        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Stack is empty");

            return top.Data;
        }

        public void Clear()
        {
            top = null;
            count = 0;
        }

        public bool Contains(T item)
        {
            StackNode current = top;
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
            StackNode current = top;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}