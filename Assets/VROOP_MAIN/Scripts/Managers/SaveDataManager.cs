
public class SaveDataManager : IManager
{
    PlayerData _playerData;
    ManagerContainer _managerContainer;

    public SaveDataManager(ManagerContainer managerContainer)
    {
        _managerContainer = managerContainer;
    }

    public void Load()
    {

        EventManager.StartListening(EventNames.OnUserdataLoaded, SetUserData);
        EventManager.StartListening(EventNames.QuestionAnswered, UpdateUserScore);
        EventManager.StartListening(EventNames.UpdateLevel, UpdateUserLevel);
    }

    public void Cleanup()
    {
        EventManager.StopListening(EventNames.OnUserdataLoaded, SetUserData);
        EventManager.StopListening(EventNames.QuestionAnswered, UpdateUserScore);
        EventManager.StopListening(EventNames.UpdateLevel, UpdateUserLevel);
    }

    void SetUserData(object data)
    {
        if(data != null)
        {
            _playerData = (PlayerData)data;
        }
        else
        {
            GameUtils.Log("Userdata not found");
        }
    }

    void UpdateUserScore(object data)
    {
        if(data != null)
        {
            if(data is QuestionAnsweredEvenData)
            {
                QuestionAnsweredEvenData questionAnsweredEvenData = (QuestionAnsweredEvenData)data;
                if (questionAnsweredEvenData.IsCorrect)
                {
                    _playerData.UpdateScore(Constants.ScorePerAnswer);
                    EventManager.TriggerEvent(EventNames.UpdateUserInfoUI, (object)_playerData);

                    _managerContainer.Resolve<PlayFabManager>().SetUserData(Constants.PF_KEY_SCORE, _playerData.GetScore().ToString());
                }
            }
            else
            {
                ExQuestionAnsweredEvenData questionAnsweredEvenData = (ExQuestionAnsweredEvenData)data;
                if (questionAnsweredEvenData.IsCorrect)
                {
                    _playerData.UpdateScore(Constants.ScorePerAnswer*2);
                    EventManager.TriggerEvent(EventNames.UpdateUserInfoUI, (object)_playerData);

                    _managerContainer.Resolve<PlayFabManager>().SetUserData(Constants.PF_KEY_SCORE, _playerData.GetScore().ToString());
                }
            }
            
        }
        else
        {
            GameUtils.Log("answer recorded is null!");
        }
    }

    void UpdateUserLevel(object data)
    {
        if (data != null)
        {
            _playerData.UpdateLevel((int)data);
            EventManager.TriggerEvent(EventNames.UpdateUserInfoUI,(object)_playerData);
            _managerContainer.Resolve<PlayFabManager>().SetUserData(Constants.PF_KEY_LEVEL, _playerData.GetLevel().ToString());
        }
        else
        {
            GameUtils.Log("level recorded is null!");
        }
    }

    public PlayerData GetPlayerData()
    {
        return _playerData;
    }
}
