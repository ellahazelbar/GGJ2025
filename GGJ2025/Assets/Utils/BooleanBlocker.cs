using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 436
/* ===================================================================================
 * BooleanBlocker -
 * DESCRIPTION - A good substitute for when multiple things need to interact with 
 * a single boolean. 
 * 
 * =================================================================================== */
namespace Utils
{
    /// <summary>
    /// This class keeps track of every object that wants to set a boolean to false, 
    /// and only when there are no objects inhibiting that boolean from being true, it returns true.
    /// <para>
    /// Inhibit means disable, disallow.
    /// </para>
    /// </summary>
    /// <remarks>
    /// For example, when multiple things want to use the mouse at the same time, 
    /// it would be annoying if when the first object was done with the mouse it would 
    /// lock it. This class could be used to keep track of all the objects that want 
    /// to use the mouse, and only when no one needs the mouse anymore, lock it. 
    /// See BooleanProhibitor class for the opposite behavior. 
    /// </remarks>
    public class BooleanInhibitor
    {
        /// <summary>
        /// Called when transitioning from uninhibited to inhibited.
        /// <para>
        /// In other words, called when an inhibitor is added when there were none. 
        /// </para> 
        /// </summary>
        public event System.Action OnInhibited;
        /// <summary>
        /// Called when the last inhibitor is removed. 
        /// </summary>
        public event System.Action OnUninhibited;

        private HashSet<object> inhibitors;

        public BooleanInhibitor()
        {
            inhibitors = new HashSet<object>();
        }

        public BooleanInhibitor(IEqualityComparer<object> Comparer)
        {
            inhibitors = new HashSet<object>(Comparer);
        }

        /// <summary>
        /// Add an inhibitor. <see cref="UnInhibited"/> will return false at least until <see cref="RemoveInhibitor(object)"/> is called with the same inhibitor.
        /// </summary>
        /// <param name="Caller">The inhibitor to add (and remove later). </param>
        public void AddInhibitor(object Caller)
        {
            bool wasUninhibited = UnInhibited;
            if (!inhibitors.Contains(Caller))
                inhibitors.Add(Caller);
            if (wasUninhibited && null != OnInhibited)
                OnInhibited();
        }

        /// <summary>
        /// Remove a previously added inhibitor. If this was the last inhibitor, <see cref="OnUninhibited"/> will be called and <see cref="UnInhibited"/> will return true after this method is finished. 
        /// </summary>
        /// <param name="Caller">The inhibitor to remove. </param>
        public void RemoveInhibitor(object Caller)
        {
            if (inhibitors.Remove(Caller) && UnInhibited && null != OnUninhibited)
            {
                OnUninhibited();
            }
        }

        /// <summary>
        /// Returns true if there are no inhibitors presernt. 
        /// </summary>
        public bool UnInhibited { get { return inhibitors.Count == 0; } }

        public static implicit operator bool (BooleanInhibitor b)
        {
            return null != b && b.UnInhibited;
        }
    }

    public class BooleanInhibitor<T>
    {
        /// <summary>
        /// Called when transitioning from uninhibited to inhibited.
        /// <para>
        /// In other words, called when an inhibitor is added when there were none. 
        /// </para> 
        /// </summary>
        public event System.Action OnInhibited;
        /// <summary>
        /// Called when the last inhibitor is removed. 
        /// </summary>
        public event System.Action OnUninhibited;

        private HashSet<T> inhibitors;

        public BooleanInhibitor()
        {
            inhibitors = new HashSet<T>();
        }

        public BooleanInhibitor(IEqualityComparer<T> Comparer)
        {
            inhibitors = new HashSet<T>(Comparer);
        }

        /// <summary>
        /// Add an inhibitor. 
        /// </summary>
        /// <param name="Caller">The inhibitor to add (and remove later). </param>
        public void AddInhibitor(T Caller)
        {
            bool wasUninhibited = UnInhibited;
            if (!inhibitors.Contains(Caller))
                inhibitors.Add(Caller);
            if (wasUninhibited && null != OnInhibited)
                OnInhibited();
        }

        /// <summary>
        /// Remove a previously added inhibitor. 
        /// </summary>
        /// <param name="Caller">The inhibitor to remove. </param>
        public void RemoveInhibitor(T Caller)
        {
            if (inhibitors.Remove(Caller) && UnInhibited && null != OnUninhibited)
            {
                OnUninhibited();
            }
        }

        /// <summary>
        /// Returns true if there are no inhibitors presernt. 
        /// </summary>
        public bool UnInhibited { get { return inhibitors.Count == 0; } }

        public static implicit operator bool (BooleanInhibitor<T> b)
        {
            return null != b && b.UnInhibited;
        }
    }

    /// <summary>
    /// Returns true if there are any prohibitors present.     
    /// <para>
    /// Prohibit means enable, allow.
    /// </para>
    /// </summary>
    public class BooleanEnabler
    {
        /// <summary>
        /// Called when the first prohibitor is added, i.e. when the value changes from false to true. 
        /// </summary>
        public event System.Action OnEnabled;
        /// <summary>
        /// Called when the last prohibitor is removed. 
        /// </summary>
        public event System.Action OnUnenabled;

        private HashSet<object> enablers;

        public BooleanEnabler()
        {
            enablers = new HashSet<object>();
        }

        public BooleanEnabler(IEqualityComparer<object> Comparer)
        {
            enablers = new HashSet<object>(Comparer);
        }

        /// <summary>
        /// Add a prohibitor and allow the class to return true. 
        /// </summary>
        /// <param name="Caller">The prohibitor to add (and remove later). </param>
        public void AddEnabler(object Caller)
        {
            if (!enablers.Contains(Caller))
            {
                bool wasUnprohibited = !Enabled;
                enablers.Add(Caller);
                if (wasUnprohibited && null != OnEnabled)
                    OnEnabled();
            }
        }

        /// <summary>
        /// Removes a priviously added prohibitor. 
        /// </summary>
        /// <param name="Caller">The prohibitor to remove. </param>
        public void RemoveEnabler(object Caller)
        {
            if (enablers.Remove(Caller) && Enabled && null != OnUnenabled)
            {
                OnUnenabled();
            }
        }

        /// <summary>
        /// Returns true if there are enablers presernt. 
        /// </summary>
        public bool Enabled { get { return enablers.Count > 0; } }

        public static implicit operator bool (BooleanEnabler b)
        {
            return null != b && b.Enabled;
        }
    }

    /// <summary>
    /// Returns true if there are any prohibitors present.     
    /// <para>
    /// Prohibit means enable, allow.
    /// </para>
    /// </summary>
    public class BooleanProhibitor<T>
    {
        /// <summary>
        /// Called when the first prohibitor is added, i.e. when the value changes from false to true. 
        /// </summary>
        public event System.Action OnProhibited;
        /// <summary>
        /// Called when the last prohibitor is removed. 
        /// </summary>
        public event System.Action OnUnprohibited;

        private HashSet<T> prohibitors;

        public BooleanProhibitor()
        {
            prohibitors = new HashSet<T>();
        }

        public BooleanProhibitor(IEqualityComparer<T> Comparer)
        {
            prohibitors = new HashSet<T>(Comparer);
        }

        /// <summary>
        /// Add a prohibitor and allow the class to return true. 
        /// </summary>
        /// <param name="Item">The prohibitor to add (and remove later). </param>
        public void Add(T Item)
        {
            if (!prohibitors.Contains(Item))
            {
                bool wasUnprohibited = !Prohibited;
                prohibitors.Add(Item);
                if (wasUnprohibited && null != OnProhibited)
                    OnProhibited();
            }
        }

        /// <summary>
        /// Removes a priviously added prohibitor. 
        /// </summary>
        /// <param name="Item">The prohibitor to remove. </param>
        public void Remove(T Item)
        {
            bool prohibited = Prohibited;
            if (prohibitors.Remove(Item) && prohibited && !Prohibited && null != OnUnprohibited)
            {
                OnUnprohibited();
            }
        }

        /// <summary>
        /// Returns true if there are no inhibitors presernt. 
        /// </summary>
        public bool Prohibited { get { return prohibitors.Count > 0; } }

        public static implicit operator bool (BooleanProhibitor<T> b)
        {
            return null != b && b.Prohibited;
        }
    }
}
#pragma warning restore 436
