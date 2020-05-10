using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    LevelsDataBase _levelsData;

    ManagerContainer _managerContainer;

    GameManager _gameManager;
    PlayFabManager _playFabManager;
    SaveDataManager _saveDataManager;
    StaticDataManager _staticDataManager;

    IWorldView[] _worldViews;

    LoginResult _loginResult;

    private void OnEnable()
    {
        Cursor.visible = true;
        EventManager.StartListening(EventNames.OnUserLoggedIn, StartServices);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventNames.OnUserLoggedIn, StartServices);
    }

    void StartServices(object data)
    {
        if (data != null)
        {
            _loginResult = (LoginResult)data;
            CreateServices();
            BindServices();
            InitializeWorldViews();
            StartGame();
        }
        else
        {
            Debug.Log("login Failed");
        }
    }

    void InitializeWorldViews()
    {
        _worldViews = GameObject.FindObjectsOfType<IWorldView>();
        foreach (var item in _worldViews)
        {
            item.Initialize(_managerContainer);
        }
    }

    public void CreateServices()
    {
        _managerContainer = new ManagerContainer();
        _gameManager = new GameManager(_managerContainer);
        _playFabManager = new PlayFabManager(_managerContainer,_loginResult);
        _saveDataManager = new SaveDataManager(_managerContainer);
        _staticDataManager = new StaticDataManager(_managerContainer, _levelsData);
    }

    public void BindServices()
    {
        _managerContainer.Bind<GameManager>(_gameManager);
        _managerContainer.Bind<PlayFabManager>(_playFabManager);
        _managerContainer.Bind<SaveDataManager>(_saveDataManager);
        _managerContainer.Bind<StaticDataManager>(_staticDataManager);
        _gameManager.Load();
        _playFabManager.Load();
        _saveDataManager.Load();
        _staticDataManager.Load();
    }

    void StartGame()
    {
        if (_loginResult != null)
        {
            _loginResult.InfoResultPayload.TitleData.TryGetValue(Constants.PF_KEY_USERNAME, out string userName);
            _loginResult.InfoResultPayload.TitleData.TryGetValue(Constants.PF_KEY_SCORE, out string userScore);
            _loginResult.InfoResultPayload.TitleData.TryGetValue(Constants.PF_KEY_LEVEL, out string userLevel);

            userName = userName == null ? "user" : userName;
            userScore = userScore == null ? "0" : userScore;
            userLevel = userLevel == null ? "0" : userLevel;

            PlayerData playerData = new PlayerData(_loginResult.PlayFabId, userName, int.Parse(userScore), int.Parse(userLevel));

            EventManager.TriggerEvent(EventNames.OnUserdataLoaded, (object)playerData);
        }
        else
        {
            GameUtils.Log("Failed to userdata");
        }
    }
}
