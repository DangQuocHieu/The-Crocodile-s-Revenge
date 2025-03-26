using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using TMPro;
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
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI usernameText;

    private CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();  
        playButton.onClick.AddListener(()=>{
            AudioManager.Instance.PlayStartSFX();
            canvasGroup.interactable = false;
            StartGame();
        });
        settingButton.onClick.AddListener(() => { 
            ScreenManager.Instance.ShowPopUp(ScreenID.SettingScreen); 
            canvasGroup.interactable = false;
        });
        upgradeButton.onClick.AddListener(() => { 
            ScreenManager.Instance.TransitionTo(ScreenID.UpgradeScreen); 
            canvasGroup.interactable = false;
        });
        statisticButton.onClick.AddListener(() =>
        {
            PlayFabManager.Instance.GetLeaderboard();
            ScreenManager.Instance.ShowPopUp(ScreenID.LeaderboardScreen);
            canvasGroup.interactable = false;
        });
        accountButton.onClick.AddListener(() =>
        {   
            AudioManager.Instance.StopMusic();
            Observer.Notify(GameEvent.OnPlayerLogOut);
            canvasGroup.interactable = false;
        });
        
    }
 
    void OnDestroy()
    {
    
    }
    void Update()
    {
        coinsText.text = PlayFabManager.Instance.Coins.ToString();
        usernameText.text = PlayFabManager.Instance.CurrentUserName.ToString();
    }
    void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }
    IEnumerator StartGameCoroutine()
    {
        yield return PlayFabManager.Instance.LoadPlayerData();
        yield return UITransitionController.SlideAndScaleTransition(StartGameCallback());
        gameObject.SetActive(false);
    }
    public IEnumerator StartGameCallback()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Game Scene");
        while(!asyncOperation.isDone)
        {
            yield return null;
        }
        if(asyncOperation.isDone)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            Observer.Notify(GameEvent.OnGameStart);
        }
    }

    public override IEnumerator Show()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        yield return null;
    }

    public override IEnumerator Hide()
    {
        gameObject.SetActive(false);
        yield return null;
    }

    public void DisplayCoinText(object[] datas)
    {
        int coins = (int)datas[0];
        coinsText.text = coins.ToString();
    }


}
