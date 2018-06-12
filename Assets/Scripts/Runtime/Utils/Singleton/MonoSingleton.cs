using UnityEngine;

namespace Multiverse.Runtime
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance = null;
        private static object instanceLock = new object();

        public static T Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (null == instance)
                    {
                        instance = FindObjectOfType<T>();

                        if (null == instance)
                        {
                            GameObject singleton = new GameObject();
                            instance = singleton.AddComponent<T>();
                            singleton.name = string.Format("Singleton<{0}>", typeof(T).ToString());
                            DontDestroyOnLoad(singleton);
                        }

                    }
                    return instance;
                }
            }
        }

        protected virtual void OnDestroy()
        {
            instance = null;
        }
    }
}