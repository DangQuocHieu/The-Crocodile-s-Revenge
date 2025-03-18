using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardScreen : UIScreen
{
    [SerializeField] RectTransform overlay;
    [SerializeField] Button returnButton;
    [SerializeField] GameObject tableContent;
    [SerializeField] RectTransform tableRowPrefab;
    RectTransform rectTransform;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        returnButton.onClick.AddListener(() => { ScreenManager.Instance.GoBack(); });
        Observer.AddObserver(GameEvent.OnLeaderboardGet, DisplayLeaderboard);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnLeaderboardGet, DisplayLeaderboard);
    }
    public override Tweener Show()
    {
        overlay.gameObject.SetActive(true);
        gameObject.SetActive(true);
        UIAnimationController.ScaleUIAnimation(rectTransform, isIn: true);
        return null;
    }

    public override Tweener Hide()
    {
        overlay.gameObject.SetActive(false);
        UIAnimationController.ScaleUIAnimation(rectTransform, isIn: false).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
        return null;
    }

    public void DisplayLeaderboard(object[] datas)
    {
        foreach(Transform item in tableContent.transform)
        {
            Destroy(item.gameObject);
        }
        List<LeaderboardRow> leaderboards = (List<LeaderboardRow>)datas[0];
        foreach(LeaderboardRow row in leaderboards)
        {
            RectTransform tableRow = Instantiate(tableRowPrefab, tableContent.transform);
            TextMeshProUGUI[] texts = tableRow.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = row.place.ToString();
            texts[1].text = row.name.ToString();
            texts[2].text = row.score.ToString();
        }
    }
}
