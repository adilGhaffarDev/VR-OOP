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
    }

    private void OnDisable()
    {
        EventManager.StartListening(EventNames.ShowQuestion, SetBoard);
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

                opGo.GetComponent<TextMeshProUGUI>().text = question._options[i];
                opGo.GetComponent<Button>().onClick.AddListener(() => OnOptionClick(question._options[i]) );

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
}
