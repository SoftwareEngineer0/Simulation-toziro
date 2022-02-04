using UnityEngine;

namespace MyVR_Assets
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {

        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<T>();
                    if (instance == null)
                    {
                        Debug.LogError(typeof(T) + "をアタッチしているGameObjectはありません");
                    }
                }
                return instance;
            }
        }

    }
}

