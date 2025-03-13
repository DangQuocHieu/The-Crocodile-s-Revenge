using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectible
{
    [SerializeField] int coinToAdd = 1;
    [SerializeField] float moveSpeed = 1f;
    bool isAttracted = false;
    Coroutine doubleCoinCoroutine;
    private void Awake()
    {
        Observer.AddObserver(GameEvent.OnPlayerPickUpDoubleCoinPowerUp, OnPickUpDoubleCoinPowerUp);
    }

    private void FixedUpdate()
    {
        MoveToPlayer();
    }

    void MoveToPlayer()
    {
        if (!isAttracted) return;
        Transform playerTransform = PlayerController.Instance.transform;
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Magnet Zone"))
        {
            isAttracted = true;
        }
    }

    public void OnCollect()
    {
        Observer.Notify(GameEvent.OnPlayerPickUpCoin, coinToAdd);
        Destroy(gameObject);
    }

    void OnPickUpDoubleCoinPowerUp(object[] datas)
    {
        if(doubleCoinCoroutine != null)
        {
            coinToAdd /= 2;
            StopCoroutine(doubleCoinCoroutine);
        }
        doubleCoinCoroutine = StartCoroutine(DoubleCoinEffect((float)datas[0]));
    }

    IEnumerator DoubleCoinEffect(float duration)
    {
        coinToAdd *= 2;
        yield return new WaitForSeconds(duration);
        coinToAdd /= 2;
        doubleCoinCoroutine = null;
        Observer.Notify(GameEvent.OnPowerdown);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnPlayerPickUpDoubleCoinPowerUp, OnPickUpDoubleCoinPowerUp);

    }

}
