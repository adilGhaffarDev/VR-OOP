using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int _currentQuestionIndex = 0;

    string _recordedAnswer;
    List<string> _recordedAnswerList;

    LevelData _currentLevelData;
    Questions _currentQuestion;

    private void OnEnable()
    {
        EventManager.StartListening(EventNames.OnGameStart, StartGame);
        EventManager.StartListening(EventNames.OnLevelLoaded, StartLevel);
        EventManager.StartListening(EventNames.RecordWord, RecordWord);
        EventManager.StartListening(EventNames.ShowNextQuestion, NextQuestion);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventNames.OnGameStart, StartGame);
        EventManager.StartListening(EventNames.OnLevelLoaded, StartLevel);
        EventManager.StopListening(EventNames.RecordWord, RecordWord);
        EventManager.StopListening(EventNames.ShowNextQuestion, NextQuestion);

    }

    void StartGame(object data)
    {
        Debug.Log("Game Started");
        StartLevel(data);
    }

    void StartLevel(object data)
    {
        _currentLevelData = (LevelData)data;
        ShowQuestion();
    }

    void NextQuestion(object data)
    {
        _currentQuestionIndex++;
        if (_currentQuestionIndex >= _currentLevelData._questions.Length)
        {
            EventManager.TriggerEvent(EventNames.LoadNextLevel,null);
        }
        else
        {
            ShowQuestion();
        }
    }

    void ShowQuestion()
    {
        if (_currentLevelData._questions[_currentQuestionIndex] != null)
        {
            _currentQuestion = _currentLevelData._questions[_currentQuestionIndex];
            EventManager.TriggerEvent(EventNames.ShowQuestion, (object)_currentQuestion);
        }
        else
        {
            GameUtils.Log("No questions in current level!");
        }
    }

    void RecordWord(object data)
    {
        if(data != null)
        {
            string opData = (string)data;
            if (_recordedAnswerList == null)
                _recordedAnswerList = new List<string>();
            _recordedAnswerList.Add(opData);
            _recordedAnswer += " " + opData;

            if(_recordedAnswerList.Count >= _currentQuestion._options.Length)
            {
                Answered(string.Equals(_recordedAnswer, _currentQuestion._answer));
            }
        }
        else
        {
            GameUtils.Log("Error click!");
        }
    }

    void Answered(bool isCorrect)
    {
        QuestionAnsweredEvenData questionAnsweredEvenData = new QuestionAnsweredEvenData(isCorrect, _recordedAnswer, _currentLevelData);
        EventManager.TriggerEvent(EventNames.QuestionAnswered, (object)questionAnsweredEvenData);
        if (isCorrect)
        {
            EventManager.TriggerEvent(EventNames.UpdateScore,null);
        }
    }
}
