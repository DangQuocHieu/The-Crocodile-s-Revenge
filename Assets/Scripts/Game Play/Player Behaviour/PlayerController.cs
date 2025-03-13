using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] Vector3 initPosition = new Vector3(0, -6, 0);
    [SerializeField] float jumpForce = 8f;
    [SerializeField] float speed = 10f;
    [SerializeField] Animator playerAnim;
    [SerializeField] CapsuleCollider2D bodyCollider;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float extraHeightCheck = 0.01f;
    int jumpCount;
    Rigidbody2D playerRb;
    PowerUpEffect powerUpEffect;
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
        Observer.AddObserver(GameEvent.OnGameOver, UpdateGameOverAnimation);
    }
    private void Update()
    {
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

    void ProcessJumping()
    {
        if (isDisableControl) return;
        if (!IsGrounded())
        {
            if (jumpCount == 0 || jumpCount >= powerUpEffect.CurrentMaxJumpCount) return;
        }
        playerRb.linearVelocity = new Vector2(0, jumpForce);
        playerAnim.SetBool("isRunning", false);
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
        playerAnim.SetBool("isMultipleJump", jumpCount >= powerUpEffect.CurrentMaxJumpCount);
    }

    void OnGrounded()
    {
        if(playerRb.linearVelocity.y > 0.00001f)
        {
            return;
        }
        bool isGrounded = IsGrounded();
        if(!wasGrounded && isGrounded)
        {
            jumpCount = 0;
            playerAnim.SetBool("isRunning", true);
        }
        wasGrounded = isGrounded;
    }

    async void UpdateGameOverAnimation(object[] datas)
    {
        if(playerAnim != null)
        playerAnim.SetTrigger("Die");
        await Task.Yield();
        isDisableControl = true;
    }

}
