using UnityEngine;

public class ShieldPowerUp : PowerUp
{
    private void Awake()
    {
        level = DataPersistenceManager.Instance.GameData.shieldLevel;
        currentDuration = level * powerupData.durationIncreasePerLevel + powerupData.baseDuration;
    }
    public override void OnCollect()
    {
        Observer.Notify(GameEvent.OnPlayerPickUpShieldPowerUp, currentDuration);
        base.OnCollect(); 
    }

}
        