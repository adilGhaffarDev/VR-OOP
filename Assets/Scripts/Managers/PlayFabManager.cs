using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabManager : MonoBehaviour
{
    LoginResult _loginResult;

    private void OnEnable()
    {
        EventManager.StartListening(EventNames.OnUserLoggedIn, GetUserData);
        EventManager.StartListening(EventNames.QuestionAnswered, UpdateUserProgress);

    }

    private void OnDisable()
    {
        EventManager.StopListening(EventNames.OnUserLoggedIn, GetUserData);
        EventManager.StopListening(EventNames.QuestionAnswered, UpdateUserProgress);

    }

    void GetUserData(object data)
    {
        if(data != null)
        {
            _loginResult = (LoginResult)data;

            _loginResult.InfoResultPayload.TitleData.TryGetValue(Constants.PF_KEY_USERNAME, out string userName);
            _loginResult.InfoResultPayload.TitleData.TryGetValue(Constants.PF_KEY_SCORE, out string userScore);
            _loginResult.InfoResultPayload.TitleData.TryGetValue(Constants.PF_KEY_LEVEL, out string userLevel);

            userName = userName == null ? "user" : userName;
            userScore = userScore == null ? "0" : userScore;
            userLevel = userLevel == null ? "0" : userLevel;

            PlayerData playerData = new PlayerData(_loginResult.PlayFabId, userName, int.Parse(userScore), int.Parse(userLevel));

            EventManager.TriggerEvent(EventNames.OnUserdataLoaded,(object)playerData);
        }
        else
        {
            GameUtils.Log("Failed to userdata");
        }
    }

    public void GetUserData(Action<string, string> _onSuccess, string key)
    {
        if (_loginResult == null)
            return;
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = _loginResult.PlayFabId,
            Keys = null
        }, result =>
        {
            if (result.Data == null || !result.Data.ContainsKey(key)) _onSuccess(key, "false");
            else _onSuccess(key, result.Data[key].Value);
        }, (error) =>
        {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public void SetUserData(string key, string value)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string> {
                { key, value },
            },
        },
        result => Debug.Log("Successfully updated user data"),
        error =>
        {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    void UpdateUserProgress(object data)
    {
        QuestionAnsweredEvenData questionAnsweredEvenData = data as QuestionAnsweredEvenData;
        if (questionAnsweredEvenData.IsCorrect)
        {

        }
        else
        {

        }
    }
}
