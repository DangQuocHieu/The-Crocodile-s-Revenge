using System.Collections;
using UnityEngine;

public class PowerUpEffect : MonoBehaviour
{
    [SerializeField] GameObject shield;
    [SerializeField] CircleCollider2D magnetCollider;
    [SerializeField] float magnetMultiplier;
    int currentMaxJumpCount;
    int maxJumpCount = 2;
    public int CurrentMaxJumpCount => currentMaxJumpCount;

    private Coroutine tripleJumpCoroutine = null;
    private Coroutine shieldCoroutine = null;
    private Coroutine magnetCoroutine = null;
    private void Awake()
    {
        Observer.AddObserver(GameEvent.OnPlayerPickupTripleJumpPowerUp, OnPickupTripleJumpPowerUp);
        Observer.AddObserver(GameEvent.OnPlayerPickUpShieldPowerUp, OnPickUpShieldPowerUp);
        Observer.AddObserver(GameEvent.OnPlayerPickUpMagnetPowerUp, OnPickUpMagnetPowerUp);
        Observer.AddObserver(GameEvent.OnPlayerBeginRevive, StopPowerupCoroutine);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnPlayerPickupTripleJumpPowerUp, OnPickupTripleJumpPowerUp);
        Observer.RemoveListener(GameEvent.OnPlayerPickUpShieldPowerUp, OnPickUpShieldPowerUp);
        Observer.RemoveListener(GameEvent.OnPlayerPickUpMagnetPowerUp, OnPickUpMagnetPowerUp);
        Observer.RemoveListener(GameEvent.OnPlayerBeginRevive, StopPowerupCoroutine);
    }
    private void Start()
    {
        currentMaxJumpCount = maxJumpCount;
    }

    void OnPickupTripleJumpPowerUp(object[] datas)
    {
        currentMaxJumpCount = (int)datas[1];
        if(tripleJumpCoroutine != null)
        {
            StopCoroutine(tripleJumpCoroutine);
        }
        tripleJumpCoroutine = StartCoroutine(TripleJumpEffect((float)datas[0]));
    }

    IEnumerator TripleJumpEffect(float duration)
    {
        yield return new WaitForSeconds(duration);
        currentMaxJumpCount = maxJumpCount;
        tripleJumpCoroutine = null;
        Observer.Notify(GameEvent.OnPowerdown);
    }

    void OnPickUpShieldPowerUp(object[] datas)
    {
        if(shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }
        shieldCoroutine = StartCoroutine(ShieldEffect((float)datas[0]));
    }

    IEnumerator ShieldEffect(float duration)
    {
        shield.SetActive(true);
        yield return new WaitForSeconds(duration);
        shield.SetActive(false);
        shieldCoroutine = null;
        Observer.Notify(GameEvent.OnPowerdown);
    }

    void OnPickUpMagnetPowerUp(object[] datas)
    {
        if (magnetCoroutine != null)
        {
            magnetCollider.radius /= magnetMultiplier;
            StopCoroutine(magnetCoroutine);
        }
        magnetCoroutine = StartCoroutine(MagnetEffect((float)datas[0]));
    }

    IEnumerator MagnetEffect(float duration)
    {
        magnetCollider.radius *= magnetMultiplier;
        yield return new WaitForSeconds(duration);
        magnetCollider.radius /= magnetMultiplier;
        magnetCoroutine = null;
        Observer.Notify(GameEvent.OnPowerdown);
    }

    void StopPowerupCoroutine(object[] datas)
    {
        if (tripleJumpCoroutine != null)
        {
            StopCoroutine(tripleJumpCoroutine);
            currentMaxJumpCount = maxJumpCount; // Reset lại số lần nhảy
            tripleJumpCoroutine = null;
        }

        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
            shield.SetActive(false); // Tắt khiên
            shieldCoroutine = null;
        }

        if (magnetCoroutine != null)
        {
            StopCoroutine(magnetCoroutine);
            magnetCollider.radius /= magnetMultiplier; // Reset lại phạm vi nam châm
            magnetCoroutine = null;
        }
    }


}
