using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayFabManager : Singleton<PlayFabManager>
{
    [SerializeField] TextMeshProUGUI currentPlayerText;
    private string currentId = "";
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        Observer.AddObserver(GameEvent.OnPlayerLogin, OnPlayerLogin);

    }

    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnPlayerLogin, OnPlayerLogin);
    }
    private void Update()
    {
        if(currentPlayerText != null)
        currentPlayerText.text = "CURRENT USER: "+ currentId.ToString();
    }


    public void OnPlayerLogin(object[] datas)
    {
        string username = (string)datas[0];
        string password = (string)datas[1];
        Login(username, password);
    }
    public void GetUser()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result =>
        {
            currentId = result.AccountInfo.PlayFabId;
        }, OnError);
    }

    public void Register(string username, string password, string email)
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = email,
            Username = username,
            Password = password,
            RequireBothUsernameAndEmail = true
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    public void Login(string username, string password)
    {
        var request = new LoginWithPlayFabRequest
        {
            Username = username,
            Password = password
        };
        PlayFabClientAPI.LoginWithPlayFab(request, result =>
        {
            Observer.Notify(GameEvent.OnLoginSuccess, result);
            LoadPlayerData();
            Debug.Log("Login Successfully");
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
            Observer.Notify(GameEvent.OnLoginError, error);
        });
    }

    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());

    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Distance",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful leaderboard sent");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest()
        {
            StatisticName = "Distance",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);

    }

    private void OnLeaderboardGet(GetLeaderboardResult result)
    {
        List<LeaderboardRow> leaderboards = new List<LeaderboardRow>();
        foreach (var item in result.Leaderboard)
        {
            var leaderboardRow = new LeaderboardRow(item.Position, item.PlayFabId, item.StatValue);
            leaderboards.Add(leaderboardRow);
        }
        Observer.Notify(GameEvent.OnLeaderboardGet, leaderboards);
        Debug.Log("NO ERROR");
    }

    public void SavePlayerData()
    {
        GameData gameData = DataPersistenceManager.Instance.GameData;
        if (gameData == null) return;
        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>
            {
                { "Magnet", gameData.magnetLevel.ToString() },
                { "Double Coin", gameData.doublecoinLevel.ToString() },
                { "Triple Jump", gameData.tripleJumpLevel.ToString() },
                { "Shield", gameData.shieldLevel.ToString() }
            }
        }; 
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    public void LoadPlayerData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnError);
    }

    public void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Completely user data send !");

    }

    public void OnDataReceived(GetUserDataResult result)
    {
       
        if (result.Data != null)
        {
            if(result.Data.Count == 0)
            {
                DataPersistenceManager.Instance.LoadGame(new GameData());
                return;
            }
            GameData gameData = new GameData(
                shieldLevel: int.Parse(result.Data["Shield"].Value),
                tripleJumpLevel: int.Parse(result.Data["Triple Jump"].Value),
                magnetLevel: int.Parse(result.Data["Magnet"].Value),
                doublecoinLevel: int.Parse(result.Data["Double Coin"].Value)
            );
            DataPersistenceManager.Instance.LoadGame(gameData);
            Debug.Log("Successful player data received");
        }
        else
        {
            Debug.Log("RESULT DATA EMPTY");
        }
    }

    public void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Đăng ký thành công !");
        
    }



    private void OnApplicationQuit()
    {
        SavePlayerData();
    }

    void LoadMenuScene()
    {
        StartCoroutine(LoadMenuSceneCoroutine());
    }

    IEnumerator LoadMenuSceneCoroutine()
    {
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene("Menu Scene");
    }
    

}
