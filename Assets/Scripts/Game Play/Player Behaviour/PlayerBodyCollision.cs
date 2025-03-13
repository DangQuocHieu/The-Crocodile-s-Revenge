using System.Collections;
using UnityEngine;

public class PlayerBodyCollision : MonoBehaviour
{
    [SerializeField] float hurtDuration;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool isInvicible = false;
    
    protected void Awake()
    {
        Observer.AddObserver(GameEvent.OnPlayerHurt, OnHurt);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnPlayerHurt, OnHurt);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICollectible collectible = collision.gameObject.GetComponent<ICollectible>();
        if (collectible != null)
        {
            collectible.OnCollect();
        }
        IDamaging damaging = collision.gameObject.GetComponent<IDamaging>();
        if (damaging != null && !isInvicible)
        {
            damaging.DealDamage(gameObject.transform);
        }
        if(collision.gameObject.CompareTag("Dead Zone"))
        {
            Observer.Notify(GameEvent.OnPlayerHurt);
            float delayDuration = 2f;
            Observer.Notify(GameEvent.OnGameOver, delayDuration);
        }
        
    }
    void OnHurt(object[] datas)
    {
        int currentHealth = GetComponent<PlayerHealthManager>().CurrentHealth;
        Debug.Log(currentHealth);
        if(currentHealth <= 0)
        {
            return;
        }
        StartCoroutine(OnHurtCoroutine());
    }
    IEnumerator OnHurtCoroutine()
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
    }
}
