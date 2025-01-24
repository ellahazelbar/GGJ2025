using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/* ===================================================================================
 * CompositeTree -
 * DESCRIPTION - A tree design pattern, in which every node contains a T value and a dictionary of child nodes.
 * =================================================================================== */
namespace Utils
{
    public class CompositeTree<T> : IEnumerable<CompositeTree<T>>
    {
        /// <summary>
        /// The value stored at this node.
        /// </summary>
        public T Value;
        /// <summary>
        /// The parent of this 
        /// </summary>
        public CompositeTree<T> Parent { get; private set; }

        protected Dictionary<T, CompositeTree<T>> children;

        private IEqualityComparer<T> comparer;

        /// <summary>
        /// Create a new tree.
        /// </summary>
        /// <param name="Value">The value of the topmost parent tree. </param>
        public CompositeTree(T Value) : this(Value, null, null)
        {

        }

        /// <summary>
        /// Create a new tree with an equality comparer used to compare children by the internal dictionary.
        /// </summary>
        /// <param name="Value">The value of the topmost parent tree. </param>
        /// <param name="Comparer">The comparer by which the internal dictionary compares the children. </param>
        public CompositeTree(T Value, IEqualityComparer<T> Comparer) : this(Value, Comparer, null)
        {
        }

        protected CompositeTree(T Value, IEqualityComparer<T> Comparer, CompositeTree<T> parent)
        {
            this.Value = Value;
            comparer = Comparer;
            if (null == Comparer) children = new Dictionary<T, CompositeTree<T>>();
            else children = new Dictionary<T, CompositeTree<T>>(Comparer);
            Parent = parent;
        }

        /// <summary>
        /// Add a child to this tree node. 
        /// </summary>
        /// <param name="Child"></param>
        /// <returns></returns>
        public CompositeTree<T> AddChild(T Child)
        {
            CompositeTree<T> newChild = new CompositeTree<T>(Child, comparer, this);
            children.Add(Child, newChild);
            return newChild;
        }

        /// <summary>
        /// Removes a child node.
        /// </summary>
        /// <param name="Child"></param>
        public bool RemoveChild(T Child)
        {
            return children.Remove(Child);
        }

        /// <summary>
        /// Returns a child node by value.
        /// </summary>
        /// <param name="Child">The value stored in the child node to return. </param>
        /// <exception cref="KeyNotFoundException">Thrown if no child exists with the given key.</exception>
        public CompositeTree<T> GetChild(T Child)
        {
            return children[Child];
        }

        /// <summary>
        /// Try to get a child by the value stored in that child.
        /// </summary>
        /// <param name="ChildValue">The value stored in the wanted child.</param>
        /// <param name="Child">The out node value.</param>
        public bool TryGetChild(T ChildValue, out CompositeTree<T> Child)
        {
            if (HasChild(ChildValue))
            {
                Child = GetChild(ChildValue);
                return true;
            }
            Child = null;
            return false;
        }

        /// <summary>
        /// Get a child node by evaluating the predicate. 
        /// </summary>
        public CompositeTree<T> GetChild(System.Predicate<CompositeTree<T>> Predicate)
        {
            foreach (CompositeTree<T> child in children.Values)
                if (Predicate(child))
                    return child;
            return null;
        }

        /// <summary>
        /// Get a child node by evaluating the 
        /// </summary>
        /// <param name="Predicate"></param>
        /// <param name="Child"></param>
        /// <returns></returns>
        public bool TryGetChild(System.Predicate<CompositeTree<T>> Predicate, out CompositeTree<T> Child)
        {
            foreach (CompositeTree<T> child in children.Values)
                if (Predicate(child))
                {
                    Child = child;
                    return true;
                }
            Child = null;
            return false;
        }

        /// <summary>
        /// If a child node with the same value already exists, return it. Otherwise add a new one and return it.
        /// </summary>
        /// <param name="Child">The child to add or get.</param>
        /// <returns></returns>
        public CompositeTree<T> AddOrGetChild(T Child)
        {
            if (!HasChild(Child))
                return AddChild(Child);
            return GetChild(Child);
        }

        /// <summary>
        /// returns whether a child node exists whose stored value equals the given value.
        /// </summary>
        /// <param name="Child">The value of the child.</param>
        /// <returns></returns>
        public bool HasChild(T Child)
        {
            return children.ContainsKey(Child);
        }

        /// <summary>
        /// returns 
        /// </summary>
        /// <param name="Predicate"></param>
        /// <returns></returns>
        public bool HasChild(System.Predicate<CompositeTree<T>> Predicate)
        {
            foreach (CompositeTree<T> child in children.Values)
                if (Predicate(child))
                    return true;
            return false;
        }

        public IEnumerator<CompositeTree<T>> GetEnumerator()
        {
            return children.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// The amount of children of this node.
        /// </summary>
        public int Count { get { return children.Count; } }

        public CompositeTree<T>[] Children { get { return children.Values.ToArray(); } }
    }
}