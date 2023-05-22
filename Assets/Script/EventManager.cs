using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IListener
{
    void OnEvent(string eventType, Component sender, object parameter = null);
}

public class EventManager : MonoBehaviour
{
    public static EventManager instance { get { return _instance; } }
    private static EventManager _instance = null;
    private Dictionary<string,List<IListener>> _listeners = new Dictionary<string,List<IListener>>();

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        DestroyImmediate(gameObject);
    }

    public void addListener(string eventType, IListener listener)
    {
        List<IListener> listenList = null;
        if(_listeners.TryGetValue(eventType, out listenList))
        {
            listenList.Add(listener);
            return;
        }

        listenList = new List<IListener>();
        listenList.Add(listener);
        _listeners.Add(eventType, listenList);
    }

    public void removeListener(IListener listener)
    {
        foreach(var listen in _listeners)
        {
            int listenerListCount = listen.Value.Count;
            for(int index =0; index<listenerListCount; ++index)
            {
                if (listen.Value[index] == listener)
                    listen.Value.RemoveAt(index);
            }
        }
    }

    public void postNotification(string eventType, Component sender, object parameter = null)
    {
        List<IListener> listenList = null;
        if (_listeners.TryGetValue(eventType, out listenList) == false)
            return;

        int listenListIndex = 0;
        foreach (var listener in listenList)
        {
            if(listener == null)
            {
                listenList.RemoveAt(listenListIndex);
                continue;
            }

            listener.OnEvent(eventType, sender, parameter);
            ++listenListIndex;
        }
    }

    public void RemoveEvent(string eventType)
    {
        _listeners.Remove(eventType);
    }

    public void RemoveRedundancies()
    {
        Dictionary<string, List<IListener>> newListeners = new Dictionary<string, List<IListener>>();

        foreach(KeyValuePair<string, List<IListener>> listener in _listeners)
        {
            for (int index = listener.Value.Count - 1; index >= 0; index--)
                listener.Value.RemoveAt(index);

            if(listener.Value.Count > 0)
                newListeners.Add(listener.Key, listener.Value);
        }

        _listeners = newListeners;
    }

    public void postPossibleEvent()
    {
        foreach(KeyValuePair<string, List<IListener>> pair in _listeners){
            if (pair.Key.Contains("possible"))
                postNotification(pair.Key, this, null);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RemoveRedundancies();
    }
}
