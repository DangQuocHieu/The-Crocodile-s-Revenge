using System.Collections;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] float timeElapsed;
    [SerializeField] float maxDifficultyDuration;
    private void Awake()
    {
        timeElapsed = 0f;
    }
    void Update()
    {
        IncreaseGameDifficulty();
    }

    void IncreaseGameDifficulty()
    {
        if(timeElapsed < maxDifficultyDuration)
        {
            timeElapsed += Time.deltaTime;
        }
        else
        {
            timeElapsed = maxDifficultyDuration;
        }
        Observer.Notify(GameEvent.OnGameDifficultyIncreasing, timeElapsed, maxDifficultyDuration);
    }

    
}
