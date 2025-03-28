using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabManager : Singleton<PlayFabManager>
{
    private int coins = 0;
    public int Coins => coins;
    private string currentUsername = "";
    public string CurrentUserName => currentUsername;

    private string sessionId;

    private List<CatalogItem> catalogItems = new List<CatalogItem>();
    protected override void Awake()
    {
        sessionId = Guid.NewGuid().ToString();
        base.Awake();
        DontDestroyOnLoad(this);
        Observer.AddObserver(GameEvent.OnPlayerLogin, OnPlayerLogin);
        Observer.AddObserver(GameEvent.OnPlayerLogOut, OnPlayerLogOut);
        Observer.AddObserver(GameEvent.OnRecoverPassword, OnRecoverPassword);   
        Observer.AddObserver(GameEvent.OnOtherPlayerLogin, OnOtherPlayerLogin);

    }


    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnPlayerLogin, OnPlayerLogin);
        Observer.RemoveListener(GameEvent.OnPlayerLogOut, OnPlayerLogOut);
        Observer.RemoveListener(GameEvent.OnRecoverPassword, OnRecoverPassword);
        Observer.RemoveListener(GameEvent.OnOtherPlayerLogin, OnOtherPlayerLogin);

    }

    public void OnPlayerLogin(object[] datas)
    {
        string username = (string) datas[0];
        string password = (string) datas[1];
        StartCoroutine(OnPlayerLoginCoroutine(username, password));
    }

    public IEnumerator OnPlayerLoginCoroutine(string username, string password)
    {
        bool isDone = false, isError = false;
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest{
            Username = username,
            Password = password,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetUserData = true
            }
        }, result =>{
            Guid guid = new Guid();
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string> 
                {
                    {"SessionToken", guid.ToString()}
                }
            }, result => {
               
                isDone = true;
                
            }, error => {
                OnError(error);
                isError = true;
            });
        }, error => {
            OnError(error);
            isError = true;
        });
        if(isError) yield break;
        yield return new WaitUntil(() => isDone);
        StartCoroutine(UITransitionController.SlideTransition(OnPlayerLoginCallback()));
    }

    IEnumerator UpdateSessionId()
    {
        var request = new UpdateUserDataRequest() 
        {
            Data = new Dictionary<string, string> 
            {
                {"SessionId", sessionId}
            }
        };
        bool isDone = false, isError = false;
        PlayFabClientAPI.UpdateUserData(request, result =>
        {
            isDone = true;
        }, error => {
            isError = true;
            OnError(error);
        });

        if(isError) yield break;
        yield return new WaitUntil(()=>isDone);
    }

    public IEnumerator CheckSessionIdRoutine()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(10f);
            if(PlayFabClientAPI.IsClientLoggedIn())
            {
                PlayFabClientAPI.GetUserData(new GetUserDataRequest(){

                }, result => {
                    if(result.Data.ContainsKey("SessionId") && result.Data["SessionId"].Value != sessionId)
                    {
                        Observer.Notify(GameEvent.OnOtherPlayerLogin, "Another device logged in! Log out...");
                    }
                }, 
                error => {
                    OnError(error);
                });
            }
            
        }
    }
    
    void OnOtherPlayerLogin(object[] datas)
    {
        StartCoroutine(OnOtherPlayerLoginCoroutine());
    }

    IEnumerator OnOtherPlayerLoginCoroutine()
    {
        yield return new WaitForSecondsRealtime(3f);
        Observer.Notify(GameEvent.OnPlayerLogOut);
    }

    IEnumerator OnPlayerLoginCallback()
    {
        yield return UpdateSessionId();
        yield return GetVirtualCurrencies();
        yield return LoadPlayerData();
        yield return  GetUserName();
        yield return GetCatalogItems();
        Observer.Notify(GameEvent.OnLoginSuccessfully);
        StartCoroutine(CheckSessionIdRoutine());

    }
        
    public IEnumerator GetUserName()
    {
        bool isDone = false, isError = false;
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result =>
        {
            currentUsername = result.AccountInfo.Username;
            isDone = true;
        }, error => {
            OnError(error);
            isError = true;
        });
        if(isError) yield break;
        yield return new WaitUntil(() => isDone);
        Debug.Log("CurrentUser: " + currentUsername);
    }

    public void Register(string username, string password, string email)
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = email,
            Username = username,
            Password = password,
            DisplayName = username,
            RequireBothUsernameAndEmail = true
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }



    void OnError(PlayFabError error)
    {
        string errorMessage = GetPlayFabErrorMessage(error);
        Observer.Notify(GameEvent.OnPlayFabError, errorMessage);
    }

    private string GetPlayFabErrorMessage(PlayFabError error)
    {
        switch(error.Error)
        {
            case PlayFabErrorCode.NameNotAvailable:
                return "Name has already been registered";
            case PlayFabErrorCode.EmailAddressNotAvailable:
                return "Email has already been registered";
            case PlayFabErrorCode.InvalidUsernameOrPassword:
                return "Invalid user name or password";
        }
        return error.ErrorMessage;
    }
    
    public IEnumerator SendLeaderboard(int score)
    {
        bool isDone = false;

        PlayFabClientAPI.UpdatePlayerStatistics(new PlayFab.ClientModels.UpdatePlayerStatisticsRequest
        {
            Statistics = new List<PlayFab.ClientModels.StatisticUpdate>
            {
                new PlayFab.ClientModels.StatisticUpdate
                {
                    StatisticName = "Distance",
                    Value = score
                }
            }
        },
        result => {
            Debug.Log("Leaderboard updated successfully");
            isDone = true;
        },
        error => {
            Debug.LogError("Error updating leaderboard: " + error.GenerateErrorReport());
            isDone = true;
    });

        yield return new WaitUntil(() => isDone);
    }
    public void GetLeaderboard()
    {
        var request = new GetLeaderboardAroundPlayerRequest()
        {
            StatisticName = "Distance",
            MaxResultsCount = 100
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardGet, OnError);

    }
    private void OnLeaderboardGet(GetLeaderboardAroundPlayerResult result)
    {
        List<LeaderboardRow> leaderboards = new List<LeaderboardRow>();
        foreach (var item in result.Leaderboard)
        {
            var leaderboardRow = new LeaderboardRow(item.Position, item.DisplayName, item.StatValue);
            if(item.DisplayName == currentUsername)
            {
                leaderboardRow = new LeaderboardRow(item.Position + 1, item.DisplayName + "(You)", item.StatValue);
            } 
            leaderboards.Add(leaderboardRow);
        }
        Observer.Notify(GameEvent.OnLeaderboardGet, leaderboards);
        Debug.Log("NO ERROR");  
    }

    public IEnumerator RecoverPassword(string recoverEmail)
    {
        bool isDone = false, isError = false;
        PlayFabClientAPI.SendAccountRecoveryEmail(new SendAccountRecoveryEmailRequest
        {
            Email = recoverEmail,
            TitleId = PlayFabSettings.TitleId
        }, result =>
        {
            Debug.Log("Recovery email sent successfully");
            isDone = true;
        }, error =>
        {
            OnError(error);
            isError = true;

        });
        if(isError) 
        {
            yield break;
        }
        yield return new WaitUntil(() => isDone);
        Observer.Notify(GameEvent.OnConfirmNotification, "Password recovery email sent!");
    }
    public IEnumerator SavePlayerData()
    {
        if(!PlayFabClientAPI.IsClientLoggedIn())
        {
            yield break;
        }
        GameData gameData = DataPersistenceManager.Instance.GameData;
        if (gameData == null) yield break;
        bool isDone = false, isError = false;
        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>
            {
                { "Magnet", gameData.magnetLevel.ToString() },
                { "X2Coins", gameData.doublecoinLevel.ToString() },
                { "X3Jump", gameData.tripleJumpLevel.ToString() },
                { "Shield", gameData.shieldLevel.ToString() }
            }
        }; 
        PlayFabClientAPI.UpdateUserData(request, result => {
            isDone = true;
            Debug.Log("Player data saved successfully");
        }, error => {
            OnError(error);
            isError = true;
        });
        if(isError) yield break;
        yield return new WaitUntil(() => isDone);
    }

    public IEnumerator LoadPlayerData()
    {
        GetUserDataResult userDataResult = null;
        bool isDone = false, isError = false;
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result => {
            userDataResult = result;
            isDone = true;
        }, error => {
            OnError(error);
            isError = true;
        });
        if(isError) yield break;
        yield return new WaitUntil(() => isDone);
        OnDataReceived(userDataResult);
    }


    public void OnDataReceived(GetUserDataResult result)
    {
        if (result.Data != null)
        {
            if(result.Data.Count <= 0)
            {
                DataPersistenceManager.Instance.LoadGame(new GameData());
                Debug.Log("Successful player data received");
                return;
            }
            GameData gameData = new GameData(
                shieldLevel: result.Data.ContainsKey("Shield") ? int.Parse(result.Data["Shield"].Value) : 0,
                tripleJumpLevel: result.Data.ContainsKey("X3Jump") ? int.Parse(result.Data["X3Jump"].Value) : 0,
                magnetLevel: result.Data.ContainsKey("Magnet") ? int.Parse(result.Data["Magnet"].Value) : 0,
                doublecoinLevel:    result.Data.ContainsKey("X2Coins") ? int.Parse(result.Data["X2Coins"].Value) : 0
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
        Observer.Notify(GameEvent.OnConfirmNotification, "You have registered successfully");
    }

    private void OnApplicationQuit()
    {
        StartCoroutine(SavePlayerData());
    }
    public void OnPlayerLogOut(object[] datas)
    {
        StartCoroutine(LogOutCoroutine());
    }

    IEnumerator LogOutCoroutine()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        yield return UITransitionController.SlideTransition(LogOutCallback());
    }

    IEnumerator LogOutCallback()
    {
        ScreenManager.Instance.OnLogOut();
        yield return null;
    }
    public void OnRecoverPassword(object[] datas)
    {
        string recoverEmail = (string)datas[0];
        StartCoroutine(RecoverPassword(recoverEmail));
    }


    public IEnumerator GetVirtualCurrencies()
    {
        bool isDone = false, isError = false;
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>{
            coins = result.VirtualCurrency["CN"];
            isDone = true;
        }, error => {
            OnError(error);
            isError = true;
        });
        if(isError) yield break;
        yield return new WaitUntil(() => isDone);
        Debug.Log("Currencies Get");
    }

    public IEnumerator BuyItem(int price)
    {
        bool isDone = false, isError = false;
        PlayFabClientAPI.SubtractUserVirtualCurrency(new SubtractUserVirtualCurrencyRequest{
            VirtualCurrency = "CN", Amount = price
        }, result => {
            isDone = true;
        }, error => {
            isError = true;
            OnError(error);
        });
        if(isError) yield break;
        yield return new WaitUntil(()=>isDone);
    }

    public IEnumerator AddCoins(int coinToAdd)
    {
        bool isDone = false, isError = false;
        PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest{
            VirtualCurrency = "CN", Amount = coinToAdd
        }, result => {
            isDone = true;
        }, error => {
            isError = true;
            OnError(error);
        });
        if(isError) yield break;
        yield return new WaitUntil(()=>isDone);
    }
    
    IEnumerator GetCatalogItems()
    {
        bool isDone = false;
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest()
        {
            CatalogVersion = "1"
        }, result => {
            foreach(var item in result.Catalog)
            {
                catalogItems.Add(item);
                Debug.Log(item.ItemId + " " + item.VirtualCurrencyPrices + " " + item.DisplayName);
            }
            isDone = true;
        }, error => {
            OnError(error);

        });
        yield return new WaitUntil(()=>isDone);
    }
    
}
