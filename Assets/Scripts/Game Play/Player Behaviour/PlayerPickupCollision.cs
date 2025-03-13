using UnityEngine;
using UnityEngine.Tilemaps;
public class PlayerPickupCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        ICollectible collectible = collision.gameObject.GetComponent<ICollectible>();
        if(collectible != null)
        {
            collectible.OnCollect();
        }
    }
}
