using UnityEngine;

    /// <summary>
    /// <para> 내    용 : 싱글톤 패턴 클래스 </para>
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    T[] _typeArr = FindObjectsOfType<T>();

                    //if (instance == null)
                    //{
                    //    instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    //}
                    if (_typeArr.Length == 1)
                    {
                        instance = _typeArr[0];
                    }
                    if (_typeArr.Length > 1)
                    {
                        for (int i = 1; i < _typeArr.Length - 1; i++)
                        {
                            Debug.Log("<color=#FF5733>Destroy:</color>" + typeof(T));
                            Destroy(_typeArr[i].gameObject);
                        }
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            Func_Init();
        }

        /// <summary>
        /// <para> 내    용 : 싱글톤 패턴을 시작할 때 초기화하는 메서드 </para>
        /// </summary>
        protected virtual void Func_Init()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            else
            {
                Debug.Log("<color=#FF5733>Destroy:</color>" + typeof(T));
                Destroy(gameObject);
            }
        }
    }