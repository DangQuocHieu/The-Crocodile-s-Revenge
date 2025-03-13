using UnityEngine;

[System.Serializable]
public class GameData
{
    public int totalCoin;
    public float shieldPowerupDuration;
    public float tripleJumpPowerupDuration;
    
    public GameData()
    {
        this.totalCoin = 0;
        this.shieldPowerupDuration = 5f;
        this.tripleJumpPowerupDuration = 5f;
    }
}
