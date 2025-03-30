using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardScreen : UIScreen
{
    [SerializeField] RectTransform overlay;
    [SerializeField] Button backButton;
    [SerializeField] GameObject tableContent;
    [SerializeField] RectTransform tableRowPrefab;
    RectTransform rectTransform;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        backButton.onClick.AddListener(() => { ScreenManager.Instance.HidePopUp(); });
        Observer.AddObserver(GameEvent.OnLeaderboardGet, DisplayLeaderboard);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnLeaderboardGet, DisplayLeaderboard);
    }
    public override IEnumerator Show()
    {
        overlay.gameObject.SetActive(true);
        gameObject.SetActive(true);
        yield return UIAnimationController.ScaleUIAnimation(rectTransform, isIn: true).WaitForCompletion();
    }

    public override IEnumerator Hide()
    {
        overlay.gameObject.SetActive(false);
        yield return UIAnimationController.ScaleUIAnimation(rectTransform, isIn: false).WaitForCompletion();
        gameObject.SetActive(false);
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
            texts[1].text = row.name == null ? "" : row.name;
            texts[2].text = row.score.ToString();
        }
    }
}
