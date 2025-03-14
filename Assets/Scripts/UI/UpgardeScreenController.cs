using DG.Tweening;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UpgardeScreenController : UIScreen
{
    [SerializeField] Button backButton;
    
    protected void Awake()
    {
        backButton.onClick.AddListener(() => { ScreenManager.Instance.GoBack(); });
    }

    public override Tweener Show()
    {
        UITransitionController.SlideTransition(async() => {
            await Task.Yield();
            gameObject.SetActive(true); 
        });
        return null;

    }

    public override Tweener Hide()
    {
        UITransitionController.SlideTransition(async() => {
            await Task.Yield();
            gameObject.SetActive(false); });
        return null;
    }
}
