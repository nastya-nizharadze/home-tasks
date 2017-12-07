using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO добавить локи

namespace BST
{
    static partial class BST
    {
        public class BinarySearchTree<K, V> : ITree<K, V> where K : IComparable<K>
        {
            public Node<K, V> root = null;

            public bool Insert(K key, V value)
            {
                Node<K, V> parent = null;
                Node<K, V> cur = root;

                while (cur != null)
                {
                    if (parent == null)
                    {
                        lock (cur)
                        {
                            parent = cur;
                            if (key.CompareTo(cur.key) < 0) cur = cur.left;
                            else if (key.CompareTo(cur.key) > 0) cur = cur.right;
                            else if (key.CompareTo(cur.key) == 0)
                            {
                                cur.value = value;
                                return (true);
                            }
                        }
                    }
                    else
                    {
                        lock (parent)
                        {
                            lock (cur)
                            {
                                parent = cur;
                                if (key.CompareTo(cur.key) < 0) cur = cur.left;
                                else if (key.CompareTo(cur.key) > 0) cur = cur.right;
                                else if (key.CompareTo(cur.key) == 0)
                                {
                                    cur.value = value;
                                    return (true);
                                }
                            }
                        }
                    }
                }


                if (parent == null)
                {
                    root = new Node<K, V>
                    {
                        key = key,
                        value = value
                    };
                    return (true);
                }
                lock (parent)
                {
                    if (key.CompareTo(parent.key) < 0)
                    {
                        cur = new Node<K, V>
                        {
                            key = key,
                            value = value,
                            parent = parent
                        };
                        parent.left = cur;
                        return (true);
                    }
                    else
                    {
                        cur = new Node<K, V>
                        {
                            key = key,
                            value = value,
                            parent = parent
                        };
                        parent.right = cur;
                        return (true);
                    }
                }
            }

            public void Delete(K key)
            {
                Node<K,V> delNode = FindNode(key);
                if (delNode == null) return;
                Node<K, V> delParent = delNode.parent;

                if (delParent == null && delNode.left == null && delNode.right == null)
                {
                    lock (delNode)
                    {
                        root = null;
                        return;
                    }
                }
                else if (delParent != null && delNode.left == null && delNode.right == null)
                {
                    lock (delParent)
                    {
                        lock (delNode)
                        {
                            if (delNode == delParent.left)
                                delParent.left = null;

                            if (delNode == delParent.right)
                                delParent.right = null;
                        }
                    }
                }
                else if (delParent == null && delNode.left == null && delNode.right != null)
                {
                    lock (delNode)
                    {
                        delNode.right.parent = null;
                        root = delNode.right;
                    }
                }
                else if (delParent == null && delNode.right == null && delNode.left != null)
                {
                    lock (delNode)
                    {
                        delNode.left.parent = null;
                        root = delNode.left;
                    }
                }
                else if (delParent != null && (delNode.right == null || delNode.left == null))
                {
                    lock (delParent)
                    {
                        lock (delNode)
                        {
                            if (delNode.right != null)
                            {
                                if (delParent.left == delNode)
                                {
                                    delNode.right.parent = delParent;
                                    delParent.left = delNode.right;
                                }
                                else if (delParent.right == delNode)
                                {
                                    delNode.right.parent = delParent;
                                    delParent.right = delNode.right;
                                }
                            }
                            else if (delNode.left != null)
                            {
                                if (delParent.left == delNode)
                                {
                                    delNode.left.parent = delParent;
                                    delParent.left = delNode.left;
                                }
                                else if (delParent.right == delNode)
                                {
                                    delNode.left.parent = delParent;
                                    delParent.right = delNode.left;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (delParent != null)
                    {
                        lock (delParent)
                        {
                            lock (delNode)
                            {
                                var temp = Min(delNode.right);
                                delNode.key = temp.key;

                                if (temp.parent.left == temp)
                                {
                                    temp.parent.left = temp.right;

                                    if (temp.right != null)
                                        temp.right.parent = temp.parent;
                                }
                                else
                                {
                                    temp.parent.right = temp.right;

                                    if (temp.right != null)
                                        temp.right.parent = temp.parent;
                                }
                            }
                        }
                    }
                    else if (delParent == null)
                    {
                        lock (delNode)
                        {
                            var temp = Min(delNode.right);
                            delNode.key = temp.key;

                            if (temp.parent.left == temp)
                            {
                                temp.parent.left = temp.right;

                                if (temp.right != null)
                                    temp.right.parent = temp.parent;
                            }
                            else
                            {
                                temp.parent.right = temp.right;

                                if (temp.right != null)
                                    temp.right.parent = temp.parent;
                            }
                        }
                    }
                }
            }

            public V Find(K key)
            {
                Node<K,V> node = (FindNode(key));
                if (node == null) return default(V);
                else return (node.value);
            }

            public Node<K, V> FindNode(K key)
            {
                Node<K, V> cur = root;
                while (cur != null)
                {
                    if (cur.parent == null)
                    {
                        lock (cur)
                        { 
                            if (key.CompareTo(cur.key) == 0)
                            {
                                return cur;
                            }
                            else if (key.CompareTo(cur.key) < 0) cur = cur.left;
                            else cur = cur.right;
                        }
                    }
                    else if (cur.parent != null)
                    {
                        lock (cur.parent)
                        {
                            lock (cur)
                            {
                                if (key.CompareTo(cur.key) == 0)
                                {
                                    return cur;
                                }
                                else if (key.CompareTo(cur.key) < 0) cur = cur.left;
                                else cur = cur.right;
                            }
                        }
                    }
                }
                return null;
            }

            public bool IsBreak(Node<K, V> node)
            {
                if (node == null) return (true);
                if (node.left != null)
                {
                    if ((Max(node.left).key.CompareTo(node.key) > 0)) return false;
                }
                if (node.right != null)
                {
                    if ((Min(node.right).key.CompareTo(node.key) < 0)) return false;
                }
                if (!IsBreak(node.left) || !IsBreak(node.right)) return false;
                return true;
            }

            public Node<K, V> Min(Node<K, V> cur)
            {
                if (cur.left == null) return cur;
                else return Min(cur.left);
            }

            public Node<K, V> Max(Node<K, V> cur)
            {
                if (cur.right == null) return cur;
                else return Max(cur.right);
            }
        }
    }
}