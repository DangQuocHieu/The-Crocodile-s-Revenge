using UnityEngine;

public class RandomCoin : MonoBehaviour
{
    [SerializeField] GameObject[] coinPrefabs;
    [SerializeField] float appearanceRate = 0.5f;
    private void OnEnable()
    {
        if(Random.value > appearanceRate)
        {
            gameObject.SetActive(false);
            return;
        }
        Instantiate(coinPrefabs[Random.Range(0, coinPrefabs.Length)], transform.position, Quaternion.identity, this.transform); 
    }
}
