using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class PowerUp : MonoBehaviour, ICollectible
{
    [SerializeField] protected float duration = 5f;
    [SerializeField] protected PowerupType type;
    [SerializeField] RectTransform powerupCountdownUI;
    public virtual void OnCollect()
    {
        Observer.Notify(GameEvent.OnPlayerPickUpPowerup, type, duration);
        Destroy(gameObject);
    }

}
