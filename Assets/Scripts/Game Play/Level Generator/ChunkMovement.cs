using UnityEngine;

public class ChunkMovement : MonoBehaviour, IDifficultyScaler
{
    [SerializeField] float baseSpeed = 10f;
    [SerializeField] float chunkSpeed;
    private bool disableMoving = false;

    private void Awake()
    {
        Observer.AddObserver(GameEvent.OnGameOver, StopMoving);
        Observer.AddObserver(GameEvent.OnPlayerBeginRevive, StopMoving);
        Observer.AddObserver(GameEvent.OnPlayerFinishRevive, ContinueMoving);
        Observer.AddObserver(GameEvent.OnGamePaused, StopMoving);
        Observer.AddObserver(GameEvent.OnGameResume, ContinueMoving);
        Observer.AddObserver(GameEvent.OnPlayerFallIntoAHole, StopMoving);
        Observer.AddObserver(GameEvent.OnPlayerLand, ContinueMoving);
        Observer.AddObserver(GameEvent.OnGameDifficultyIncreasing, OnIncreaseDifficulty);
    }
    private void FixedUpdate()
    {
        if (disableMoving) return;
        transform.Translate(Vector2.left * chunkSpeed * Time.fixedDeltaTime);
    }
    private void StopMoving(object[] datas)
    {
        disableMoving = true;
    }

    void ContinueMoving(object[] datas)
    {
        disableMoving = false;
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnGameOver, StopMoving);
        Observer.RemoveListener(GameEvent.OnPlayerBeginRevive, StopMoving);
        Observer.RemoveListener(GameEvent.OnPlayerFinishRevive, ContinueMoving);
        Observer.RemoveListener(GameEvent.OnGamePaused, StopMoving);
        Observer.RemoveListener(GameEvent.OnGameResume, ContinueMoving);
        Observer.RemoveListener(GameEvent.OnPlayerFallIntoAHole, StopMoving);
        Observer.RemoveListener(GameEvent.OnPlayerLand, ContinueMoving);
        Observer.RemoveListener(GameEvent.OnGameDifficultyIncreasing, OnIncreaseDifficulty);
    }

    public void OnIncreaseDifficulty(object[] datas)
    {
        float t = (float)datas[0];
        float difficultyScale = (float)datas[1];
        chunkSpeed = Mathf.Lerp(baseSpeed, baseSpeed * difficultyScale, t);
    }

    
}
