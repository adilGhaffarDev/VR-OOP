using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class HelperUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _helperText;

    LevelData _currentLevelData;

    Question _currentQuestion;

    int _speechIndex = 0;

    //bool inBetweenLevel = false;

    int _speechIndexQ = 0;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
      
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventNames.QuestionAnswered, OnQuestionAnswered);
        EventManager.StartListening(EventNames.OnLevelLoaded, ShowStartLevelSpeechBubbles);
        EventManager.StartListening(EventNames.ShowQuestion, OnNewQuestionLoaded);

    }

    private void OnDisable()
    {
        EventManager.StopListening(EventNames.QuestionAnswered, OnQuestionAnswered);
        EventManager.StartListening(EventNames.OnLevelLoaded, ShowStartLevelSpeechBubbles);
        EventManager.StopListening(EventNames.ShowQuestion, OnNewQuestionLoaded);

    }

    void OnQuestionAnswered(object data)
    {
        //inBetweenLevel = true;
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
                _helperText.text = _currentLevelData._errorLevelText._speeches[0];
                StartCoroutine(DownloadAndPlaySpeech(_currentLevelData._errorLevelText._speeches[0]));
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
                _helperText.text = _currentLevelData._errorLevelText._speeches[0];
                StartCoroutine(DownloadAndPlaySpeech(_currentLevelData._errorLevelText._speeches[0]));
            }
        }
        
    }

    void ShowStartLevelSpeechBubbles(object data)
    {
        //inBetweenLevel = false;
        _currentLevelData = (LevelData)data;
        _speechIndex = 0;
        //ShowNextSpeechBubble();
    }

    public void ShowNextSpeechBubble()
    {
       
        //if (!inBetweenLevel && !_audioSource.isPlaying)
        //{

        //    if (_currentLevelData._startLevelText._speeches != null && _currentLevelData._startLevelText._speeches.Length > 0)
        //    {
        //        if (_speechIndex >= _currentLevelData._startLevelText._speeches.Length)
        //        {
        //            _speechIndex = 0;
        //        }
        //        _helperText.text = _currentLevelData._startLevelText._speeches[_speechIndex];
        //        StartCoroutine(DownloadAndPlaySpeech(_currentLevelData._startLevelText._speeches[_speechIndex]));

        //        _speechIndex++;
        //    }
        //}
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
        //if (!_audioSource.isPlaying)
        {
            if (data is Question)
            {
                 _currentQuestion = (Question)data;
                if (_currentQuestion != null)
                {
                    ShowNextSpeechBubble();
                    //foreach (var item in _currentQuestion._questionSpeech._speeches)
                    //{
                    //    _helperText.text = item;
                    //    StartCoroutine(DownloadAndPlaySpeech(item));
                    //}
                }
                else
                {
                    GameUtils.Log("No questions in current level!");
                }
            }
        }
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

    
}
