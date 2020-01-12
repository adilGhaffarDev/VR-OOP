using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataBase", menuName = "ScriptableObjects/CreateLevelDataBase", order = 1)]
public class LevelsDataBase : ScriptableObject
{
    LevelData[] _levels;

    public LevelData[] GetAllLevelsData()
    {
        if (_levels.Length > 0)
        {
            return _levels;
        }
        else
            return null;
    }

    public LevelData GetLevelData(int index)
    {
        if (index >= _levels.Length)
        {
            return null;
        }
        else
        {
            return _levels[index];
        }
    }

    public int GetLevelsCount()
    {
        return _levels.Length;
    }
}
