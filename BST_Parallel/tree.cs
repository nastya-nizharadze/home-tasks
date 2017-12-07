using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BST
{
    static partial class BST
    {
        public interface ITree<K, V> where K : IComparable<K>
        {
            bool Insert(K key, V value);

            void Delete(K key);

            V Find(K key);
        }
    }
}