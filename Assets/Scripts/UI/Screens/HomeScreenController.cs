using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeScreenController : UIScreen
{
    [Header("Button")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button statisticButton;
    [SerializeField] private Button accountButton;
    private void Awake()
    {
        playButton.onClick.AddListener(StartGame);
        settingButton.onClick.AddListener(() => { ScreenManager.Instance.TransitionTo(ScreenID.SettingScreen); });
        upgradeButton.onClick.AddListener(() => { ScreenManager.Instance.TransitionTo(ScreenID.UpgradeScreen); });
        statisticButton.onClick.AddListener(() =>
        {
            PlayFabManager.Instance.GetLeaderboard();
            ScreenManager.Instance.TransitionTo(ScreenID.LeaderboardScreen);
        });
        accountButton.onClick.AddListener(() =>
        {
            ScreenManager.Instance.TransitionTo(ScreenID.AccountScreen);
        });
    }
    void StartGame()
    {
        Observer.Notify(GameEvent.OnGameStart);
        Time.timeScale = 1;
        StartCoroutine(UITransitionController.SlideAndScaleTransition(() =>
        {
            StartCoroutine(Hide());
            SceneManager.LoadSceneAsync("Game Scene");
        }));
    }
    public override IEnumerator Show()
    {
        gameObject.SetActive(true);
        yield return null;
    }

    public override IEnumerator Hide()
    {
        gameObject.SetActive(false);
        yield return null;
    }


}
