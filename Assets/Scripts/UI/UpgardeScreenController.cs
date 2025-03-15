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
