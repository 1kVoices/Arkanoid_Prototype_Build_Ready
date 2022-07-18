using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents singleton;

    public event Action onTrigger;

    public event Action<string> onLog;

    private void Awake()
    {
        if (!singleton)
        {
            singleton = this;
            DontDestroyOnLoad(this);
        }
        
        else Destroy(gameObject);
    }

    public void TriggerEnter()
    {
        onTrigger?.Invoke();
    }

    public void Log(string x)
    {
        onLog?.Invoke(x);
    }
}
