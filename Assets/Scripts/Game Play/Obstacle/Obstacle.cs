using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Obstacle : MonoBehaviour, IDamaging, IDamageable, ICoinTransformable
{
    [SerializeField] int damage = 1;
    [SerializeField] float hurtDuration = 3f;
    [SerializeField] protected float destroyEffectDuration = 0.5f;
    [SerializeField] AudioName dealDamageSound, takeDamageSound;
    [SerializeField] bool destroyOnHit;
    [SerializeField] bool cooldownSFX = false;
    [SerializeField] GameObject[] effects;
    [SerializeField] GameObject coinPrefab;

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
        Observer.Notify(GameEvent.OnObstacleHitPlayer, damage);
        Observer.Notify(GameEvent.OnPlayerHurt, hurtDuration);

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
        AudioManager.Instance.PlaySFX(takeDamageSound, isCooldown: cooldownSFX);
        Destroy(gameObject);
    }

    public void TransformToCoins()
    {
        Instantiate(coinPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
