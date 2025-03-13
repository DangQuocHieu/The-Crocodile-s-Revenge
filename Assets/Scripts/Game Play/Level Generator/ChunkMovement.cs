using UnityEngine;

public class ChunkMovement : MonoBehaviour
{
    [SerializeField] float chunkSpeed = 12f;
    float currentChunkSpeed;

    private void Awake()
    {
        currentChunkSpeed = chunkSpeed;
        Observer.AddObserver(GameEvent.OnGameOver, StopMoving);
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector2.left * currentChunkSpeed * Time.fixedDeltaTime);
    }

    private void StopMoving(object[] datas)
    {
        currentChunkSpeed = 0;
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnGameOver, StopMoving);
    }
}
