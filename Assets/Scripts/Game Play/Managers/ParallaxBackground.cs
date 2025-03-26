using UnityEngine;

public class ParallaxBackground : MonoBehaviour, IDifficultyScaler
{
    Material material;
    float distance;
    [Range(0f, 1f)]
    [SerializeField] float speed = 0.2f;
    private float baseSpeed;
    private bool disableMoving = false;

    private void Awake()
    {
        baseSpeed = speed;
        material = GetComponent<Renderer>().material;
        Observer.AddObserver(GameEvent.OnGameOver, StopParallax);
        Observer.AddObserver(GameEvent.OnPlayerBeginRevive, StopParallax);
        Observer.AddObserver(GameEvent.OnPlayerFinishRevive, ContinueParallax);
        Observer.AddObserver(GameEvent.OnGameDifficultyIncreasing, OnIncreaseDifficulty);
    }

    private void Update()
    {
        if (disableMoving) return;
        distance += Time.deltaTime * speed;
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
        Observer.RemoveListener(GameEvent.OnGameDifficultyIncreasing, OnIncreaseDifficulty);
    }

    public void OnIncreaseDifficulty(object[] datas)
    {
        float t = (float)datas[0];
        float diffcultyScale = (float)datas[1];
        speed = Mathf.Lerp(baseSpeed, baseSpeed * diffcultyScale, t);
    }
}
