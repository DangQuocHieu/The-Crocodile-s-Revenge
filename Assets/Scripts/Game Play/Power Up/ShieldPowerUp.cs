using UnityEngine;

public class ShieldPowerUp : PowerUp, IDataPersistence
{
    public override void OnCollect()
    {
        Observer.Notify(GameEvent.OnPlayerPickUpShieldPowerUp, duration);
        base.OnCollect(); 
    }

    public void SaveData(GameData data)
    {
        data.shieldPowerupDuration = duration;
    }
    public void LoadData(GameData data)
    {
        duration = data.shieldPowerupDuration;
    }
}
        