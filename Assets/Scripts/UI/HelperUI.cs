using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelperUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _helperText;

    private void OnEnable()
    {
        EventManager.StartListening(EventNames.QuestionAnswered, OnQuestionAnswered);

    }

    private void OnDisable()
    {
        EventManager.StopListening(EventNames.QuestionAnswered, OnQuestionAnswered);

    }

    void OnQuestionAnswered(object data)
    {
        QuestionAnsweredEvenData questionAnsweredEvenData = data as QuestionAnsweredEvenData;
        if(questionAnsweredEvenData.IsCorrect)
        {
            _helperText.text = "Correct answer!";
        }
        else
        {
            _helperText.text = "Wrong answer, Try again!";
        }
    }
}
