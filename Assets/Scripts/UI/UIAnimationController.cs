using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
public class UIAnimationController: MonoBehaviour
{
    static UIAnimatonConfig scaleConfig = (UIAnimatonConfig)(Resources.Load<UIAnimatonConfig>("ScriptableObjects/ScaleConfig"));
    static UIAnimatonConfig slideConfig = (UIAnimatonConfig)(Resources.Load<UIAnimatonConfig>("ScriptableObjects/SlideConfig")); 
    public static Tweener ScaleUIAnimation(RectTransform rectTransform, bool isIn = true)
    {
        
        if(isIn)
        {
            rectTransform.localScale = Vector3.zero;
            return rectTransform.DOScale(scaleConfig.inConfig, scaleConfig.duration).SetEase(scaleConfig.ease).SetUpdate(true);
        }
        else
        {   
            return rectTransform.DOScale(scaleConfig.outConfig, scaleConfig.duration).SetEase(scaleConfig.ease).SetUpdate(true);
        }
    }

    public static Tweener Slide(RectTransform rectTransform, bool isIn = true)
    {
        if(isIn)
        {
            return rectTransform.DOAnchorPos(slideConfig.inConfig, slideConfig.duration).SetEase(slideConfig.ease).SetUpdate(true);
        }
        else
        {
            return rectTransform.DOAnchorPos(slideConfig.outConfig, slideConfig.duration).SetEase(slideConfig.ease).SetUpdate(true);
        }
    }

    public static Tweener CountUp(TextMeshProUGUI targetText, int targetNumber, float duration = 1f)
    {
        targetText.text = "0";
        return DOVirtual.Int(0, targetNumber, duration, (value) =>
        {
            targetText.text = value.ToString();
        }).SetUpdate(true);
    }

    public static Tweener CountUpTimer(TextMeshProUGUI targetText, float targetNumber, float duration = 1f)
    {
        targetText.text = "00:00:00";
        return DOVirtual.Float(0, targetNumber, duration, (value) =>
        {
            int minutes = Mathf.FloorToInt(value / 60);
            int seconds = Mathf.FloorToInt(value % 60);
            int ticks = Mathf.FloorToInt((value * 1000) % 1000 / 10);
            targetText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, ticks);
        }).SetUpdate(true); 
    }
}



