using DG.Tweening;
using System.Collections;
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
    [SerializeField] RectTransform overlay;
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

    public override IEnumerator Hide()
    {
        overlay.gameObject.SetActive(false);
        yield return UIAnimationController.Slide(rectTransform, isIn: false);
        gameObject.SetActive(false);
    }

    public override IEnumerator Show()
    {
        gameObject.SetActive(true);
        overlay.gameObject.SetActive(true);
        yield return UIAnimationController.Slide(rectTransform, isIn: true);
        int totalCoin = GameScreenController.Instance.CoinCollected;
        UIAnimationController.CountUp(totalCoinText, totalCoin);
        int totalDistance = GameManager.Instance.DistanceSoFar;
        UIAnimationController.CountUp(totalDistanceText, totalDistance);
        float totalTime = GameManager.Instance.TimeEpalsed;
        UIAnimationController.CountUpTimer(gameTimeText, totalTime);

    }
}
