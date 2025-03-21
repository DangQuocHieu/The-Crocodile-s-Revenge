using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UpgardeScreenController : UIScreen
{
    [SerializeField] Button upgradeMagnetButton;
    [SerializeField] Button upgradeShieldButton;
    [SerializeField] Button upgradeTripleJumpButton;
    [SerializeField] Button upgradeDoubleCoinButton;
    [SerializeField] Button backButton;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        upgradeMagnetButton.onClick.AddListener(() =>
        {
            Observer.Notify(GameEvent.OnPlayerUpgradePowerup, PowerupType.Magnet);
        });
        upgradeShieldButton.onClick.AddListener(() =>
        {
            Observer.Notify(GameEvent.OnPlayerUpgradePowerup, PowerupType.Shield);
        });
        upgradeTripleJumpButton.onClick.AddListener(() =>
        {
            Observer.Notify(GameEvent.OnPlayerUpgradePowerup, PowerupType.TripleJump);
        });
        upgradeDoubleCoinButton.onClick.AddListener(() =>
        {
            Observer.Notify(GameEvent.OnPlayerUpgradePowerup, PowerupType.DoubleCoin);
        });
        backButton.onClick.AddListener(() =>
        {
            ScreenManager.Instance.GoBack();
        });
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
    }
    public override IEnumerator Show()
    {
        gameObject.SetActive(true);
        yield return StartCoroutine(UITransitionController.SlideTransition(ShowCallback()));
    }

    IEnumerator ShowCallback()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        yield return null;
    }

    public override IEnumerator Hide()
    {
        yield return StartCoroutine(UITransitionController.SlideTransition(HideCallback()));
        gameObject.SetActive(false);
    }
    
    IEnumerator HideCallback()
    {
        yield return null;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
    }
}
