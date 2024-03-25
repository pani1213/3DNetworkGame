using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T mInstance = null;
    public static T Instance
    {
        get
        {
            if (Equals(mInstance, null))
            {
                mInstance = FindObjectOfType(typeof(T)) as T;

                if (Equals(mInstance, null))
                {
                    GameObject go = new GameObject();
                    go.name = "_" + typeof(T).ToString();
                    mInstance = go.AddComponent<T>();

                    DontDestroyOnLoad(go);
                }
            }
            return mInstance;
        }
    }

    public virtual void OnApplicationQuit()
    {
        mInstance = null;
    }
    public virtual void OnDestroy()
    {
        mInstance = null;
    }
}
