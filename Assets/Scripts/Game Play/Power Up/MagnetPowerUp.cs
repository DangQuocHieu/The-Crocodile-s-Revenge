using UnityEngine;

public class MagnetPowerUp : PowerUp
{
    private void Awake()
    {
        level = DataPersistenceManager.Instance.GameData.magnetLevel;
        currentDuration = level * powerupData.durationIncreasePerLevel + powerupData.baseDuration;
    }
    public override void OnCollect()
    {
        Observer.Notify(GameEvent.OnPlayerPickUpMagnetPowerUp, currentDuration);
        base.OnCollect();
    }

}
