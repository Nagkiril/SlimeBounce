using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Settings.Generic
{
    public abstract class GenericSettings<T> : ScriptableObject
    {
        protected static string ObjectName = "Test";

        private static GenericSettings<T> _loadedInstance;

        protected static GenericSettings<T> GetInstance(string loadPath)
        {
            if (_loadedInstance == null)
                _loadedInstance = Resources.Load<GenericSettings<T>>(loadPath);
            return _loadedInstance;
        }
    }
}