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
        
    }
    void OnBeginRevive(object[] datas)
    {
        StartCoroutine(BeginRevive());
    }

    IEnumerator BeginRevive()
    {
        playerRb.bodyType = RigidbodyType2D.Static;
        AudioManager.Instance.StopMusic();
        yield return new WaitForSecondsRealtime(0.5f);
        transform.DOMove(new Vector2(transform.position.x, reviveYPosition), 1f);
        yield return null;
        isReviving = true;
    }

    void FinishRevive()
    {
        playerRb.bodyType = RigidbodyType2D.Dynamic;
        AudioManager.Instance.ContinuePlayMusic();
        isReviving = false;
        Observer.Notify(GameEvent.OnPlayerFinishRevive);
        Observer.Notify(GameEvent.OnPlayerHurt, hurtDuration);
    }

}
