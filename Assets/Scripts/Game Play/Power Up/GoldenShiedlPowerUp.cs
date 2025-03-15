using UnityEngine;

public class GoldenShiedlPowerUp : PowerUp, ICollectible
{
    public override void OnCollect()
    {
        Observer.Notify(GameEvent.OnPlayerPickUpGoldenShieldPowerup);
    }
}
