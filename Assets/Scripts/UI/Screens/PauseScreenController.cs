using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreenController : UIScreen
{
    
    [SerializeField] Button resumeButton;
    [SerializeField] Button restartButton;
    [SerializeField] Button returnToTilescreenButton;
    [SerializeField] GameObject overlay;

    //duration wait for resume game
    [SerializeField] float countDownDuration = 3f;
    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            Observer.Notify(GameEvent.OnGameResume, countDownDuration);
        });
        restartButton.onClick.AddListener(() => {
            Observer.Notify(GameEvent.OnGameRestart);
        });
        returnToTilescreenButton.onClick.AddListener(() => {
            Observer.Notify(GameEvent.OnGobackToHomeScreen);
        });
    }

    public override Tweener Hide()
    {
        overlay.SetActive(false);
        gameObject.SetActive(false);
        return null;
    }

    public override Tweener Show()
    {
        overlay.SetActive(true);
        gameObject.SetActive(true);
        return null;
    }
}
