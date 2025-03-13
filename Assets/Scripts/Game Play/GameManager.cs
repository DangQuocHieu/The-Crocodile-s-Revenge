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
        Time.timeScale = 1f;
        Observer.AddObserver(GameEvent.OnGameOver, OnGameOver);
        Observer.AddObserver(GameEvent.OnGamePaused, OnGamePaused);
        Observer.AddObserver(GameEvent.OnGameResume, OnGameResume);
        Observer.AddObserver(GameEvent.OnGameRestart, OnGameRestart);
        Observer.AddObserver(GameEvent.OnGobackToHomeScreen, OnGobackToHomeScreen);
        base.Awake();
    }

    private void Update()
    {
        if (isGameOver) return;
        timeElapsed += Time.deltaTime;
        distanceSoFar += Time.deltaTime * distanceMultiple;
        Observer.Notify(GameEvent.OnUpdateGameStatisticUI, timeElapsed, distanceSoFar);
    }

    async void OnGameOver(object[] datas)
    {
        isGameOver = true;
        float delayDuration = (float)datas[0];
        await Task.Delay(TimeSpan.FromSeconds(delayDuration));
        if(ScreenManager.Instance != null)
        ScreenManager.Instance.TransitionTo(ScreenID.GameOverScreen);
        Time.timeScale = 0;
    }

    void OnGamePaused(object[] datas)
    {
        Time.timeScale = 0;
        ScreenManager.Instance.TransitionTo(ScreenID.PauseScreen);
    }

    async void OnGameResume(object[] datas)
    {
        ScreenManager.Instance.GoBack();
        await Task.Delay(TimeSpan.FromSeconds((float)datas[0]));
        Time.timeScale = 1;
    }

    void OnGameRestart(object[] datas)
    {
        UITransitionController.SlideAndScaleTransition(async () =>
        {
            Time.timeScale = 1f;
            await SceneManager.LoadSceneAsync("Game Scene");
            ScreenManager.Instance.GoBack();
        });
    }

    void OnGobackToHomeScreen(object[] datas)
    {
        UITransitionController.SlideAndScaleTransition(async () =>
        {
            Time.timeScale = 1;
            await SceneManager.LoadSceneAsync("Menu Scene");
            ScreenManager.Instance.GoBack(isHidePrevScreen: false);
        });
    }
    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnGameOver, OnGameOver);
        Observer.RemoveListener(GameEvent.OnGamePaused, OnGamePaused);
        Observer.RemoveListener(GameEvent.OnGameResume, OnGameResume);
        Observer.RemoveListener(GameEvent.OnGameRestart, OnGameRestart);
        Observer.RemoveListener(GameEvent.OnGobackToHomeScreen, OnGobackToHomeScreen);
    }

 
}
