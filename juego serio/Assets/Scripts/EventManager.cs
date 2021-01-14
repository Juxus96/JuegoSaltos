using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    private Dictionary<string, Action> voidAction;
    private Dictionary<string, Action<int>> intAction;

    private void Awake()
    {
        CreateSingleton();
        voidAction = new Dictionary<string, Action>();
        intAction = new Dictionary<string, Action<int>>();
    }

    #region Void Action
    public void SuscribeToEvent(string key, Action answer)
    {
        if (!voidAction.ContainsKey(key))
        {
            voidAction.Add(key, () => { });
        }
        voidAction[key] += answer;

    }

    public void UnsuscribeFromEvent(string key, Action answer)
    {
        if (voidAction.ContainsKey(key))
            voidAction[key] -= answer;
    }

    public void RaiseEvent(string key)
    {
        voidAction[key]?.Invoke();
    }
    #endregion

    #region Int Action
    public void SuscribeToEvent(string key, Action<int> answer)
    {
        if (!intAction.ContainsKey(key))
        {
            intAction.Add(key, (int i) => { });
        }
        intAction[key] += answer;

    }

    public void UnsuscribeFromEvent(string key, Action<int> answer)
    {
        if (intAction.ContainsKey(key))
            intAction[key] -= answer;
    }

    public void RaiseEvent(string key, int i)
    {
        intAction[key]?.Invoke(i);
    }
    #endregion

    private void CreateSingleton()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }
}
