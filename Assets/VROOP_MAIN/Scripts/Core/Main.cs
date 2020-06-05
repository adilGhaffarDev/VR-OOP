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
    ICanvasUI[] _canvasViews;

    LoginResult _loginResult;

    private void OnEnable()
    {
        Cursor.visible = true;
        EventManager.StartListening(EventNames.OnUserLoggedIn, StartServices);
        EventManager.StartListening(EventNames.OnGameStart, StartGame);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventNames.OnUserLoggedIn, StartServices);
        EventManager.StopListening(EventNames.OnGameStart, StartGame);
    }

    void StartServices(object data)
    {
        if (data != null)
        {
            _loginResult = (LoginResult)data;
            CreateServices();
            BindServices();
            InitializeWorldViews();
            InitializeUI();
        }
        else
        {
            Debug.Log("login Failed");
        }
    }

    void InitializeUI()
    {
        _canvasViews = GameObject.FindObjectsOfType<ICanvasUI>();
        foreach (var item in _canvasViews)
        {
            item.Initialize(_managerContainer);
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

    void StartGame(object data)
    {
        if (_loginResult != null)
        {
            _loginResult.InfoResultPayload.UserData.TryGetValue(Constants.PF_KEY_USERNAME, out UserDataRecord userName);
            _loginResult.InfoResultPayload.UserData.TryGetValue(Constants.PF_KEY_SCORE, out UserDataRecord userScore);
            _loginResult.InfoResultPayload.UserData.TryGetValue(Constants.PF_KEY_LEVEL, out UserDataRecord userLevel);

            string userNameS = userName == null ? "user" : userName.Value;
            string userScoreS = userScore == null ? "0" : userScore.Value;
            string userLevelS = userLevel == null ? "0" : userLevel.Value;

            PlayerData playerData = new PlayerData(_loginResult.PlayFabId, userNameS, int.Parse(userScoreS), int.Parse(userLevelS));

            EventManager.TriggerEvent(EventNames.OnUserdataLoaded, (object)playerData);
        }
        else
        {
            GameUtils.Log("Failed to userdata");
        }
    }
}
