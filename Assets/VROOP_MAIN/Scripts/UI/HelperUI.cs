using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class HelperUI : ICanvasUI
{
    [SerializeField]
    TextMeshProUGUI _helperText;

    [SerializeField]
    GameObject _agent;

    LevelData _currentLevelData;

    Question _currentQuestion;

    HelperServer _helperServer;

    int _speechIndex = 0;

    int _speechIndexQ = 0;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _helperServer = GetComponent<HelperServer>();
        _helperServer._chatCallBack = OnResponseSuccessFromChatBot;
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventNames.QuestionAnswered, OnQuestionAnswered);
        EventManager.StartListening(EventNames.OnLevelLoaded, ShowStartLevelSpeechBubbles);
        EventManager.StartListening(EventNames.ShowQuestion, OnNewQuestionLoaded);
        EventManager.StartListening(EventNames.OnGameStart, ShowAgent);

    }

    private void OnDisable()
    {
        EventManager.StopListening(EventNames.QuestionAnswered, OnQuestionAnswered);
        EventManager.StartListening(EventNames.OnLevelLoaded, ShowStartLevelSpeechBubbles);
        EventManager.StopListening(EventNames.ShowQuestion, OnNewQuestionLoaded);
        EventManager.StopListening(EventNames.OnGameStart, ShowAgent);

    }
    void ShowAgent(object data)
    {
        _agent.SetActive(true);
    }
    void OnQuestionAnswered(object data)
    {
        if (data is QuestionAnsweredEvenData)
        {
            QuestionAnsweredEvenData questionAnsweredEvenData = data as QuestionAnsweredEvenData;
            if (questionAnsweredEvenData.IsCorrect)
            {
                _helperText.text = _currentLevelData._endLevelText._speeches[0];
                StartCoroutine(DownloadAndPlaySpeech(_currentLevelData._endLevelText._speeches[0]));
            }
            else
            {

                //_helperText.text = _currentLevelData._errorLevelText._speeches[0];
                //StartCoroutine(DownloadAndPlaySpeech(_currentLevelData._errorLevelText._speeches[0]));
                _helperServer.GetHelperResponse(PrepareErrorAnswerForServer(questionAnsweredEvenData.RecodedAnswerList));
            }
        }
        else
        {
            ExQuestionAnsweredEvenData questionAnsweredEvenData = data as ExQuestionAnsweredEvenData;
            if (questionAnsweredEvenData.IsCorrect)
            {
                _helperText.text = _currentLevelData._endLevelText._speeches[0];
                StartCoroutine(DownloadAndPlaySpeech(_currentLevelData._endLevelText._speeches[0]));
            }
            else
            {
                //_helperText.text = _currentLevelData._errorLevelText._speeches[0];
                //StartCoroutine(DownloadAndPlaySpeech(_currentLevelData._errorLevelText._speeches[0]));
                _helperServer.GetHelperResponse(PrepareErrorAnswerForServer(questionAnsweredEvenData.RecodedAnswerList));

            }
        }
    }

    void ShowStartLevelSpeechBubbles(object data)
    {
        _currentLevelData = (LevelData)data;
        _speechIndex = 0;
    }

    public void ShowNextSpeechBubble()
    {
        if (_currentQuestion != null)
        {
            if (!_audioSource.isPlaying)
            {
                if (_currentQuestion._questionSpeech._speeches != null && _currentQuestion._questionSpeech._speeches.Length > 0)
                {
                    if (_speechIndexQ >= _currentQuestion._questionSpeech._speeches.Length)
                    {
                        _speechIndexQ = 0;
                    }
                    _helperText.text = _currentQuestion._questionSpeech._speeches[_speechIndexQ];
                    StartCoroutine(DownloadAndPlaySpeech(_currentQuestion._questionSpeech._speeches[_speechIndexQ]));
                    _speechIndexQ++;
                }
            }
        }
    }

    void OnNewQuestionLoaded(object data)
    {
        _speechIndexQ = 0;
        StartCoroutine(OnNewQuestionLoadedRout(data));
    }

    IEnumerator OnNewQuestionLoadedRout(object data)
    {
        yield return new WaitForSeconds(2);
        if (data is Question)
        {
            _currentQuestion = (Question)data;
            if (_currentQuestion != null)
            {
                ShowNextSpeechBubble();
            }
            else
            {
                GameUtils.Log("No questions in current level!");
            }
        }
    }

    void OnResponseSuccessFromChatBot(string response)
    {
        _helperText.text = response;
        StartCoroutine(DownloadAndPlaySpeech(response));
    }

    string voiceRSSapiKey = "39c2b2b055494503a9ac6cce0ebce648";
    AudioSource _audioSource;
    IEnumerator DownloadAndPlaySpeech(string text)
    {
        if (!_audioSource.isPlaying)
        {

            string url = "http://api.voicerss.org/?key=" + voiceRSSapiKey + "&hl=en-us&src=" + text;

            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    _audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
                    _audioSource.Play();
                }
            }
        }
    }

    string PrepareErrorAnswerForServer(List<string> ans)
    {
        string returnString = "";
        foreach (var s in ans)
        {
            returnString += s + " ";
        }
        return returnString;
    }
}
