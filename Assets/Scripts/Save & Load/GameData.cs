using UnityEngine;

[System.Serializable]
public class GameData
{
    public int shieldLevel;
    public int tripleJumpLevel;
    public int magnetLevel;
    public int doublecoinLevel;
    public string hatEquippedId;

    public GameData()
    {
        this.shieldLevel = 0;
        this.tripleJumpLevel = 0;
        this.magnetLevel = 0;
        this.doublecoinLevel = 0;
        this.hatEquippedId = "Empty";
    }
    public GameData(int shieldLevel, int tripleJumpLevel, int magnetLevel, int doublecoinLevel, string hatEquippedId)
    {
        this.shieldLevel = shieldLevel;
        this.tripleJumpLevel = tripleJumpLevel;
        this.magnetLevel = magnetLevel;
        this.doublecoinLevel = doublecoinLevel;
        this.hatEquippedId = hatEquippedId;
    }
}
