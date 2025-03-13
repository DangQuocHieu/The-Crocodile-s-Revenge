using DG.Tweening;
using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class UITransitionController: MonoBehaviour
{

    static UITransitionConfig slideConfig = Resources.Load<UITransitionConfig>("ScriptableObjects/SlideTransitionConfig");
    static UITransitionConfig slideAndZoomConfig = Resources.Load<UITransitionConfig>("ScriptableObjects/CombineSlideAndZoom");
    public static async void SlideTransition(Func<Task> callback)
    {

        RectTransform slideInstance = Instantiate(slideConfig.intransitionRect, GameObject.Find("UI Canvas").transform);
        slideInstance.anchoredPosition = slideConfig.inConfig;
        Tweener slideInTweener = slideInstance.DOAnchorPos(Vector2.zero, slideConfig.inDuration).SetUpdate(true);
        await slideInTweener.AsyncWaitForCompletion();
        if (callback != null) await callback.Invoke();
        await Task.Yield();
        Tweener slideOutTweener = slideInstance.DOAnchorPos(slideConfig.outConfig, slideConfig.outDuration).SetUpdate(true);
        await slideOutTweener.AsyncWaitForCompletion();
        Destroy(slideInstance.gameObject);
    }

    public static async void SlideAndScaleTransition(Func<Task> callback)
    {

        RectTransform slideInstance = Instantiate(slideAndZoomConfig.intransitionRect, GameObject.Find("UI Canvas").transform);
        slideInstance.anchoredPosition = slideConfig.inConfig;
        Tweener slideInTweener = slideInstance.DOAnchorPos(Vector2.zero, slideAndZoomConfig.inDuration).SetUpdate(true);
        await slideInTweener.AsyncWaitForCompletion();
        if (callback != null) await callback.Invoke();
        await Task.Yield();
        Destroy(slideInstance.gameObject);
        RectTransform zoomInstance = Instantiate(slideAndZoomConfig.outTransitionRect, GameObject.Find("UI Canvas").transform);
        Tweener zoomTweener = zoomInstance.DOScale(Vector3.zero, slideAndZoomConfig.outDuration).SetUpdate(true);
        await zoomTweener.AsyncWaitForCompletion();
        Destroy(zoomInstance.gameObject);
    }
}
