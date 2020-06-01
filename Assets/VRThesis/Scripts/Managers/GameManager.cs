using System.Collections.Generic;
using UnityEngine;

public class GameManager : IManager
{
    int _currentQuestionIndex = -1;
    int _currentExQuestionIndex = -1;

    string _recordedAnswer;
    List<string> _recordedAnswerList;

    LevelData _currentLevelData;
    Question _currentQuestion;
    ExerciseQuestion _currentExQuestion;

    ManagerContainer _managerContainer;

    public GameManager(ManagerContainer managerContainer)
    {
        _managerContainer = managerContainer;
    }

    public void Load()
    {
        EventManager.StartListening(EventNames.OnLevelLoaded, StartLevel);
        EventManager.StartListening(EventNames.RecordWord, RecordWord);
        EventManager.StartListening(EventNames.ShowNextQuestion, NextQuestion);
    }

    public void Cleanup()
    {
        EventManager.StopListening(EventNames.OnLevelLoaded, StartLevel);
        EventManager.StopListening(EventNames.RecordWord, RecordWord);
        EventManager.StopListening(EventNames.ShowNextQuestion, NextQuestion);
    }

    void StartLevel(object data)
    {
        _currentQuestionIndex = 0;
        _currentExQuestionIndex = -1;
        _currentLevelData = (LevelData)data;
        Debug.Log("Level Started :: "+ _currentLevelData.name);
        ShowQuestion();
    }

    void NextQuestion(object data)
    {
        if (_currentQuestionIndex >= _currentLevelData._questions.Length)
        {
            LoadExcercise();
        }
        else
        {
            ShowQuestion();
        }
    }

    void LoadExcercise()
    {
        _currentExQuestionIndex++;

        if (_currentExQuestionIndex >= _currentLevelData._exercise._excerciseQuestions.Length)
        {
            LoadNextLevel();
        }
        else
        {
            ShowExQuestion();
        }
    }

    void ShowExQuestion()
    {
        if (_currentLevelData._exercise._excerciseQuestions[_currentExQuestionIndex] != null)
        {
            _currentExQuestion = _currentLevelData._exercise._excerciseQuestions[_currentExQuestionIndex];
            EventManager.TriggerEvent(EventNames.ShowQuestion, (object)_currentExQuestion);
        }
        else
        {
            GameUtils.Log("No questions in current level!");
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
            if (data is string opData) // if data is string the recorded word is coming from level questions
            {
                if (_recordedAnswerList == null)
                    _recordedAnswerList = new List<string>();
                _recordedAnswerList.Add(opData);
                _recordedAnswer += opData;

                if (string.Equals(_recordedAnswer, _currentQuestion._answer))
                {
                    Answered(string.Equals(_recordedAnswer, _currentQuestion._answer), false);
                }
                else
                {
                    if (_recordedAnswerList.Count >= _currentQuestion._options.Length)
                    {
                        Answered(string.Equals(_recordedAnswer, _currentQuestion._answer), false);
                    }
                }
            }
            else if (data is int opData1)
            {
                Answered(opData1 == _currentExQuestion._answer, true);
            }
        }
        else
        {
            GameUtils.Log("Error click!");
        }
    }

    void Answered(bool isCorrect,bool isExerciseQ)
    {
        if(isExerciseQ)
        {
            ExQuestionAnsweredEvenData questionAnsweredEvenData = new ExQuestionAnsweredEvenData(isCorrect, _currentLevelData, _currentExQuestion,_recordedAnswerList);
            EventManager.TriggerEvent(EventNames.QuestionAnswered, (object)questionAnsweredEvenData);
        }
        else
        {
            QuestionAnsweredEvenData questionAnsweredEvenData = new QuestionAnsweredEvenData(isCorrect, _recordedAnswer, _currentLevelData, _currentQuestion, _recordedAnswerList);
            EventManager.TriggerEvent(EventNames.QuestionAnswered, (object)questionAnsweredEvenData);
            ClearRecordedAnswer();
            if(isCorrect)
            {
                _currentQuestionIndex++;
            }
        }

    }

    void ClearRecordedAnswer()
    {
        _recordedAnswerList.Clear();
        _recordedAnswer = "";
    }

    void LoadNextLevel()
    {
        EventManager.TriggerEvent(EventNames.LoadNextLevel,null);
    }
}
