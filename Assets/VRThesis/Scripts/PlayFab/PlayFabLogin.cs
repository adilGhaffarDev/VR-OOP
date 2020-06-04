using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
#if UNITY_EDITOR
    public static readonly string PlayerIdKey = "PlayerId";
    public static string CustomId = "";
#endif

    void Awake()
    {
        EventManager.TriggerEvent(EventNames.ShowLoadingScreen, null);
#if UNITY_EDITOR
        if (!PlayerPrefs.HasKey(PlayerIdKey))
        {
            PlayerPrefs.SetString(PlayerIdKey, System.Guid.NewGuid().ToString());
            PlayerPrefs.Save();
        }
        var playerId = PlayerPrefs.GetString(PlayerIdKey);
        CustomId = playerId;
        Debug.Log("Player Editor ID " + playerId);
        var request = new LoginWithCustomIDRequest { CustomId = playerId, CreateAccount = true, InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetTitleData = true, GetUserData  = true} };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);

#elif UNITY_IOS
		var request = new LoginWithIOSDeviceIDRequest
		{
			CreateAccount = true, DeviceId = SystemInfo.deviceUniqueIdentifier, DeviceModel = SystemInfo.deviceModel,
			OS = SystemInfo.operatingSystem,
			InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetTitleData = true , GetUserData  = true}
		};
		PlayFabClientAPI.LoginWithIOSDeviceID(request, OnLoginSuccess, OnLoginFailure);

#elif UNITY_ANDROID
        var request = new LoginWithAndroidDeviceIDRequest
		{
			CreateAccount = true, AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
			AndroidDevice = SystemInfo.deviceModel, OS = SystemInfo.operatingSystem,
			InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetTitleData = true, GetUserData  = true }
		};
	    PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginSuccess, OnLoginFailure);
#else
		Debug.Log("PlayFab: Unsupported platform");
#endif
    }

    private void OnLoginSuccess(LoginResult result)
    {
        EventManager.TriggerEvent(EventNames.HideLoadingScreen, null);

        EventManager.TriggerEvent(EventNames.OnUserLoggedIn, result);
        GameUtils.Log("Congratulations, you made your first successful API call!");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        EventManager.TriggerEvent(EventNames.HideLoadingScreen, null);

        GameUtils.Log("Something went wrong with your first API call.  :(");
        GameUtils.Log("Here's some debug information:");
        GameUtils.Log(error.GenerateErrorReport());
    }
}
