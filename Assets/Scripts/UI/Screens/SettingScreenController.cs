using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingScreenController : UIScreen
{
    [SerializeField] Button backButton;
    RectTransform rectTransform;
    private void Awake()
    {
        backButton.onClick.AddListener(() => { ScreenManager.Instance.GoBack(); });
        rectTransform = GetComponent<RectTransform>();
    }

    public override IEnumerator Show()
    {
        gameObject.SetActive(true);
        yield return UIAnimationController.Slide(rectTransform, isIn: true).WaitForCompletion();
    }
    public override IEnumerator Hide()
    {
        yield return UIAnimationController.Slide(rectTransform, isIn: false).WaitForCompletion();
        gameObject.SetActive(false);
    }

}
