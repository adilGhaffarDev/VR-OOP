using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDataManager : MonoBehaviour
{
    [SerializeField]
    LevelsDataBase _levelsData;

    private void OnEnable()
    {
        EventManager.StartListening(EventNames.OnUserdataLoaded, LoadLevelData);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventNames.OnUserdataLoaded, LoadLevelData);
    }

    private void LoadLevelData(object data)
    {
        if(data != null)
        {
            int levelIndex = ((PlayerData)data).GetLevel();
            LevelData levelData = _levelsData.GetLevelData(levelIndex);
            EventManager.TriggerEvent(EventNames.OnGameStart,(object)levelData);
        }
        else
        {
            GameUtils.Log("Userdata not found");
        }
    }
}
