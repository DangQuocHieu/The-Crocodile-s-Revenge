using UnityEngine;

public class RandomPowerUp : MonoBehaviour
{
    [SerializeField] GameObject[] powerups;
    private void OnEnable()
    {
        Instantiate(powerups[Random.Range(0, powerups.Length)], transform.position, Quaternion.identity, this.transform);
    }
}
