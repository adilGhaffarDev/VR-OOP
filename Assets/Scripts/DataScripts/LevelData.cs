using UnityEngine;
using System;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/CreateLevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public Questions[] _questions;

}
[Serializable]
public class Questions
{
    public string[] _options;
    public string _answer;
}