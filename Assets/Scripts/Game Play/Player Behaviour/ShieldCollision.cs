using UnityEngine;

public class ShieldCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if(damageable != null)
        {   
            damageable.TakeDamage();
        }
    }
}
