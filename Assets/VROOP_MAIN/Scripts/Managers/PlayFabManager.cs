using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabManager : IManager
{
    LoginResult _loginResult;
    ManagerContainer _managerContainer;

    public PlayFabManager(ManagerContainer managerContainer,LoginResult loginResult)
    {
        _managerContainer = managerContainer;
        _loginResult = loginResult;
    }

    public void Load()
    {
    }

    public void Cleanup()
    {
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
        if(data is QuestionAnsweredEvenData)
        {
            QuestionAnsweredEvenData questionAnsweredEvenData = data as QuestionAnsweredEvenData;
            if (questionAnsweredEvenData.IsCorrect)
            {

            }
            else
            {

            }
        }
        else
        {
            ExQuestionAnsweredEvenData questionAnsweredEvenData = data as ExQuestionAnsweredEvenData;
            if (questionAnsweredEvenData.IsCorrect)
            {

            }
            else
            {

            }
        }
        
    }
}
