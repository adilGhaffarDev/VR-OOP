using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour 
{

    private class SingleParamUnityEvent : UnityEvent<object>
    {

    }

    private Dictionary<string, SingleParamUnityEvent> eventDictionary;

    private static EventManager instance = null;
    public static EventManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!instance)
                {
                    //Debug.LogError ("There needs to be one active EventManger script on a GameObject in your scene.");
                    instance = new GameObject("EventManager").AddComponent<EventManager>();
                    instance.Init();
                }
                else
                {
                    instance.Init();
                }
            }

            return instance;
        }
    }

    private void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, SingleParamUnityEvent>();
        }
    }

    public static void StartListening(string eventName, UnityAction<object> listener)
    {
        SingleParamUnityEvent thisEvent = null;

        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new SingleParamUnityEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<object> listener)
    {
        if (instance == null)
        {
            return;
        }

        SingleParamUnityEvent thisEvent = null;

        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, object userData)
    {
        SingleParamUnityEvent thisEvent = null;

        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(userData);
        }
    }
}

///
/////
// Event Data Encapsulations
/////
///

public class QuestionAnsweredEvenData
{
    bool _isCorrect;
    string _answerGiven;
    LevelData _levelData;

    public bool IsCorrect => _isCorrect;
    public string AnswerGiven => _answerGiven;
    public LevelData LevelData => _levelData;

    public QuestionAnsweredEvenData(bool isCorrect, string answerGiven, LevelData levelData)
    {
        _isCorrect = isCorrect;
        _answerGiven = answerGiven;
        _levelData = levelData;
    }
}