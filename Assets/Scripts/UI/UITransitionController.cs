using DG.Tweening;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class UITransitionController: MonoBehaviour
{

    static UITransitionConfig slideConfig = Resources.Load<UITransitionConfig>("ScriptableObjects/SlideTransitionConfig");
    static UITransitionConfig slideAndZoomConfig = Resources.Load<UITransitionConfig>("ScriptableObjects/CombineSlideAndZoom");
    public static IEnumerator SlideTransition(IEnumerator callback = null)
    {
       
        RectTransform slideInstance = Instantiate(slideConfig.intransitionRect, GameObject.Find("UI Canvas").transform);
        slideInstance.anchoredPosition = slideConfig.inConfig;
        yield return slideInstance.DOAnchorPos(Vector2.zero, slideConfig.inDuration).SetUpdate(true).WaitForCompletion();
        yield return callback;
        yield return slideInstance.DOAnchorPos(slideConfig.outConfig, slideConfig.outDuration).SetUpdate(true).OnComplete(() =>
        {
            Destroy(slideInstance.gameObject);
        }).WaitForCompletion();
    }

    public static IEnumerator SlideAndScaleTransition(IEnumerator callback = null)
    {
        Transform parent = GameObject.Find("UI Canvas").transform;
        RectTransform slideInstance = Instantiate(slideAndZoomConfig.intransitionRect, parent);
        slideInstance.anchoredPosition = slideAndZoomConfig.inConfig;
        Sequence sequence = DOTween.Sequence();
        sequence.SetUpdate(true)
            .Append(slideInstance.DOAnchorPos(Vector2.zero, slideAndZoomConfig.inDuration).SetUpdate(true));
        yield return sequence.WaitForCompletion();
        yield return callback;
        sequence = DOTween.Sequence();
        sequence.SetUpdate(true)
            .Append(slideInstance.DOAnchorPos(slideAndZoomConfig.outConfig, slideAndZoomConfig.outDuration).SetUpdate(true))
            .AppendCallback(() => Destroy(slideInstance.gameObject)).OnComplete(() =>
            {
                RectTransform zoomInstance = Instantiate(slideAndZoomConfig.outTransitionRect, parent);
                zoomInstance.DOScale(Vector3.zero, slideAndZoomConfig.outDuration).SetUpdate(true).OnComplete(() =>
                {
                    Destroy(zoomInstance.gameObject);
                });
            });
        yield return sequence.WaitForCompletion();
    }


}
