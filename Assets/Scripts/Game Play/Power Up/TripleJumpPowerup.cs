using UnityEngine;

public class TripleJumpPowerup : PowerUp
{
    [SerializeField] int jumpCount = 3;

    private void Awake()
    {
        level = DataPersistenceManager.Instance.GameData.tripleJumpLevel;
        currentDuration = powerupData.baseDuration + powerupData.durationIncreasePerLevel * level;
    }
    public override void OnCollect()
    {
        Observer.Notify(GameEvent.OnPlayerPickupTripleJumpPowerUp, currentDuration, jumpCount);
        base.OnCollect();
    }

}
