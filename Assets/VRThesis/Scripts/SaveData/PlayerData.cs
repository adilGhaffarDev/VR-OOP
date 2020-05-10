using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    private string _id;
    private string _name;
    private int _score;
    private int _level;

    public PlayerData(string id, string name, int score, int level)
    {
        _id = id;
        _name = name;
        _score = score;
        _level = level;
    }

    public string GetName()
    {
        return _name;
    }

    public int GetScore()
    {
        return _score;
    }

    public int GetLevel()
    {
        return _level;
    }

    public void UpdateScore(int score)
    {
        _score += score;
    }

    public void UpdateLevel(int level)
    {
        _level = level;
    }
}
