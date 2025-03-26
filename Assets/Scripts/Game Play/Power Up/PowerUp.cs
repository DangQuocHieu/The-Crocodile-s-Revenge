using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class PowerUp : MonoBehaviour, ICollectible
{
    protected int level = 1;
    protected float currentDuration;
    [SerializeField] protected PowerupData powerupData;

    public virtual void OnCollect()
    {
        Debug.Log(currentDuration);
        Observer.Notify(GameEvent.OnPlayerPickUpPowerup, powerupData.type, currentDuration);
        Destroy(gameObject);
    }

}
