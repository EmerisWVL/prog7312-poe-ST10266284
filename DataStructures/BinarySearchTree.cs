using System;
using System.Collections;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    /// <summary>
    /// Custom Binary Search Tree implementation for efficient service request searching
    /// </summary>
    public class BinarySearchTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        private class BSTNode
        {
            public T Data { get; set; }
            public BSTNode Left { get; set; }
            public BSTNode Right { get; set; }

            public BSTNode(T data)
            {
                Data = data;
                Left = null;
                Right = null;
            }
        }

        private BSTNode root;
        private int count;

        public int Count => count;
        public bool IsEmpty => root == null;

        public void Insert(T data)
        {
            root = InsertRec(root, data);
            count++;
        }

        private BSTNode InsertRec(BSTNode node, T data)
        {
            if (node == null)
            {
                return new BSTNode(data);
            }

            int comparison = data.CompareTo(node.Data);
            if (comparison < 0)
            {
                node.Left = InsertRec(node.Left, data);
            }
            else if (comparison > 0)
            {
                node.Right = InsertRec(node.Right, data);
            }

            return node;
        }

        public bool Contains(T data)
        {
            return ContainsRec(root, data);
        }

        private bool ContainsRec(BSTNode node, T data)
        {
            if (node == null) return false;

            int comparison = data.CompareTo(node.Data);
            if (comparison == 0) return true;

            return comparison < 0 ? ContainsRec(node.Left, data) : ContainsRec(node.Right, data);
        }

        public List<T> InOrderTraversal()
        {
            List<T> result = new List<T>();
            InOrderRec(root, result);
            return result;
        }

        private void InOrderRec(BSTNode node, List<T> result)
        {
            if (node != null)
            {
                InOrderRec(node.Left, result);
                result.Add(node.Data);
                InOrderRec(node.Right, result);
            }
        }

        public List<T> PreOrderTraversal()
        {
            List<T> result = new List<T>();
            PreOrderRec(root, result);
            return result;
        }

        private void PreOrderRec(BSTNode node, List<T> result)
        {
            if (node != null)
            {
                result.Add(node.Data);
                PreOrderRec(node.Left, result);
                PreOrderRec(node.Right, result);
            }
        }

        public List<T> PostOrderTraversal()
        {
            List<T> result = new List<T>();
            PostOrderRec(root, result);
            return result;
        }

        private void PostOrderRec(BSTNode node, List<T> result)
        {
            if (node != null)
            {
                PostOrderRec(node.Left, result);
                PostOrderRec(node.Right, result);
                result.Add(node.Data);
            }
        }

        public T FindMin()
        {
            if (IsEmpty) throw new InvalidOperationException("Tree is empty");
            return FindMinRec(root).Data;
        }

        private BSTNode FindMinRec(BSTNode node)
        {
            while (node.Left != null) node = node.Left;
            return node;
        }

        public T FindMax()
        {
            if (IsEmpty) throw new InvalidOperationException("Tree is empty");
            return FindMaxRec(root).Data;
        }

        private BSTNode FindMaxRec(BSTNode node)
        {
            while (node.Right != null) node = node.Right;
            return node;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}