using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Obstacle : MonoBehaviour, IDamaging, IDamageable
{
    [SerializeField] int damage = 1;
    [SerializeField] protected float destroyEffectDuration = 0.5f;
    [SerializeField] AudioName dealDamageSound, takeDamageSound;
    [SerializeField] bool destroyOnHit;
    [SerializeField] GameObject[] effects;

    public virtual void DealDamage(Transform target)
    {
        if(effects.Length != 0)
        {
            GameObject effect = effects[Random.Range(0, effects.Length)];
            Destroy(Instantiate(effect, target.transform.position, effect.transform.rotation, target), destroyEffectDuration);
        }
        if(dealDamageSound != AudioName.None)
        {
            AudioManager.Instance.PlaySFX(dealDamageSound);
        }
        Observer.Notify(GameEvent.OnPlayerHurt);
        Observer.Notify(GameEvent.OnObstacleHitPlayer, damage);
        if(destroyOnHit)
        {
            Destroy(gameObject);
        }
    }

    public virtual void TakeDamage()
    {
        if(effects.Length != 0)
        {
            GameObject effect = effects[Random.Range(0, effects.Length)];
            Destroy(Instantiate(effect, transform.position, effect.transform.rotation), destroyEffectDuration);
        }
        AudioManager.Instance.PlaySFX(takeDamageSound, isCooldown: true);
        Destroy(gameObject);
    }

}
