using UnityEngine;

[CreateAssetMenu(fileName = "PowerupData", menuName = "Scriptable Objects/PowerupData")]
public class PowerupData : ScriptableObject
{
    public PowerupType type;
    public float baseDuration;
    public float durationIncreasePerLevel;
    public int maxLevel;
}
