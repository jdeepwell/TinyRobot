using System.Linq;
using UnityEngine;

namespace Deepwell
{
    /*
     * MonoSingleton
     * An abstract class to create Singletons
     * Subclassing:
     * public class MyClassName : MonoSingleton<MyClassName>
     * Access single instance wth: MyClassName.Instance
     * Choose class name starting with "Static..." (e.g. "StaticGameManager" as opposed to "GameManager)
     * to make the Singleton static (currently using DontUnloadUnusedAsset hide flag)
     * (C) 2020 Deepwell.at
     */

    public abstract class MonoSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (!_instance) _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                if (!_instance)
                {
                    // Debug.Log("Creating new DWMonoSingleton of type " + typeof(T).ToString());
                    _instance = CreateInstance<T>();
                    if (ShouldBeStatic())
                    {
                        // Debug.Log("Making new DWMonoSingleton of type " + typeof(T).ToString() + " static");
                        _instance.hideFlags = HideFlags.DontSave | HideFlags.DontUnloadUnusedAsset;
                    }
                }
                return _instance;
            }
        }

        public static bool Exists()
        {
            return _instance != null;
        }

        public static void ResetInstance()
        {
            if (!_instance) return;
            DestroyImmediate(_instance);
            _instance = null;
            Debug.Log("Resetting MonoSingleton of type " + typeof(T) + ": " + Instance);
        }

        private static bool ShouldBeStatic()
        {
            return typeof(T).ToString().StartsWith("Static");
        }
    } 

}
