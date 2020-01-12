using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayUI : MonoBehaviour
{
    [SerializeField]
    LoadingPanel _loadingPanel;

    [SerializeField]
    UserInfoPanel _userInfoPanel;

    private void Awake()
    {
        _loadingPanel.ShowSelf(null);
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventNames.OnUserdataLoaded, SetUserDataUI);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventNames.OnUserdataLoaded, SetUserDataUI);
    }

    void SetUserDataUI(object data)
    {
        _loadingPanel.HideSelf();
        _userInfoPanel.ShowSelf(data);
    }
}
