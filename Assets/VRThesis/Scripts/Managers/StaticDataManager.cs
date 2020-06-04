
public class StaticDataManager : IManager
{
    LevelsDataBase _levelsData;

    ManagerContainer _managerContainer;

    public LevelsDataBase GetAllLevels() => _levelsData;

    public StaticDataManager(ManagerContainer managerContainer, LevelsDataBase levelsData)
    {
        _managerContainer = managerContainer;
        _levelsData = levelsData;
    }

    public void Load()
    {
        EventManager.StartListening(EventNames.OnUserdataLoaded, LoadLevelData);
        EventManager.StartListening(EventNames.LoadNextLevel, LoadLevelData);
        EventManager.StartListening(EventNames.LoadGivenLevel, LoadGivenLevel);

    }

    public void Cleanup()
    {
        EventManager.StopListening(EventNames.OnUserdataLoaded, LoadLevelData);
        EventManager.StopListening(EventNames.LoadNextLevel, LoadLevelData);
        EventManager.StopListening(EventNames.LoadGivenLevel, LoadGivenLevel);

    }

    private void LoadLevelData(object data)
    {
        if(data != null)
        {
            int levelIndex = ((PlayerData)data).GetLevel();
            LevelData levelData = _levelsData.GetLevelData(levelIndex);
            EventManager.TriggerEvent(EventNames.OnLevelLoaded,(object)levelData);
        }
        else
        {
            PlayerData playerData = _managerContainer.Resolve<SaveDataManager>().GetPlayerData();
            int levelIndex = ((PlayerData)playerData).GetLevel()+1;
            LevelData levelData = _levelsData.GetLevelData(levelIndex);
            if(levelData!=null)
            {
                EventManager.TriggerEvent(EventNames.OnLevelLoaded, (object)levelData);
                EventManager.TriggerEvent(EventNames.UpdateLevel, (object)levelIndex);
            }
            else
            {
                EventManager.TriggerEvent(EventNames.GameOver, (object)playerData);

            }
        }
    }

    void LoadGivenLevel(object data)
    {
        if (data != null)
        {
            int levelIndex = (int)data;
            LevelData levelData = _levelsData.GetLevelData(levelIndex);
            EventManager.TriggerEvent(EventNames.OnLevelLoaded, (object)levelData);
        }
        else
        {

        }
    }

}
