using UnityEngine;

public class DoubleCoinPowerUp : PowerUp
{
    private void Awake()
    {
        level = DataPersistenceManager.Instance.GameData.doublecoinLevel;
        currentDuration = level * powerupData.durationIncreasePerLevel + powerupData.baseDuration;
    }
    public override void OnCollect()
    {
        Observer.Notify(GameEvent.OnPlayerPickUpDoubleCoinPowerUp, currentDuration);
        base.OnCollect();
    }

}
