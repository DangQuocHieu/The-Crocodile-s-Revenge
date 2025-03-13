using UnityEngine;

public class TripleJumpPowerup : PowerUp, IDataPersistence
{
    [SerializeField] int jumpCount = 3;

    public void LoadData(GameData data)
    {
        duration = data.tripleJumpPowerupDuration;
    }

    public override void OnCollect()
    {
        Observer.Notify(GameEvent.OnPlayerPickupTripleJumpPowerUp, duration, jumpCount);
        base.OnCollect();
    }

    public void SaveData(GameData data)
    {
        data.tripleJumpPowerupDuration = duration;
    }
}
