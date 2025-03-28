
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager: Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
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


    void OnGameOver(object[] datas)
    {
        float delayDuration = (float)datas[0];
        StartCoroutine(OnGameOverCoroutine(delayDuration));
    }

    IEnumerator OnGameOverCoroutine(float duration)
    {
        AudioManager.Instance.StopMusic();
        yield return PlayFabManager.Instance.SendLeaderboard(StatisticsManager.Instance.DistanceSoFar);
        yield return PlayFabManager.Instance.AddCoins(GameScreenController.Instance.CoinCollected);
        yield return PlayFabManager.Instance.GetVirtualCurrencies();
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration);
        AudioManager.Instance.PlayGameOverSFX();
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
        StartCoroutine(UITransitionController.SlideAndScaleTransition(OnGameRestartCallback()));

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
        StartCoroutine(UITransitionController.SlideAndScaleTransition(OnGobackToHomeScreenCallback()));
    }   

    IEnumerator OnGobackToHomeScreenCallback()
    {
        ScreenManager.Instance.GoBack(isShowPrevScreen: true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Menu Scene");
        while(!asyncOperation.isDone)
        {
            yield return null;
        }
        if(asyncOperation.isDone)
        {
            AudioManager.Instance.PlayMenuMusic(null);
        }
    }


}
