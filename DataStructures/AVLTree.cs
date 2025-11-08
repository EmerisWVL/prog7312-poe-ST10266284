using System;
using System.Collections;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    /// <summary>
    /// Custom AVL Tree implementation - self-balancing binary search tree
    /// Used for efficient service request organization and retrieval
    /// </summary>
    public class AVLTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        private class AVLNode
        {
            public T Data { get; set; }
            public AVLNode Left { get; set; }
            public AVLNode Right { get; set; }
            public int Height { get; set; }

            public AVLNode(T data)
            {
                Data = data;
                Height = 1;
            }
        }

        private AVLNode root;
        private int count;

        public int Count => count;
        public bool IsEmpty => root == null;

        public void Insert(T data)
        {
            root = InsertRec(root, data);
            count++;
        }

        private AVLNode InsertRec(AVLNode node, T data)
        {
            if (node == null)
                return new AVLNode(data);

            int comparison = data.CompareTo(node.Data);
            if (comparison < 0)
                node.Left = InsertRec(node.Left, data);
            else if (comparison > 0)
                node.Right = InsertRec(node.Right, data);
            else
                return node; // Duplicate values not allowed

            // Update height
            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            // Get balance factor
            int balance = GetBalance(node);

            // Left Left Case
            if (balance > 1 && data.CompareTo(node.Left.Data) < 0)
                return RightRotate(node);

            // Right Right Case
            if (balance < -1 && data.CompareTo(node.Right.Data) > 0)
                return LeftRotate(node);

            // Left Right Case
            if (balance > 1 && data.CompareTo(node.Left.Data) > 0)
            {
                node.Left = LeftRotate(node.Left);
                return RightRotate(node);
            }

            // Right Left Case
            if (balance < -1 && data.CompareTo(node.Right.Data) < 0)
            {
                node.Right = RightRotate(node.Right);
                return LeftRotate(node);
            }

            return node;
        }

        private int GetHeight(AVLNode node)
        {
            return node?.Height ?? 0;
        }

        private int GetBalance(AVLNode node)
        {
            if (node == null) return 0;
            return GetHeight(node.Left) - GetHeight(node.Right);
        }

        private AVLNode RightRotate(AVLNode y)
        {
            AVLNode x = y.Left;
            AVLNode T2 = x.Right;

            // Perform rotation
            x.Right = y;
            y.Left = T2;

            // Update heights
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

            return x;
        }

        private AVLNode LeftRotate(AVLNode x)
        {
            AVLNode y = x.Right;
            AVLNode T2 = y.Left;

            // Perform rotation
            y.Left = x;
            x.Right = T2;

            // Update heights
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

            return y;
        }

        public bool Contains(T data)
        {
            return ContainsRec(root, data);
        }

        private bool ContainsRec(AVLNode node, T data)
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

        private void InOrderRec(AVLNode node, List<T> result)
        {
            if (node != null)
            {
                InOrderRec(node.Left, result);
                result.Add(node.Data);
                InOrderRec(node.Right, result);
            }
        }

        public int GetTreeHeight()
        {
            return GetHeight(root);
        }

        public bool IsBalanced()
        {
            return CheckBalance(root);
        }

        private bool CheckBalance(AVLNode node)
        {
            if (node == null) return true;

            int balance = GetBalance(node);
            return Math.Abs(balance) <= 1 && CheckBalance(node.Left) && CheckBalance(node.Right);
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