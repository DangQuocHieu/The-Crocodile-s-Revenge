using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "UITransitionConfig", menuName = "Scriptable Objects/UITransitionConfig")]
public class UITransitionConfig : ScriptableObject
{
    public float inDuration, waitDuration, outDuration;
    public RectTransform intransitionRect;
    public RectTransform outTransitionRect;
    public Vector2 inConfig, outConfig;
    public Ease ease = Ease.Linear;
}
