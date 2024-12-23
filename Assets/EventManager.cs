using System;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public event Action OnGenerateNewLevel;

    public void GenerateNewLevel()
    {
        OnGenerateNewLevel?.Invoke();
    }
}
