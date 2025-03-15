using DG.Tweening;
using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class UITransitionController: MonoBehaviour
{

    static UITransitionConfig slideConfig = Resources.Load<UITransitionConfig>("ScriptableObjects/SlideTransitionConfig");
    static UITransitionConfig slideAndZoomConfig = Resources.Load<UITransitionConfig>("ScriptableObjects/CombineSlideAndZoom");
    public static void SlideTransition(Action callback)
    {
        Sequence sequence = DOTween.Sequence();
        RectTransform slideInstance = Instantiate(slideConfig.intransitionRect, GameObject.Find("UI Canvas").transform);
        slideInstance.anchoredPosition = slideConfig.inConfig;
        sequence
            .SetUpdate(true)
            .Append(slideInstance.DOAnchorPos(Vector2.zero, slideConfig.inDuration).SetUpdate(true))
            .AppendCallback(() => callback?.Invoke())
            .Append(slideInstance.DOAnchorPos(slideConfig.outConfig, slideConfig.outDuration).SetUpdate(true).OnComplete(() =>
            {
                Destroy(slideInstance.gameObject);
            }));
    }

    public static void SlideAndScaleTransition(Action callback)
    {
        Transform parent = GameObject.Find("UI Canvas")?.transform;
        if (parent == null) return;
        RectTransform slideInstance = Instantiate(slideAndZoomConfig.intransitionRect, parent);
        slideInstance.anchoredPosition = slideAndZoomConfig.inConfig;
        Sequence sequence = DOTween.Sequence();
        sequence.SetUpdate(true)
            .Append(slideInstance.DOAnchorPos(Vector2.zero, slideAndZoomConfig.inDuration).SetUpdate(true))
            .AppendCallback(() => callback?.Invoke())
            .AppendInterval(0.1f)
            .Append(slideInstance.DOAnchorPos(slideAndZoomConfig.outConfig, slideAndZoomConfig.outDuration).SetUpdate(true))
            .AppendCallback(() => Destroy(slideInstance.gameObject)).OnComplete(() =>
            {
                RectTransform zoomInstance = Instantiate(slideAndZoomConfig.outTransitionRect, parent);
                zoomInstance.DOScale(Vector3.zero, slideAndZoomConfig.outDuration).SetUpdate(true).OnComplete(() =>
                {
                    Destroy(zoomInstance.gameObject);
                });
            });
        sequence.Play();
    }


}
