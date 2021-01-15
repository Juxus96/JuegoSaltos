using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    private Dictionary<string, Action> voidAction;
    private Dictionary<string, Action<int>> intAction;
    private Dictionary<string, Action<Vector2>> vector2Action;
    private Dictionary<string, Action<Vector2, int>> vector2IntAction;

    private void Awake()
    {
        CreateSingleton();
        voidAction = new Dictionary<string, Action>();
        intAction = new Dictionary<string, Action<int>>();
        vector2Action = new Dictionary<string, Action<Vector2>>();
        vector2IntAction = new Dictionary<string, Action<Vector2, int>>();
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

    #region Vector2 Action
    public void SuscribeToEvent(string key, Action<Vector2> answer)
    {
        if (!vector2Action.ContainsKey(key))
        {
            vector2Action.Add(key, (Vector2 v) => { });
        }
        vector2Action[key] += answer;

    }

    public void UnsuscribeFromEvent(string key, Action<Vector2> answer)
    {
        if (vector2Action.ContainsKey(key))
            vector2Action[key] -= answer;
    }

    public void RaiseEvent(string key, Vector2 vector2)
    {
        vector2Action[key]?.Invoke(vector2);
    }
    #endregion

    #region Vector2 Action
    public void SuscribeToEvent(string key, Action<Vector2,int> answer)
    {
        if (!vector2IntAction.ContainsKey(key))
        {
            vector2IntAction.Add(key, (Vector2 v, int i) => { });
        }
        vector2IntAction[key] += answer;

    }

    public void UnsuscribeFromEvent(string key, Action<Vector2,int> answer)
    {
        if (vector2IntAction.ContainsKey(key))
            vector2IntAction[key] -= answer;
    }

    public void RaiseEvent(string key, Vector2 vector2, int i)
    {
        vector2IntAction[key]?.Invoke(vector2, i);
    }
    #endregion

    private void CreateSingleton()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }
}


