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
    private Dictionary<string, Action<Vector2, Vector2>> vector2Vector2Action;
    private Dictionary<string, Action<Vector2, int>> vector2IntAction;

    private Dictionary<string, Func<Transform>> transformFunc;
    private Dictionary<string, Func<Vector2, bool>> vector2FuncBool;
    private Dictionary<string, Func<Vector2, Vector2>> vector2FuncVector2;
    private Dictionary<string, Func<Vector2, Vector2, Vector2>> vector2Vector2FuncVector2;

    private void Awake()
    {
        CreateSingleton();
        voidAction = new Dictionary<string, Action>();
        intAction = new Dictionary<string, Action<int>>();
        vector2Action = new Dictionary<string, Action<Vector2>>();
        vector2Vector2Action = new Dictionary<string, Action<Vector2, Vector2>>();
        vector2IntAction = new Dictionary<string, Action<Vector2, int>>();
        transformFunc = new Dictionary<string, Func<Transform>>();
        vector2FuncBool = new Dictionary<string, Func<Vector2, bool>>();
        vector2FuncVector2 = new Dictionary<string, Func<Vector2, Vector2>>();
        vector2Vector2FuncVector2 = new Dictionary<string, Func<Vector2, Vector2, Vector2>>();
    }

    #region Void Action
    public void SuscribeToEvent(string key, Action answer)
    {
        if (!voidAction.ContainsKey(key))
        {
            voidAction.Add(key, answer);
        }
        else
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
            intAction.Add(key, answer);
        }
        else
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
            vector2Action.Add(key, answer);
        }
        else
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

    #region Vector2 Vector2 Action
    public void SuscribeToEvent(string key, Action<Vector2, Vector2> answer)
    {
        if (!vector2Vector2Action.ContainsKey(key))
        {
            vector2Vector2Action.Add(key, answer);
        }
        else
            vector2Vector2Action[key] += answer;

    }

    public void UnsuscribeFromEvent(string key, Action<Vector2, Vector2> answer)
    {
        if (vector2Vector2Action.ContainsKey(key))
            vector2Vector2Action[key] -= answer;
    }

    public void RaiseEvent(string key, Vector2 vector2, Vector2 v2)
    {
        vector2Vector2Action[key]?.Invoke(vector2, v2);
    }
    #endregion

    #region Vector2 Int Action
    public void SuscribeToEvent(string key, Action<Vector2,int> answer)
    {
        if (!vector2IntAction.ContainsKey(key))
        {
            vector2IntAction.Add(key, answer);
        }
        else 
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

    #region Func Transform
    public void SuscribeToTransformEvent(string key, Func<Transform> answer)
    {
        if (!transformFunc.ContainsKey(key))
        {
            transformFunc.Add(key, answer);
        }
        else
            transformFunc[key] += answer;
    }

    public void UnsuscribeFromTransformEvent(string key, Func<Transform> answer)
    {
        if (transformFunc.ContainsKey(key))
            transformFunc[key] -= answer;
    }

    public Transform RaiseTransformEvent(string key)
    {
        if (transformFunc != null)
            return transformFunc[key]();
        else
            return null;
    }
    #endregion

    #region Vector2 Func Bool
    public void SuscribeToBoolEvent(string key, Func<Vector2, bool> answer)
    {
        if (!vector2FuncBool.ContainsKey(key))
        {
            vector2FuncBool.Add(key, answer);
        }
        else
            vector2FuncBool[key] += answer;
    }

    public void UnsuscribeFromBoolEvent(string key, Func<Vector2, bool> answer)
    {
        if (vector2FuncBool.ContainsKey(key))
            vector2FuncBool[key] -= answer;
    }

    public bool RaiseBoolEvent(string key, Vector2 vector2)
    {
        if (vector2FuncBool != null)
            return vector2FuncBool[key](vector2);
        else
            return false;
    }
    #endregion

    #region Vector2 Vector2 Func Vector2
    public void SuscribeToVect2Event(string key, Func<Vector2, Vector2, Vector2> answer)
    {
        if (!vector2Vector2FuncVector2.ContainsKey(key))
        {
            vector2Vector2FuncVector2.Add(key, answer);
        }
        else
            vector2Vector2FuncVector2[key] += answer;
    }

    public void UnsuscribeFromVect2Event(string key, Func<Vector2, Vector2, Vector2> answer)
    {
        if (vector2Vector2FuncVector2.ContainsKey(key))
            vector2Vector2FuncVector2[key] -= answer;
    }

    public Vector2 RaiseVect2Event(string key, Vector2 vector2, Vector2 v2)
    {
        if (vector2Vector2FuncVector2 != null)
            return vector2Vector2FuncVector2[key](vector2,v2);
        else
            return Vector2.up*int.MaxValue;
    }
    #endregion

    #region Vector2 Vector2 Func Bool
    public void SuscribeToVect2Event(string key, Func<Vector2, Vector2> answer)
    {
        if (!vector2FuncVector2.ContainsKey(key))
        {
            vector2FuncVector2.Add(key, answer);
        }
        else
            vector2FuncVector2[key] += answer;
    }

    public void UnsuscribeFromVect2Event(string key, Func<Vector2, Vector2> answer)
    {
        if (vector2FuncVector2.ContainsKey(key))
            vector2FuncVector2[key] -= answer;
    }

    public Vector2 RaiseVect2Event(string key, Vector2 vector2)
    {
        if (vector2FuncVector2 != null)
            return vector2FuncVector2[key](vector2);
        else
            return Vector2.up * int.MaxValue;
    }
    #endregion
    private void CreateSingleton()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }
}


