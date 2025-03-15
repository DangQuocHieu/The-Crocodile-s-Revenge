using TMPro;
using UnityEngine;

public class PlayerHealthManager : Singleton<PlayerHealthManager>
{
    [SerializeField] int maxHealth;
    int currentHealth;

    public int CurrentHealth => currentHealth;
    protected override void Awake()
    {
        currentHealth = maxHealth;
        Observer.AddObserver(GameEvent.OnObstacleHitPlayer, TakeDamage);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnObstacleHitPlayer, TakeDamage);
    }
    void Update()
    {
        Observer.Notify(GameEvent.OnUpdateHealthUI, currentHealth);
    }

    void TakeDamage(object[] damage)
    {
        currentHealth -= (int)damage[0];
        if (currentHealth < 0) return;
        if(currentHealth == 0)
        {
            float delayDuration = 2f;
            Observer.Notify(GameEvent.OnGameOver, delayDuration);
        }
    }
}
