using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    PlayerData _playerData;

    private void OnEnable()
    {
        EventManager.StartListening(EventNames.OnUserdataLoaded, SetUserData);
        EventManager.StartListening(EventNames.UpdateScore, UpdateUserScore);
        EventManager.StartListening(EventNames.UpdateLevel, UpdateUserLevel);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventNames.OnUserdataLoaded, SetUserData);
        EventManager.StopListening(EventNames.UpdateScore, UpdateUserScore);
        EventManager.StopListening(EventNames.UpdateLevel, UpdateUserLevel);
    }

    void SetUserData(object data)
    {
        if(data != null)
        {
            _playerData = (PlayerData)data;
        }
        else
        {
            GameUtils.Log("Userdata not found");
        }
    }

    void UpdateUserScore(object data)
    {
        if(data != null)
        {
            _playerData.UpdateScore((int)data);
        }
        else
        {
            GameUtils.Log("Score recorded is null!");
        }
    }

    void UpdateUserLevel(object data)
    {
        if (data != null)
        {
            _playerData.UpdateLevel((int)data);
        }
        else
        {
            GameUtils.Log("level recorded is null!");
        }
    }
}
