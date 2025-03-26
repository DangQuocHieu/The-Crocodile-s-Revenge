using System.Collections;
using UnityEngine;

public class DataPersistenceManager : Singleton<DataPersistenceManager>
{
    private GameData gameData = new GameData();
    public GameData GameData => gameData;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    
    public void LoadGame(GameData gameData)
    {
        this.gameData = gameData; 
    }

    public void Upgrade(PowerupType type)
    {
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
    }

}
