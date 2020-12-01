using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using UnityEngine;

using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace idbrii.lib.util
{
    public class GlobalCoroutine : MonoBehaviour
    {

        static GlobalCoroutine _Instance;
        public static GlobalCoroutine Instance
        {
            get {
                if (_Instance == null)
                {
                    _Instance = new GameObject("GlobalCoroutine").AddComponent<GlobalCoroutine>();
                }
                return _Instance;
            }
        }

        void OnDestroy()
        {
            _Instance = null;
        }
        
    }
}
