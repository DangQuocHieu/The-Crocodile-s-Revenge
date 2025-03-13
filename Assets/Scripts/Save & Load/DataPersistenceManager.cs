using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : Singleton<DataPersistenceManager>
{
    [SerializeField] string fileName;
    GameData gameData;
    List<IDataPersistence> dataPersistanceObjects;
    FileDataHandler fileDataHandler;
    [SerializeField] bool isEncrypted;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, isEncrypted);    
        this.dataPersistanceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>().ToList();
        LoadGame();
    }

   
    void Update()
    {
        
    }

    public void LoadGame()
    {
        gameData = fileDataHandler.Load();
        if(gameData == null)
        {
            gameData = new GameData();
        }
        foreach(var dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (var dataPersistanceObj in dataPersistanceObjects)  
        {
            if(dataPersistanceObj != null)
            dataPersistanceObj.SaveData(gameData);
        }
        fileDataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

}
