using System;
using System.Collections;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    /// <summary>
    /// Custom set implementation from scratch
    /// Used for managing unique categories and tags
    /// </summary>
    public class CustomSet<T> : IEnumerable<T>
    {
        private class SetNode
        {
            public T Data { get; set; }
            public SetNode Next { get; set; }

            public SetNode(T data)
            {
                Data = data;
                Next = null;
            }
        }

        private SetNode head;
        private int count;

        public int Count => count;
        public bool IsEmpty => head == null;

        public bool Add(T item)
        {
            if (Contains(item))
                return false;

            SetNode newNode = new SetNode(item);
            newNode.Next = head;
            head = newNode;
            count++;
            return true;
        }

        public bool Remove(T item)
        {
            if (IsEmpty)
                return false;

            if (EqualityComparer<T>.Default.Equals(head.Data, item))
            {
                head = head.Next;
                count--;
                return true;
            }

            SetNode current = head;
            while (current.Next != null)
            {
                if (EqualityComparer<T>.Default.Equals(current.Next.Data, item))
                {
                    current.Next = current.Next.Next;
                    count--;
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        public bool Contains(T item)
        {
            SetNode current = head;
            while (current != null)
            {
                if (EqualityComparer<T>.Default.Equals(current.Data, item))
                    return true;
                current = current.Next;
            }
            return false;
        }

        public void Clear()
        {
            head = null;
            count = 0;
        }

        public CustomSet<T> Union(CustomSet<T> other)
        {
            CustomSet<T> result = new CustomSet<T>();

            // Add all items from current set
            foreach (T item in this)
                result.Add(item);

            // Add all items from other set
            foreach (T item in other)
                result.Add(item);

            return result;
        }

        public CustomSet<T> Intersection(CustomSet<T> other)
        {
            CustomSet<T> result = new CustomSet<T>();

            foreach (T item in this)
            {
                if (other.Contains(item))
                    result.Add(item);
            }

            return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            SetNode current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}