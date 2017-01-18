
namespace Monitor.Core
{
    public class Singleton<T> where T : Singleton<T>, new()
    {
        private static T instance = null;

        private static object syncObj = new object();

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncObj)
                    {
                        if (instance == null)
                        {
                            instance = new T();
                        }
                    }
                }

                return instance;
            }
        }
    }
}
