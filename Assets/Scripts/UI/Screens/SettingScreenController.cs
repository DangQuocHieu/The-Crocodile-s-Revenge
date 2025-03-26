using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingScreenController : UIScreen
{

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Button backButton;
    [SerializeField] GameObject overlay;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        backButton.onClick.AddListener(() => { 

            ScreenManager.Instance.GoBack(isShowPrevScreen: true);
            canvasGroup.interactable = false;
        });
    }
    public override IEnumerator Show()
    {
        gameObject.SetActive(true);
        overlay.gameObject.SetActive(true);
        canvasGroup.interactable = true;
        yield return UIAnimationController.ScaleUIAnimation(rectTransform, isIn: true).WaitForCompletion();
    }
    public override IEnumerator Hide()
    {
        yield return UIAnimationController.ScaleUIAnimation(rectTransform, isIn: false).WaitForCompletion();
        overlay.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
