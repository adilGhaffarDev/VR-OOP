using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardUI : MonoBehaviour
{
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

    }

    private void OnDisable()
    {
        EventManager.StopListening(EventNames.ShowQuestion, SetBoard);
        EventManager.StopListening(EventNames.QuestionAnswered, OnQuestionAnswered);

    }

    void SetBoard(object data)
    {
        ClearBoard();
        Questions question = (Questions)data;
        if (question != null)
        {
            for (int i = 0; i < question._options.Length; i++)
            {
                GameObject opGo = Instantiate(_optionPrefab,_optionsParent);
                string opString = question._options[i];
                opGo.GetComponent<TextMeshProUGUI>().text = opString;
                opGo.GetComponent<Button>().onClick.AddListener(() => OnOptionClick(opString));

                _options.Add(question._options[i], opGo);
            }
        }
        else
        {
            GameUtils.Log("No questions in current level!");
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

    void OnQuestionAnswered(object data)
    {
        QuestionAnsweredEvenData questionAnsweredEvenData = data as QuestionAnsweredEvenData;
        if (questionAnsweredEvenData.IsCorrect)
        {
            _answerPan.text = "";
        }
        else
        {
            _answerPan.text = "Loading Next Question..";
            Invoke("LoadNextQuestion", 2);
        }
    }

    void LoadNextQuestion()
    {
        EventManager.TriggerEvent(EventNames.ShowNextQuestion,null);
    }
}
