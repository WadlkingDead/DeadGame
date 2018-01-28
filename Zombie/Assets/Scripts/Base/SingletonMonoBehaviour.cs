
using UnityEngine;
using System.Collections;

public class SingletonMonoBehaviour<T> : MonoBehaviour  where T : MonoBehaviour
{
    public static T Instance = null;

    protected virtual void Awake()
    {
        Instance = this as T;
    }

	
}
