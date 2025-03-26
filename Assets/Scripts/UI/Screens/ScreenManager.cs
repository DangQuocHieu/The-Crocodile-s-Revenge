
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class ScreenManager : Singleton<ScreenManager>
{
    [SerializeField] Dictionary<ScreenID, UIScreen> screens = new Dictionary<ScreenID, UIScreen>();
    [SerializeField] UIScreen homeScreen;
    [SerializeField] UIScreen settingScreen;
    [SerializeField] UIScreen upgradeScreen;
    [SerializeField] UIScreen pauseScreen;
    [SerializeField] UIScreen gameOverScreen;
    [SerializeField] UIScreen leaderboardScreen;
    [SerializeField] UIScreen accountScreen;
    [SerializeField] UIScreen registerScreen;
    [SerializeField] UIScreen recoverPasswordScreen;
    [SerializeField] UIScreen errorScreen;
    [SerializeField] UIScreen confirmScreen;
    Stack<ScreenID> stackScreen = new Stack<ScreenID>();

    [SerializeField] TextMeshProUGUI countdownTimerText;

    protected override void Awake()
    {
        Time.timeScale = 1f;
        base.Awake();
        DontDestroyOnLoad(gameObject);
        screens.Add(ScreenID.HomeScreen, homeScreen);
        screens.Add(ScreenID.UpgradeScreen, upgradeScreen);
        screens.Add(ScreenID.SettingScreen, settingScreen);
        screens.Add(ScreenID.PauseScreen, pauseScreen);
        screens.Add(ScreenID.GameOverScreen, gameOverScreen);
        screens.Add(ScreenID.LeaderboardScreen, leaderboardScreen);
        screens.Add(ScreenID.LoginScreen, accountScreen);
        screens.Add(ScreenID.RegisterScreen, registerScreen);
        screens.Add(ScreenID.RecoverPasswordScreen, recoverPasswordScreen);
        screens.Add(ScreenID.ErrorScreen, errorScreen);
        screens.Add(ScreenID.ConfrimScreen, confirmScreen);
        Observer.AddObserver(GameEvent.OnLoginSuccessfully, TransitionToHomeScreen);
        Observer.AddObserver(GameEvent.OnGameResume, UpdateCountdownUI);
        Observer.AddObserver(GameEvent.OnPlayFabError, OnPlayFabError);
        Observer.AddObserver(GameEvent.OnConfirmNotification, OnConfirmNotification);
        if(!PlayerPrefs.HasKey("RememberId"))
        {
            TransitionTo(ScreenID.LoginScreen);
        }
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnLoginSuccessfully, TransitionToHomeScreen);
        Observer.RemoveListener(GameEvent.OnGameResume, UpdateCountdownUI);
        Observer.RemoveListener(GameEvent.OnPlayFabError, OnPlayFabError);
        Observer.RemoveListener(GameEvent.OnConfirmNotification, OnConfirmNotification);
    }
    public void TransitionTo(ScreenID screenId)
    {
        if(stackScreen.Count > 0 && screenId == stackScreen.Peek())
        {
            return;
        }
        StartCoroutine(TransitionToCoroutine(screenId));

    }

    public void ShowPopUp(ScreenID screenId)
    {
        if(stackScreen.Count > 0 && screenId == stackScreen.Peek())
        {
            return;
        }
        StartCoroutine(TransitionToCoroutine(screenId,  false));
    }
    IEnumerator TransitionToCoroutine(ScreenID screenId, bool hidePrevScreen = true)
    {
        UIScreen currentScreen = (stackScreen.Count > 0) ? screens[stackScreen.Peek()] : null;
        UIScreen screenToShow = screens[screenId];
        yield return StartCoroutine(screenToShow.Show());
        if(currentScreen != null && hidePrevScreen)
        yield return StartCoroutine(currentScreen.Hide());
        stackScreen.Push(screenId);
    }

    public void GoBack(bool isShowPrevScreen = false)
    {
        if (stackScreen.Count <= 1) {
            if(stackScreen.Count == 1) stackScreen.Pop();
            TransitionTo(ScreenID.LoginScreen);

            return;
        }
        StartCoroutine(GoBackCoroutine(isShowPrevScreen));

    }

    public void HidePopUp()
    {
        StartCoroutine(HidePopUpCoroutine());
    }

    IEnumerator GoBackCoroutine(bool isShowPrevScreen)
    {
        UIScreen currentScreen = screens[stackScreen.Pop()];
        UIScreen previousScreen = screens[stackScreen.Peek()];
        if(isShowPrevScreen)
        {
            yield return StartCoroutine(previousScreen.Show());
        }
        yield return StartCoroutine(currentScreen.Hide());
    }

    IEnumerator HidePopUpCoroutine()
    {
        if(stackScreen.Count <= 1) yield break;
        UIScreen currentScreen = screens[stackScreen.Pop()];
        UIScreen previousScreen = screens[stackScreen.Peek()];
        previousScreen.gameObject.SetActive(true);
        previousScreen.gameObject.GetComponent<CanvasGroup>().interactable = true;
        yield return StartCoroutine(currentScreen.Hide());
    }
    public void UpdateCountdownUI(object[] datas)
    {
        StartCoroutine(UpdateCountdownUICoroutine((float)datas[0]));
    }
    IEnumerator UpdateCountdownUICoroutine(float duration)
    {
        float countdownDuration = duration;
        if(countdownTimerText != null)
        countdownTimerText.gameObject.SetActive(true);
        for(float i = countdownDuration; i > 0; i--)
        {
            countdownTimerText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }
        if(countdownTimerText != null)
        countdownTimerText.gameObject.SetActive(false);
    }
    public void OnPlayFabError(object[] datas)
    {
        string errorMessage = (string)datas[0];
        errorScreen.GetComponent<ErrorScreenController>().SetErrorMessage(errorMessage);
        ShowPopUp(ScreenID.ErrorScreen);
    }

    public void OnConfirmNotification(object[] datas)
    {
        string message = (string)datas[0];
        confirmScreen.GetComponent<ConfirmScreenController>().SetMessage(message);
        ShowPopUp(ScreenID.ConfrimScreen);
    }

    public void TransitionToHomeScreen(object[] datas)
    {
        TransitionTo(ScreenID.HomeScreen);
    }

}
