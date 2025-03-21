using UnityEngine;

public class ChunkMovement : MonoBehaviour
{
    [SerializeField] float chunkSpeed = 12f;
    float currentChunkSpeed;
    private bool disableMoving = false;

    private void Awake()
    {
        currentChunkSpeed = chunkSpeed;
        Observer.AddObserver(GameEvent.OnGameOver, StopMoving);
        Observer.AddObserver(GameEvent.OnPlayerBeginRevive, StopMoving);
        Observer.AddObserver(GameEvent.OnPlayerFinishRevive, ContinueMoving);
        Observer.AddObserver(GameEvent.OnGamePaused, StopMoving);
        Observer.AddObserver(GameEvent.OnGameResume, ContinueMoving);
    }
    private void FixedUpdate()
    {
        if (disableMoving) return;
        transform.Translate(Vector2.left * currentChunkSpeed * Time.fixedDeltaTime);
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
    }

}
