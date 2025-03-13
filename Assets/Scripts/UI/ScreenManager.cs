using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    Stack<ScreenID> stackScreen = new Stack<ScreenID>();

    [SerializeField] TextMeshProUGUI countdownTimerText;

    protected override void Awake()
    {
        Application.targetFrameRate = 60;
        base.Awake();
        screens.Add(ScreenID.HomeScreen, homeScreen);
        screens.Add(ScreenID.UpgradeScreen, upgradeScreen);
        screens.Add(ScreenID.SettingScreen, settingScreen);
        screens.Add(ScreenID.PauseScreen, pauseScreen);
        screens.Add(ScreenID.GameOverScreen, gameOverScreen);
        TransitionTo(ScreenID.HomeScreen);
        DontDestroyOnLoad(gameObject);

        Observer.AddObserver(GameEvent.OnGameResume, UpdateCountdownUI);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnGameResume, UpdateCountdownUI);
    }
    public void TransitionTo(ScreenID screenId)
    {
        if(stackScreen.Count > 0 && screenId == stackScreen.Peek())
        {
            return;
        }
        UIScreen currentScreen = (stackScreen.Count > 0) ? screens[stackScreen.Peek()] : null;
        UIScreen screenToShow = screens[screenId];
        screenToShow.Show().OnComplete(() =>
        {
            if (currentScreen != null)
            {
                currentScreen.Hide();
            }
        });
        
        stackScreen.Push(screenId);
    }

    public void GoBack(bool isHidePrevScreen = true)
    {
        if (stackScreen.Count <= 1) return;
        UIScreen currentScreen = screens[stackScreen.Pop()];
        currentScreen.Hide();
        UIScreen previousScreen = screens[stackScreen.Peek()];
        if(!isHidePrevScreen)
        previousScreen.Show();
    }

    async void UpdateCountdownUI(object[] datas)
    {
        float countdownDuration = (float)datas[0];
        if(countdownTimerText != null)
        countdownTimerText.gameObject.SetActive(true);
        for(float i = countdownDuration; i > 0; i--)
        {
            countdownTimerText.text = i.ToString();
            await Task.Delay(TimeSpan.FromSeconds(1f));
        }
        if(countdownTimerText != null)
        countdownTimerText.gameObject.SetActive(false);
    }
}
