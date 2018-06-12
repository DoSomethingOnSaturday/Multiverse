using System;

namespace Multiverse.Runtime
{
    public class Singleton<T> where T : class
    {
        private static T instance = default(T);
        private static object instanceLock = new object();

        public static T Instance
        {
            get
            {
                return singleton;
            }
        }

        public static T singleton
        {
            get
            {
                if (null == instance)
                {
                    lock (instanceLock)
                    {
                        if (null == instance)
                        {
                            instance = (T)Activator.CreateInstance(typeof(T), true);
                        }
                    }
                }
                return instance;
            }
        }

        protected Singleton(){}
    }
}