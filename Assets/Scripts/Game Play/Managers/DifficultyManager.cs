using System.Collections;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] float timeElapsed;
    [SerializeField] float maxDifficultyDuration;
    [SerializeField] float diffcultyScale = 1.5f;
    bool canIncrease = true;
    private void Awake()
    {
        timeElapsed = 0f;
        Observer.AddObserver(GameEvent.OnPlayerBeginRevive, OnBeginRevive);
        Observer.AddObserver(GameEvent.OnPlayerFinishRevive, OnFinishRevive);
    }

    void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnPlayerBeginRevive, OnBeginRevive);
        Observer.RemoveListener(GameEvent.OnPlayerFinishRevive, OnFinishRevive);
    }
    void Update()
    {
        IncreaseGameDifficulty();
    }
    void IncreaseGameDifficulty()
    {
        if(!canIncrease) return;
        if(timeElapsed < maxDifficultyDuration)
        {
            timeElapsed += Time.deltaTime;
        }
        else
        {
            timeElapsed = maxDifficultyDuration;
        }
        Observer.Notify(GameEvent.OnGameDifficultyIncreasing, timeElapsed / maxDifficultyDuration, diffcultyScale);
    }

    void OnBeginRevive(object[] datas)
    {
        canIncrease = false;
    }

    void OnFinishRevive(object[] datas)
    {
        canIncrease = true;
    }
    
}
