using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ===================================================================================
 * SingeltonMonoBehavior - 
 * DESCRIPTION -makes whatever inherits from it a singleton.
 * =================================================================================== */

namespace Utils
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static GameObject Globals;
        private static T _instance;
        public static T Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// When Overriding, don't forget to call base.Awake();
        /// </summary>
        protected virtual void Awake()
        {
            if (null == _instance)
            {
                _instance = (T)this;
            }
            else if (_instance != this)
            {
                Debug.LogWarning("Multiple instances of " + GetType().Name + " present");
                Destroy(gameObject);
            }
        }

        public static bool Exists
        {
            get
            {
                return null != _instance;
            }
        }
    }
}