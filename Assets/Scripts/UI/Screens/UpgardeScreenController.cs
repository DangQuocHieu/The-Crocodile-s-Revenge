using DG.Tweening;
using NUnit.Framework;
using System;
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
    }
    public override Tweener Show()
    {
        UITransitionController.SlideTransition(() => {
            gameObject.SetActive(true); 
        });
        return null;

    }

    public override Tweener Hide()
    {
        UITransitionController.SlideTransition(() => {
            gameObject.SetActive(false); });
        return null;
    }
    
}
