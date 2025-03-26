using UnityEngine;

public class StatisticsManager : Singleton<StatisticsManager>, IDifficultyScaler
{
    [SerializeField] float baseDistacneMultiple = 2f;
    private float currentDistanceMultiple;
    private float distanceSoFar = 0;
    private float timeElapsed = 0f; 
    bool stopUpdate = false;

    public int DistanceSoFar
    {
        get { return Mathf.RoundToInt(distanceSoFar); }
    }

    public float TimeEpalsed => timeElapsed;

    protected override void Awake()
    {
        instance = this;
        Observer.AddObserver(GameEvent.OnGameOver, StopUpdateStatistics);
        Observer.AddObserver(GameEvent.OnGameDifficultyIncreasing, OnIncreaseDifficulty);
        Observer.AddObserver(GameEvent.OnPlayerBeginRevive, StopUpdateStatistics);
        Observer.AddObserver(GameEvent.OnPlayerFinishRevive, ContinueUpdateStatistics);
    }

    void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnGameOver, StopUpdateStatistics);
        Observer.RemoveListener(GameEvent.OnGameDifficultyIncreasing, OnIncreaseDifficulty);
        Observer.RemoveListener(GameEvent.OnPlayerFinishRevive, StopUpdateStatistics);
        Observer.AddObserver(GameEvent.OnPlayerFinishRevive, ContinueUpdateStatistics);
        
    }
    void Update()
    {
        if (stopUpdate) return;
        timeElapsed += Time.deltaTime;
        distanceSoFar += Time.deltaTime * currentDistanceMultiple;
        Observer.Notify(GameEvent.OnUpdateGameStatisticUI, timeElapsed, distanceSoFar);
    }

    void StopUpdateStatistics(object[] datas)
    {
        stopUpdate = true;
    }

    void ContinueUpdateStatistics(object[] datas)
    {
        stopUpdate = false;
    }

    public void OnIncreaseDifficulty(object[] datas)
    {
        float t = (float)datas[0];
        float difficultyScale = (float)datas[1];
        currentDistanceMultiple = Mathf.Lerp(baseDistacneMultiple, baseDistacneMultiple * difficultyScale, t);
    }
}
