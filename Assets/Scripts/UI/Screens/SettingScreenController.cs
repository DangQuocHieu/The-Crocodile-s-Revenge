using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingScreenController : UIScreen
{
    [SerializeField] Button backButton;
    [SerializeField] float delayDuration = 0.2f;
    RectTransform rectTransform;
    private void Awake()
    {
        backButton.onClick.AddListener(() => { ScreenManager.Instance.GoBack(); });
        rectTransform = GetComponent<RectTransform>();
    }

    public override Tweener Show()
    {
        gameObject.SetActive(true);
        UIAnimationController.Slide(rectTransform, isIn: true);
        return null;
    }
    public override Tweener Hide()
    {
        UIAnimationController.Slide(rectTransform, isIn: false).OnComplete(() => { gameObject.SetActive(false); });
        return null;
    }

}
