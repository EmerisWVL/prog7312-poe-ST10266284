using System;
using System.Collections;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    /// <summary>
    /// Custom Red-Black Tree implementation - self-balancing binary search tree
    /// Used for efficient service request organization with guaranteed O(log n) operations
    /// </summary>
    public class RedBlackTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        private enum NodeColor { Red, Black }

        private class RBNode
        {
            public T Data { get; set; }
            public RBNode Left { get; set; }
            public RBNode Right { get; set; }
            public RBNode Parent { get; set; }
            public NodeColor Color { get; set; }

            public RBNode(T data)
            {
                Data = data;
                Color = NodeColor.Red; // New nodes are always red
                Left = Right = Parent = null;
            }

            public RBNode Uncle()
            {
                if (Parent == null || Parent.Parent == null)
                    return null;

                if (Parent.IsLeftChild())
                    return Parent.Parent.Right;
                else
                    return Parent.Parent.Left;
            }

            public bool IsLeftChild()
            {
                return this == Parent?.Left;
            }

            public RBNode Sibling()
            {
                if (Parent == null)
                    return null;

                return IsLeftChild() ? Parent.Right : Parent.Left;
            }

            public void MoveDown(RBNode newParent)
            {
                if (Parent != null)
                {
                    if (IsLeftChild())
                        Parent.Left = newParent;
                    else
                        Parent.Right = newParent;
                }
                newParent.Parent = Parent;
                Parent = newParent;
            }

            public bool HasRedChild()
            {
                return (Left != null && Left.Color == NodeColor.Red) ||
                       (Right != null && Right.Color == NodeColor.Red);
            }
        }

        private RBNode root;
        private int count;

        public int Count => count;
        public bool IsEmpty => root == null;

        public void Insert(T data)
        {
            RBNode newNode = new RBNode(data);
            if (root == null)
            {
                // First node becomes black
                newNode.Color = NodeColor.Black;
                root = newNode;
            }
            else
            {
                RBNode temp = root;
                while (true)
                {
                    int comparison = data.CompareTo(temp.Data);
                    if (comparison == 0)
                    {
                        return; // Duplicate not allowed
                    }
                    else if (comparison < 0)
                    {
                        if (temp.Left == null)
                        {
                            temp.Left = newNode;
                            newNode.Parent = temp;
                            break;
                        }
                        temp = temp.Left;
                    }
                    else
                    {
                        if (temp.Right == null)
                        {
                            temp.Right = newNode;
                            newNode.Parent = temp;
                            break;
                        }
                        temp = temp.Right;
                    }
                }

                FixRedRed(newNode);
            }
            count++;
        }

        private void FixRedRed(RBNode node)
        {
            // If node is root, color it black and return
            if (node == root)
            {
                node.Color = NodeColor.Black;
                return;
            }

            RBNode parent = node.Parent;
            RBNode grandparent = parent.Parent;
            RBNode uncle = node.Uncle();

            if (parent.Color != NodeColor.Black)
            {
                if (uncle != null && uncle.Color == NodeColor.Red)
                {
                    // Uncle red, perform recoloring and recurse
                    parent.Color = NodeColor.Black;
                    uncle.Color = NodeColor.Black;
                    grandparent.Color = NodeColor.Red;
                    FixRedRed(grandparent);
                }
                else
                {
                    // Else perform LR, LL, RL, RR
                    if (parent.IsLeftChild())
                    {
                        if (node.IsLeftChild())
                        {
                            // Left Left
                            SwapColors(parent, grandparent);
                        }
                        else
                        {
                            // Left Right
                            LeftRotate(parent);
                            SwapColors(node, grandparent);
                        }
                        RightRotate(grandparent);
                    }
                    else
                    {
                        if (node.IsLeftChild())
                        {
                            // Right Left
                            RightRotate(parent);
                            SwapColors(node, grandparent);
                        }
                        else
                        {
                            // Right Right
                            SwapColors(parent, grandparent);
                        }
                        LeftRotate(grandparent);
                    }
                }
            }
        }

        private void SwapColors(RBNode node1, RBNode node2)
        {
            NodeColor temp = node1.Color;
            node1.Color = node2.Color;
            node2.Color = temp;
        }

        private void LeftRotate(RBNode node)
        {
            RBNode newParent = node.Right;
            if (node == root)
                root = newParent;

            node.MoveDown(newParent);
            node.Right = newParent.Left;

            if (newParent.Left != null)
                newParent.Left.Parent = node;

            newParent.Left = node;
        }

        private void RightRotate(RBNode node)
        {
            RBNode newParent = node.Left;
            if (node == root)
                root = newParent;

            node.MoveDown(newParent);
            node.Left = newParent.Right;

            if (newParent.Right != null)
                newParent.Right.Parent = node;

            newParent.Right = node;
        }

        public bool Contains(T data)
        {
            return FindNode(data) != null;
        }

        private RBNode FindNode(T data)
        {
            RBNode temp = root;
            while (temp != null)
            {
                int comparison = data.CompareTo(temp.Data);
                if (comparison == 0)
                    return temp;
                else if (comparison < 0)
                    temp = temp.Left;
                else
                    temp = temp.Right;
            }
            return null;
        }

        public List<T> InOrderTraversal()
        {
            List<T> result = new List<T>();
            InOrderRec(root, result);
            return result;
        }

        private void InOrderRec(RBNode node, List<T> result)
        {
            if (node != null)
            {
                InOrderRec(node.Left, result);
                result.Add(node.Data);
                InOrderRec(node.Right, result);
            }
        }

        public bool IsValidRedBlackTree()
        {
            if (root == null) return true;
            if (root.Color != NodeColor.Black) return false;

            int blackCount = -1;
            return CheckRedBlackProperties(root, 0, ref blackCount);
        }

        private bool CheckRedBlackProperties(RBNode node, int blackCount, ref int expectedBlackCount)
        {
            if (node == null)
            {
                if (expectedBlackCount == -1)
                    expectedBlackCount = blackCount;
                return blackCount == expectedBlackCount;
            }

            // Check for consecutive red nodes
            if (node.Color == NodeColor.Red &&
                ((node.Left != null && node.Left.Color == NodeColor.Red) ||
                 (node.Right != null && node.Right.Color == NodeColor.Red)))
                return false;

            int newBlackCount = blackCount + (node.Color == NodeColor.Black ? 1 : 0);

            return CheckRedBlackProperties(node.Left, newBlackCount, ref expectedBlackCount) &&
                   CheckRedBlackProperties(node.Right, newBlackCount, ref expectedBlackCount);
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