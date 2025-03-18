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
        this.shieldLevel = 1;
        this.tripleJumpLevel = 1;
        this.magnetLevel = 1;
        this.doublecoinLevel = 1;
    }
    public GameData(int shieldLevel, int tripleJumpLevel, int magnetLevel, int doublecoinLevel)
    {
        this.shieldLevel = shieldLevel;
        this.tripleJumpLevel = tripleJumpLevel;
        this.magnetLevel = magnetLevel;
        this.doublecoinLevel = doublecoinLevel;
    }
}
