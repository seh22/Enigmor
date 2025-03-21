using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
           
                instance = (T)FindAnyObjectByType(typeof(T));

                if (instance == null)
                {
                    GameObject gameObj = new GameObject(typeof(T).Name, typeof(T));
                    instance = gameObj.AddComponent<T>();

                }
            }
            return instance;
        }
    }
    public virtual void Awake()
    {
        if (transform.parent != null && transform.root != null)
        {
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}