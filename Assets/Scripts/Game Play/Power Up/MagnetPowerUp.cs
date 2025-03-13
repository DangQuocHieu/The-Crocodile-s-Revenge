using UnityEngine;

public class MagnetPowerUp : PowerUp
{
    public override void OnCollect()
    {
        Observer.Notify(GameEvent.OnPlayerPickUpMagnetPowerUp, duration);
        base.OnCollect();
    }
}
