using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    }

    private void Start()
    {
        if(!PlayerPrefs.HasKey("PlayfabSessionTicket"))
        {
            LoginAsAGuest();
            LoadMenuScene();
        }
        else
        {
            
        }
    }

    private void Update()
    {
        if(currentPlayerText != null)
        currentPlayerText.text = "CURRENT USER: "+ currentId.ToString();
    }

    public void GetUser()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result =>
        {
            currentId = result.AccountInfo.PlayFabId;
        }, OnError);
    }

    void LoginAsAGuest()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
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
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnError);
    }

    public void LinkAccountToGuest(string email, string password)
    {
        var request = new AddUsernamePasswordRequest
        {
            Email = email,
            Password = password
        };
        PlayFabClientAPI.AddUsernamePassword(request, OnLinkAccountSuccess, OnError);
    }
    void OnSuccess(LoginResult result)
    {
        GetUser();
        Debug.Log("Successful login/account create !");
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

    public void OnLoginSuccess(LoginResult result)
    {
        GetUser();
        Debug.Log("Đăng nhập thành công !");
        string sessionTicket = result.SessionTicket;
        PlayerPrefs.SetString("PlayfabSessionTicket", sessionTicket);
        PlayerPrefs.Save();
    }

    public void OnLinkAccountSuccess(AddUsernamePasswordResult result)
    {
        Debug.Log("Liên kết thành công");
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
