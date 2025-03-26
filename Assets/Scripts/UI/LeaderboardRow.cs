using UnityEngine;

public class LeaderboardRow
{
    public int place { get; set; }
    public string name {  get; set; }
    public int score {  get; set; }
    public LeaderboardRow(int place, string name, int score)
    {
        this.place = place;
        this.name = name;
        this.score = score;
    }
}
