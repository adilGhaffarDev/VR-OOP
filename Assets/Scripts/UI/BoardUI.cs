using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoardUI : MonoBehaviour
{
    [SerializeField]
    GameObject _optionPrefab;

    [SerializeField]
    TextMeshProUGUI _optionsParent;

    [SerializeField]
    TextMeshProUGUI _answerPan;

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
        Questions question = (Questions)data;
        if (question != null)
        {
            for (int i = 0; i < question._options.Length; i++)
            {

            }
        }
        else
        {
            GameUtils.Log("No questions in current level!");
        }
    }
}
