using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    Material material;
    float distance;
    [Range(0f, 1f)]
    [SerializeField] float speed = 0.2f;
    private float currentSpeed;

    private void Awake()
    {
        currentSpeed = speed;
        material = GetComponent<Renderer>().material;
        Observer.AddObserver(GameEvent.OnGameOver, StopParallax);
    }

    private void Update()
    {
        distance += Time.deltaTime * currentSpeed;
        material.SetTextureOffset("_MainTex", Vector2.right * distance);
    }

    void StopParallax(object[] datas)
    {
        currentSpeed = 0f;
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnGameOver, StopParallax);
    }
}
