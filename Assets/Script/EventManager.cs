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
    private Dictionary<string,List<IListener>> listeners = new Dictionary<string,List<IListener>>();

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
        if(listeners.TryGetValue(eventType, out listenList))
        {
            listenList.Add(listener);
            return;
        }

        listenList = new List<IListener>();
        listenList.Add(listener);
        listeners.Add(eventType, listenList);
    }

    public void postNotification(string eventType, Component sender, object parameter = null)
    {
        List<IListener> listenList = null;
        if (listeners.TryGetValue(eventType, out listenList) == false)
            return;

        foreach(var listener in listenList)
            listener.OnEvent(eventType, sender, parameter);
    }

    public void RemoveEvent(string eventType)
    {
        listeners.Remove(eventType);
    }

    public void RemoveRedundancies()
    {
        Dictionary<string, List<IListener>> newListeners = new Dictionary<string, List<IListener>>();

        foreach(KeyValuePair<string, List<IListener>> listener in listeners)
        {
            for (int index = listener.Value.Count - 1; index >= 0; index--)
                listener.Value.RemoveAt(index);

            if(listener.Value.Count > 0)
                newListeners.Add(listener.Key, listener.Value);
        }

        listeners = newListeners;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RemoveRedundancies();
    }


}
