using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    protected virtual void Awake()
    {
        Instance = GetComponent<T>();
    }
}