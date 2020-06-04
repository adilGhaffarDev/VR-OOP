using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayUI : ICanvasUI
{
    [SerializeField]
    UserInfoPanel _userInfoPanel;

    private void OnEnable()
    {
        EventManager.StartListening(EventNames.OnUserdataLoaded, SetUserDataUI);
        EventManager.StartListening(EventNames.UpdateUserInfoUI, UpdateUI);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventNames.OnUserdataLoaded, SetUserDataUI);
        EventManager.StartListening(EventNames.UpdateUserInfoUI, UpdateUI);
    }

    void SetUserDataUI(object data)
    {
        _userInfoPanel.ShowSelf(data);
    }

    void UpdateUI(object data)
    {
        _userInfoPanel.UpdateUI(data);
    }
}
