using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "UIAnimatonConfig", menuName = "Scriptable Objects/UIAnimatonConfig")]
public class UIAnimatonConfig : ScriptableObject
{
    public float duration = 0.5f;
    public Vector2 inConfig = Vector2.one;
    public Vector2 outConfig = Vector2.zero;
    public Ease ease = Ease.Linear;
}
