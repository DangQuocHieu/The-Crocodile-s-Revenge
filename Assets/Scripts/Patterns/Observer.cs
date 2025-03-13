using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    static Dictionary<GameEvent, List<Action<object[]>>> Listeners = new Dictionary<GameEvent, List<Action<object[]>>>();
    public static void AddObserver(GameEvent name, Action<object[]> callback)
    {
        if(!Listeners.ContainsKey(name))
        {
            Listeners.Add(name, new List<Action<object[]>>());
        }
        Listeners[name].Add(callback);
    }

    public static void RemoveListener(GameEvent name, Action<object[]> callback)
    {
        if(!Listeners.ContainsKey(name))
        {
            return;
        }
        Listeners[name].Remove(callback);
    }

    public static void Notify(GameEvent name, params object[] datas)
    {
        if(!Listeners.ContainsKey(name))
        {
            return;
        }
        foreach(var item in Listeners[name])
        {
            item?.Invoke(datas);
        }
    }
}
