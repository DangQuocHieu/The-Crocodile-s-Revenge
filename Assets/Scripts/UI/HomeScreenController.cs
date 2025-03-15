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

    private void Awake()
    {
        playButton.onClick.AddListener(StartGame);
        settingButton.onClick.AddListener(() => { ScreenManager.Instance.TransitionTo(ScreenID.SettingScreen); });
        upgradeButton.onClick.AddListener(() => { ScreenManager.Instance.TransitionTo(ScreenID.UpgradeScreen); });
    }
    void StartGame()
    {
        Observer.Notify(GameEvent.OnGameStart);
        Time.timeScale = 1;
        UITransitionController.SlideAndScaleTransition(() =>
        {
            SceneManager.LoadScene("Game Scene");
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
