using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenController : UIScreen
{
    [SerializeField] Button restartButton;
    [SerializeField] Button returnToTilescreenButton;
    [SerializeField] TextMeshProUGUI totalCoinText;
    [SerializeField] TextMeshProUGUI totalDistanceText;
    [SerializeField] TextMeshProUGUI gameTimeText;

    RectTransform rectTransform;
    private void Awake()
    {
        restartButton.onClick.AddListener(() => {
            Observer.Notify(GameEvent.OnGameRestart);
        });
        returnToTilescreenButton.onClick.AddListener(() => {
            Observer.Notify(GameEvent.OnGobackToHomeScreen);
        });
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public override Tweener Hide()
    {
        UIAnimationController.Slide(rectTransform, isIn: false).OnComplete(() => { gameObject.SetActive(false); });
        return null;
    }

    public override Tweener Show()
    {
        gameObject.SetActive(true);
        UIAnimationController.Slide(rectTransform, isIn: true).OnComplete(() =>
        {
            int totalCoin = GameScreenController.Instance.CoinCollected;
            UIAnimationController.CountUp(totalCoinText, totalCoin);
            int totalDistance = GameManager.Instance.DistanceSoFar;
            UIAnimationController.CountUp(totalDistanceText, totalDistance);
            float totalTime = GameManager.Instance.TimeEpalsed;
            UIAnimationController.CountUpTimer(gameTimeText, totalTime);
        });
        return null;
    }
}
