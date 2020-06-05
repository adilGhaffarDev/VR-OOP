using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsPanel : PanelScript
{
    [SerializeField]
    List<GameObject> _levelItems;

    [SerializeField]
    Transform _parentForItems;

    public override void ShowSelf(object data)
    {
        if (data != null)
        {
            LevelsDataBase levelsDataBase = (LevelsDataBase)data;
            LevelData[] allLoadedLevels = levelsDataBase.GetAllLevelsData();
            for (int i = 0; i < allLoadedLevels.Length; i++)
            {
                if(i >= _levelItems.Count)
                {
                    GameObject levelItem = Instantiate(_levelItems[0], _parentForItems);
                    _levelItems.Add(levelItem);
                }
                _levelItems[i].GetComponentInChildren<TextMeshProUGUI>().text = "LEVEL " + (i + 1).ToString();
                int levelNumber = i;
                _levelItems[i].GetComponent<Button>().onClick.AddListener(() => LoadLevel(levelNumber));
            }
        }

        gameObject.SetActive(true);
    }

    void LoadLevel(int levelNumber)
    {
        gameObject.SetActive(false);
        EventManager.TriggerEvent(EventNames.LoadGivenLevel, (object)levelNumber);
    }

    public override void HideSelf()
    {
        gameObject.SetActive(false);
    }
}
