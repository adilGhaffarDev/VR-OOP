using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserInfoPanel : PanelScript
{
    [SerializeField]
    TextMeshProUGUI _userName;
    [SerializeField]
    TextMeshProUGUI _userScore;
    [SerializeField]
    TextMeshProUGUI _userLevel;

    public override void ShowSelf(object data)
    {
        if(data!=null)
        {
            base.ShowSelf(data);
            PlayerData playerData = (PlayerData)data;
            _userName.text = playerData.GetName();
            _userScore.text = playerData.GetScore().ToString();
            _userLevel.text = playerData.GetLevel().ToString();
        }
        else
        {
            GameUtils.Log("no userdata found to display");
        }
    }

    public void UpdateUI(object data)
    {
        if (data != null)
        {
            base.ShowSelf(data);
            PlayerData playerData = (PlayerData)data;
            _userName.text = playerData.GetName();
            _userScore.text = playerData.GetScore().ToString();
            _userLevel.text = playerData.GetLevel().ToString();
        }
        else
        {
            GameUtils.Log("no userdata found to display");
        }
    }
}
