using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BoardUI : ICanvasUI
{
    [SerializeField]
    Button _startButton;

    [SerializeField]
    Button _restartGameButton;

    [SerializeField]
    Button _quitGameButton;

    [SerializeField]
    Button _levelsButton;

    [SerializeField]
    Button _resetAnswerButton;

    [SerializeField]
    LoadingPanel _loadingScreen;

    [SerializeField]
    LevelsPanel _levelScreen;

    [SerializeField]
    GameObject _optionPrefab;

    [SerializeField]
    Transform _optionsParent;

    [SerializeField]
    TextMeshProUGUI _answerPan;

    Dictionary<string, GameObject> _options;

    private void OnEnable()
    {
        EventManager.StartListening(EventNames.ShowQuestion, SetBoard);
        EventManager.StartListening(EventNames.QuestionAnswered, OnQuestionAnswered);
        EventManager.StartListening(EventNames.GameOver, GameOver);
        EventManager.StartListening(EventNames.ShowLoadingScreen, ShowLoadingScreen);
        EventManager.StartListening(EventNames.HideLoadingScreen, HideLoadingScreen);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventNames.ShowQuestion, SetBoard);
        EventManager.StopListening(EventNames.QuestionAnswered, OnQuestionAnswered);
        EventManager.StopListening(EventNames.GameOver, GameOver);
        EventManager.StopListening(EventNames.ShowLoadingScreen, ShowLoadingScreen);
        EventManager.StopListening(EventNames.HideLoadingScreen, HideLoadingScreen);
    }

    private void Start()
    {
        AddButtonListeners();
    }

    void AddButtonListeners()
    {
        _resetAnswerButton.onClick.AddListener(() => OnResetAnswer());
        _startButton.onClick.AddListener(() => StartGame());
        _restartGameButton.onClick.AddListener(() => RestartGame());
        _quitGameButton.onClick.AddListener(() => QuitGame());
        _levelsButton.onClick.AddListener(() => ShowLevelScreen());
    }

    void ShowLevelScreen()
    {
        _levelScreen.ShowSelf(_managerContainer.Resolve<StaticDataManager>().GetAllLevels());
    }

    void RestartGame()
    {
        //SceneManager.LoadScene("GamePlay");
        EventManager.TriggerEvent(EventNames.LoadGivenLevel, 0);
    }

    void QuitGame()
    {
        Application.Quit();
    }

    void OnResetAnswer()
    {
        _answerPan.text = "";
        EventManager.TriggerEvent(EventNames.ClearAnswer, null);
    }

    void StartGame()
    {
        EventManager.TriggerEvent(EventNames.OnGameStart, null);
    }

    void SetBoard(object data)
    {
        ClearBoard();
        if(data is Question)
        {
            Question question = (Question)data;
            if (question != null)
            {
                for (int i = 0; i < question._options.Length; i++)
                {
                    GameObject opGo = Instantiate(_optionPrefab, _optionsParent);
                    opGo.name = "option" + i.ToString();

                    string opString = question._options[i];
                    opGo.GetComponentInChildren<TextMeshProUGUI>().text = opString;
                    opGo.GetComponent<Button>().onClick.AddListener(() => OnOptionClick(opString));

                    _options.Add(question._options[i], opGo);
                }
                _answerPan.text = "";

            }
            else
            {
                GameUtils.Log("No questions in current level!");
            }
        }
        else if(data is ExerciseQuestion)
        {
            ExerciseQuestion question = (ExerciseQuestion)data;
            if (question != null)
            {
                for (int i = 0; i < question._options.Length; i++)
                {
                    GameObject opGo = Instantiate(_optionPrefab, _optionsParent);
                    opGo.name = "option" + i.ToString();
                    string opString = question._options[i];
                    opGo.GetComponentInChildren<TextMeshProUGUI>().text = opString;
                    int ind = i;
                    opGo.GetComponent<Button>().onClick.AddListener(() => OnOptionClick(ind));

                    _options.Add(question._options[i], opGo);
                }
                _answerPan.text = question._question;

            }
            else
            {
                GameUtils.Log("No questions in current level!");
            }
        }
    }

    void ClearBoard()
    {
        if (_options == null)
        {
            _options = new Dictionary<string, GameObject>();
            return;
        }
        foreach (var item in _options.Values)
        {
            Destroy(item);
        }
        _options.Clear();
    }

    void OnOptionClick(string opString)
    {
        _answerPan.text += " " + opString;
        EventManager.TriggerEvent(EventNames.RecordWord, (object)opString);
    }

    void OnOptionClick(int opString)
    {
        EventManager.TriggerEvent(EventNames.RecordWord, (object)opString);
    }

    void OnQuestionAnswered(object data)
    {
        if(data is QuestionAnsweredEvenData)
        {
            QuestionAnsweredEvenData questionAnsweredEvenData = data as QuestionAnsweredEvenData;
            if (!questionAnsweredEvenData.IsCorrect)
            {
                _answerPan.text = "Wrong Answer!";
                Invoke("EmptyAnswerString", 1);
            }
            else
            {
                StartCoroutine("LoadNextQuestion");
            }
        }
        else
        {
            ExQuestionAnsweredEvenData questionAnsweredEvenData = data as ExQuestionAnsweredEvenData;
            if (!questionAnsweredEvenData.IsCorrect)
            {
                _answerPan.text = "Wrong Answer!";
            }
            else
            {
                StartCoroutine("LoadNextQuestion");
            }
        }
    }

    IEnumerator LoadNextQuestion()
    {
        yield return new WaitForSeconds(2);

        _answerPan.text = "Loading Next Question..";

        yield return new WaitForSeconds(1);

        _answerPan.text = "";

        EventManager.TriggerEvent(EventNames.ShowNextQuestion,null);
    }

    void EmptyAnswerString()
    {
        _answerPan.text = "";
    }

    void GameOver(object data)
    {
        _answerPan.text = "Game Over!!";
    }

    void ShowLoadingScreen(object data)
    {
        _loadingScreen.ShowSelf(null);
    }

    void HideLoadingScreen(object data)
    {
        _loadingScreen.HideSelf();
    }
}
