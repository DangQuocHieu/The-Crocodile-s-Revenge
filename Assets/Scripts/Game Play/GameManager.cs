using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager: Singleton<GameManager>
{
    [SerializeField] float distanceMultiple = 2f;
    private float distanceSoFar = 0;
    private float timeElapsed = 0f;
    bool isGameOver = false;

    public int DistanceSoFar
    {
        get { return Mathf.FloorToInt(distanceSoFar); }
    }
    public float TimeEpalsed => timeElapsed;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        Time.timeScale = 1;
        Observer.AddObserver(GameEvent.OnGameOver, OnGameOver);
        Observer.AddObserver(GameEvent.OnGamePaused, OnGamePaused);
        Observer.AddObserver(GameEvent.OnGameResume, OnGameResume);
        Observer.AddObserver(GameEvent.OnGameRestart, OnGameRestart);
        Observer.AddObserver(GameEvent.OnGobackToHomeScreen, OnGobackToHomeScreen);
    }
    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnGameOver, OnGameOver);
        Observer.RemoveListener(GameEvent.OnGamePaused, OnGamePaused);
        Observer.RemoveListener(GameEvent.OnGameResume, OnGameResume);
        Observer.RemoveListener(GameEvent.OnGameRestart, OnGameRestart);
        Observer.RemoveListener(GameEvent.OnGobackToHomeScreen, OnGobackToHomeScreen);
    }

    private void Update()
    {
        if (isGameOver) return;
        timeElapsed += Time.deltaTime;
        distanceSoFar += Time.deltaTime * distanceMultiple;
        Observer.Notify(GameEvent.OnUpdateGameStatisticUI, timeElapsed, distanceSoFar);
    }

    void OnGameOver(object[] datas)
    {
        PlayFabManager.Instance.SendLeaderboard(DistanceSoFar);
        isGameOver = true;
        float delayDuration = (float)datas[0];
        StartCoroutine(OnGameOverCoroutine(delayDuration));
    }

    IEnumerator OnGameOverCoroutine(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        ScreenManager.Instance.TransitionTo(ScreenID.GameOverScreen);
        Time.timeScale = 1;
    }
    void OnGamePaused(object[] datas)
    {
        Time.timeScale = 0;
        ScreenManager.Instance.TransitionTo(ScreenID.PauseScreen);
    }

     void OnGameResume(object[] datas)
    {
        ScreenManager.Instance.GoBack();
        StartCoroutine(ResumeGameCoroutine((float)datas[0]));
    }

    IEnumerator ResumeGameCoroutine(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }

    void OnGameRestart(object[] datas)
    {
        Debug.Log("Game Restart");
        Time.timeScale = 1f;
        StartCoroutine(UITransitionController.SlideTransition(OnGameRestartCallback()));

    }
    IEnumerator OnGameRestartCallback()
    {
        ScreenManager.Instance.GoBack();
        AsyncOperation asyncOperation =  SceneManager.LoadSceneAsync("Game Scene");
        while(!asyncOperation.isDone)
        {
            yield return null;
        }
    }
    void OnGobackToHomeScreen(object[] datas)
    {
        Time.timeScale = 1f;
        StartCoroutine(UITransitionController.SlideTransition(OnGobackToHomeScreenCallback()));
    }   

    IEnumerator OnGobackToHomeScreenCallback()
    {
        ScreenManager.Instance.GoBack(isShowPrevScreen: true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Menu Scene");
        while(!asyncOperation.isDone)
        {
            yield return null;
        }
    }


}
