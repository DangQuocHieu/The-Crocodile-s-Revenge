using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerReviveManager : MonoBehaviour
{
    [SerializeField] float reviveYPosition;
    // Duration player hurt after reviving
    [SerializeField] float hurtDuration = 5f;
    private Rigidbody2D playerRb;
    bool isReviving = false;
    bool onJump = false;
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        Observer.AddObserver(GameEvent.OnPlayerBeginRevive, OnBeginRevive);
        Observer.AddObserver(GameEvent.OnGameOver, DisableRevive);
    }

    private void FixedUpdate()
    {
        if(isReviving && onJump)
        {
            FinishRevive();
        }
    }

    public void OnJump(InputValue value)
    {
        onJump = value.isPressed;
    }
    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnPlayerBeginRevive, OnBeginRevive);
        Observer.RemoveListener(GameEvent.OnGameOver, DisableRevive);

    }
    void OnBeginRevive(object[] datas)
    {
        int currentHealth = GetComponent<PlayerHealthManager>().CurrentHealth;
        if (currentHealth <= 0) return;
        StartCoroutine(BeginRevive());
    }

    IEnumerator BeginRevive()
    {
        playerRb.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSecondsRealtime(0.5f);
        transform.DOMove(new Vector2(transform.position.x, reviveYPosition), 1f).OnComplete(() =>
        {
            isReviving = true;
        });
    }

    void FinishRevive()
    {
        playerRb.bodyType = RigidbodyType2D.Dynamic;
        isReviving = false;
        Observer.Notify(GameEvent.OnPlayerFinishRevive);
        Observer.Notify(GameEvent.OnPlayerHurt, hurtDuration);
    }

    void DisableRevive(object[] datas)
    {
        enabled = false;
    }

}
