using UnityEngine;

public class DataPersistenceManager : Singleton<DataPersistenceManager>
{
    private GameData gameData;
    public GameData GameData => gameData;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        Observer.AddObserver(GameEvent.OnPlayerUpgradePowerup, OnUpgrade);
    }
    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnPlayerUpgradePowerup, OnUpgrade);
    }
    
    public void LoadGame(GameData gameData)
    {
        this.gameData = gameData; 
    }

    public void OnUpgrade(object[] datas)
    {
        PowerupType type = (PowerupType)datas[0];
        switch (type)
        {
            case PowerupType.Shield:
                ++gameData.shieldLevel;
                break;
            case PowerupType.DoubleCoin:
                ++gameData.doublecoinLevel;
                break;
            case PowerupType.Magnet:
                ++gameData.magnetLevel;
                break;
            case PowerupType.TripleJump:
                ++gameData.tripleJumpLevel;
                break;
        }
        PlayFabManager.Instance.SavePlayerData();
    }

}
