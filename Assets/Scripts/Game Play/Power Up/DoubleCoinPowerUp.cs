using UnityEngine;

public class DoubleCoinPowerUp : PowerUp
{
    public override void OnCollect()
    {
        Observer.Notify(GameEvent.OnPlayerPickUpDoubleCoinPowerUp, duration);
        base.OnCollect();
    }
}
