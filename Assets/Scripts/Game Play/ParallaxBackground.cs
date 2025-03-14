using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    Material material;
    float distance;
    [Range(0f, 1f)]
    [SerializeField] float speed = 0.2f;
    private float currentSpeed;
    private bool disableMoving = false;

    private void Awake()
    {
        currentSpeed = speed;
        material = GetComponent<Renderer>().material;
        Observer.AddObserver(GameEvent.OnGameOver, StopParallax);
        Observer.AddObserver(GameEvent.OnPlayerBeginRevive, StopParallax);
        Observer.AddObserver(GameEvent.OnPlayerFinishRevive, ContinueParallax);
    }

    private void Update()
    {
        if (disableMoving) return;
        distance += Time.deltaTime * currentSpeed;
        material.SetTextureOffset("_MainTex", Vector2.right * distance);
    }

    void StopParallax(object[] datas)
    {
        disableMoving = true;
    }

    void ContinueParallax(object[] datas)
    {
        disableMoving = false;
    }
    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnGameOver, StopParallax);
        Observer.RemoveListener(GameEvent.OnPlayerBeginRevive, StopParallax);
        Observer.RemoveListener(GameEvent.OnPlayerFinishRevive, ContinueParallax);
    }
}
