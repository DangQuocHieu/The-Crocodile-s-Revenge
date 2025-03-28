using UnityEngine;
public enum StrawType {
    WatermelonHat, StrawHat, ConicalHat
}
[CreateAssetMenu(fileName = "HatCustomizeData", menuName = "Scriptable Objects/HatCustomizeData")]
public class HatCustomizeData : ScriptableObject
{
    [SerializeField] StrawType type;
    public AnimationClip runClip;
    public AnimationClip jumpClip;
    public AnimationClip dieClip;
}
