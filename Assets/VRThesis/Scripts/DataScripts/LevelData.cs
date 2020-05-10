using UnityEngine;
using System;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/CreateLevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public HelperAgentText _startLevelText;
    public HelperAgentText _errorLevelText;
    public HelperAgentText _endLevelText;

    public Question[] _questions;

    public Exercise _exercise;
}
[Serializable]
public class Question
{
    public string[] _options;
    public string _answer;
    public string _action;
    public string _actor;
    public ObjectToInstantiate[] _objectsToBeInstantiated;
    public HelperAgentText _questionSpeech;
}


[Serializable]
public class HelperAgentText
{
    public string[] _speeches;
}

[Serializable]
public class ObjectToInstantiate
{
    public GameObject _prefab;
    public Vector3 _position;
    public Vector3 _scale = Vector3.one;
    public Vector3 _rotation;
}

[Serializable]
public class Exercise
{
    public HelperAgentText _excerciseIntro;
    public ExerciseQuestion[] _excerciseQuestions;
}

[Serializable]
public class ExerciseQuestion
{
    public string _question;
    public string[] _options;
    public int _answer;
}

