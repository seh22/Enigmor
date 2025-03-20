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
                // 해당 컴포넌트를 가지고 있는 게임 오브젝트를 찾아서 반환한다.
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
        if (transform.parent != null && transform.root != null) // 해당 오브젝트가 자식 오브젝트라면
        {
            DontDestroyOnLoad(this.transform.root.gameObject); // 부모 오브젝트를 DontDestroyOnLoad 처리
        }
        else
        {
            DontDestroyOnLoad(this.gameObject); // 해당 오브젝트가 최 상위 오브젝트라면 자신을 DontDestroyOnLoad 처리
        }
    }
}