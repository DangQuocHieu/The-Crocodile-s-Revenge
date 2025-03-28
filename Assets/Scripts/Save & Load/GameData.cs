using UnityEngine;

[System.Serializable]
public class GameData
{
    public int shieldLevel;
    public int tripleJumpLevel;
    public int magnetLevel;
    public int doublecoinLevel;

    public GameData()
    {
        this.shieldLevel = 0;
        this.tripleJumpLevel = 0;
        this.magnetLevel = 0;
        this.doublecoinLevel = 0;
    }
    public GameData(int shieldLevel, int tripleJumpLevel, int magnetLevel, int doublecoinLevel)
    {
        this.shieldLevel = shieldLevel;
        this.tripleJumpLevel = tripleJumpLevel;
        this.magnetLevel = magnetLevel;
        this.doublecoinLevel = doublecoinLevel;
    }
}
