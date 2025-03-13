using UnityEngine;

public class HeartUIManager : MonoBehaviour
{
    [SerializeField] GameObject heartImage;
    [SerializeField] Vector2 initPosition = Vector2.zero;
    [SerializeField] float xOffset = 50f;
    float playerHealth;
    private void Start()
    {
        InstantiateHeartImages();
    }

    void InstantiateHeartImages()
    {
        
        for (int i = 0; i < playerHealth; i++)
        {
            Instantiate(heartImage, new Vector2(initPosition.x + i * xOffset, initPosition.y), Quaternion.identity, this.transform);
        }
    }
}
