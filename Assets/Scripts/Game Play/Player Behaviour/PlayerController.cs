﻿using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] Vector3 initPosition = new Vector3(0, -6, 0);
    [SerializeField] float jumpForce = 8f;
    [SerializeField] CapsuleCollider2D bodyCollider;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float extraHeightCheck = 0.01f;
    [SerializeField] float minVeclocityY;
    [SerializeField] float minPositionY = -10f;
    int jumpCount;
    Rigidbody2D playerRb;
    PowerUpEffect powerUpEffect;
    Animator playerAnimator;
    private bool wasGrounded;
    private bool isDisableControl = false;
    private bool isJumpRequested = false;
    protected override void Awake()
    {
        instance = this;
        transform.position = initPosition;
        playerRb = GetComponent<Rigidbody2D>();
        powerUpEffect = GetComponent<PowerUpEffect>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        playerAnimator = GetComponent<Animator>();
        Observer.AddObserver(GameEvent.OnGameOver, UpdateGameOverAnimation);
        Observer.AddObserver(GameEvent.OnPlayerFinishRevive, OnFinishRevive);
    }

    void Start()
    {
        playerAnimator.runtimeAnimatorController = PlayerCustomizationController.Instance.PlayerAnimatorController;
    }
    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnGameOver, UpdateGameOverAnimation);
        Observer.RemoveListener(GameEvent.OnPlayerFinishRevive, OnFinishRevive);
    }
    private void Update()
    {
        if(transform.position.y < minPositionY)
        {
            Observer.Notify(GameEvent.OnPlayerFallIntoAHole);
        }
        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);
        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        if(IsTouchOverUI(touch.position))
        //        {
        //            Debug.Log("Touch UI");
        //        }
        //        else
        //        {
        //            isJumpRequested = true;
        //        }    
        //    }
        //}
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            ProcessJumping();
        }
    }
    void FixedUpdate()
    {
        OnGrounded();
        UpdateAnimation();
        if(isJumpRequested)
        {
            ProcessJumping();
            isJumpRequested = false;
        }
    }

    //private bool IsTouchOverUI(Vector2 touchPosition)
    //{
    //    PointerEventData eventData = new PointerEventData(EventSystem.current);
    //    eventData.position = touchPosition;

    //    List<RaycastResult> results = new List<RaycastResult>();
    //    EventSystem.current.RaycastAll(eventData, results);

    //    return results.Count > 0; 
    //}

    public void ProcessJumping()
    {
        if (isDisableControl) return;
        if (!IsGrounded())
        {
            if (jumpCount == 0 || jumpCount >= powerUpEffect.CurrentMaxJumpCount) return;
        }
        playerRb.linearVelocity = new Vector2(0, jumpForce);
        playerAnimator.SetBool("isRunning", false);
        if (IsMultipleJump()) Observer.Notify(GameEvent.OnPlayerMultipleJump);
        else Observer.Notify(GameEvent.OnPlayerJump);
        ++jumpCount;
    }
   
    bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(bodyCollider.bounds.center - new Vector3(bodyCollider.bounds.extents.x, 0, 0), Vector2.down, bodyCollider.bounds.extents.y + extraHeightCheck, groundLayerMask);
        return raycastHit.collider != null;
    }

    bool IsMultipleJump()
    {
        return jumpCount == powerUpEffect.CurrentMaxJumpCount - 1;
    }
    void UpdateAnimation()
    {
        playerAnimator.SetBool("isMultipleJump", jumpCount == powerUpEffect.CurrentMaxJumpCount);
    }

    void OnGrounded()
    {
        bool isGrounded = IsGrounded();
        if (playerRb.linearVelocityY >= minVeclocityY)
        {
            return;
        }
        if (!wasGrounded && isGrounded)
        {
            jumpCount = 0;
            playerAnimator.SetBool("isRunning", true);
        }
        wasGrounded = isGrounded;
    }

    void UpdateGameOverAnimation(object[] datas)
    {
        if(playerAnimator != null)
        playerAnimator.SetTrigger("Die");
        isDisableControl = true;
    }

    void OnFinishRevive(object[] datas)
    {
        playerRb.linearVelocity = new Vector2(0, jumpForce);
    }
}
