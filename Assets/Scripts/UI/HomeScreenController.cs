using DG.Tweening;
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

    protected override void Awake()
    {
        playButton.onClick.AddListener(StartGame);
        settingButton.onClick.AddListener(() => { ScreenManager.Instance.TransitionTo(ScreenID.SettingScreen); });
        upgradeButton.onClick.AddListener(() => { ScreenManager.Instance.TransitionTo(ScreenID.UpgradeScreen); });
        base.Awake();
    }
    void StartGame()
    {
        Observer.Notify(GameEvent.OnGameStart);
        UITransitionController.SlideAndScaleTransition(async() =>
        {
            await SceneManager.LoadSceneAsync("Game Scene");
            Hide();
        });
    }
    public override Tweener Show()
    {
        gameObject.SetActive(true);
        return null;
    }

    public override Tweener Hide()
    {
        gameObject.SetActive(false);
        return null;
    }


}
