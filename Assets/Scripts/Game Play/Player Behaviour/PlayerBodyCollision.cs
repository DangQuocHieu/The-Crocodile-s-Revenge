using System.Collections;
using UnityEngine;

public class PlayerBodyCollision : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool isInvicible = false;
    public bool IsInvicible => isInvicible;
    private Coroutine hurtCoroutine;
    
    protected void Awake()
    {
        Observer.AddObserver(GameEvent.OnPlayerHurt, OnHurt);
        Observer.AddObserver(GameEvent.OnPlayerBeginRevive, StopOnHurtCoroutine);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnPlayerHurt, OnHurt);
        Observer.RemoveListener(GameEvent.OnPlayerBeginRevive, StopOnHurtCoroutine);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICollectible collectible = collision.gameObject.GetComponent<ICollectible>();
        if (collectible != null)
        {
            collectible.OnCollect();
        }
        IDamaging damaging = collision.gameObject.GetComponent<IDamaging>();
        int currentHealth = gameObject.GetComponent<PlayerHealthManager>().CurrentHealth;
        if (damaging != null && !isInvicible && currentHealth > 0)
        {
            damaging.DealDamage(gameObject.transform);
        }        
    }
    void OnHurt(object[] datas)
    {
        float hurtDuration = (float)(datas[0]);
        int currentHealth = GetComponent<PlayerHealthManager>().CurrentHealth;
        Debug.Log(currentHealth);
        if(currentHealth <= 0)
        {
            return;
        }
        if(hurtCoroutine != null)
        {
            StopCoroutine(hurtCoroutine);
        }
        hurtCoroutine = StartCoroutine(OnHurtCoroutine(hurtDuration));
    }
    IEnumerator OnHurtCoroutine(float hurtDuration)
    {
        isInvicible = true;
        float elapsedTime = 0f;
        float blinkInterval = 0.1f;
        while (elapsedTime < hurtDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }
        spriteRenderer.enabled = true;
        isInvicible = false;
        hurtCoroutine = null;
    }

    void StopOnHurtCoroutine(object[] datas)
    {
        if(hurtCoroutine != null)
        {
            StopCoroutine(hurtCoroutine);
            hurtCoroutine = null;
            spriteRenderer.enabled = true;
        }
    }

}
