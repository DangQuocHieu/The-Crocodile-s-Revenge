using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            bool isInvicible = collision.gameObject.GetComponent<PlayerBodyCollision>().IsInvicible;
            if(!isInvicible)
            {
                Observer.Notify(GameEvent.OnObstacleHitPlayer, damage);
            }
            int currentHealth = collision.gameObject.GetComponent<PlayerHealthManager>().CurrentHealth;
            if (currentHealth == 0) return;
            Observer.Notify(GameEvent.OnPlayerBeginRevive);
        }
    }
}
