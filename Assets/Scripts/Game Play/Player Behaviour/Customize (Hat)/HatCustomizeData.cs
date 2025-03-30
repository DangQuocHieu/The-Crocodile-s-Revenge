using UnityEngine;
[CreateAssetMenu(fileName = "HatCustomizeData", menuName = "Scriptable Objects/HatCustomizeData")]
public class HatCustomizeData : ScriptableObject
{
    public string id;
    public uint price;
    public bool owned;
    public Sprite hatSprite;    
    public AnimationClip runClip;
    public AnimationClip jumpClip;
    public AnimationClip dieClip;
}
