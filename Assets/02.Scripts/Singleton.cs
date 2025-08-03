using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject singletonObject = new(typeof(T).Name);
                    instance = singletonObject.AddComponent<T>();
                    (instance as Singleton<T>).Initialize();
                }
            }
            return instance;
        }
    }

    public bool useDontDestroyOnLoad = true;


    protected virtual void Initialize() { }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this as T;
            Initialize();
            if (useDontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }
    }
}
