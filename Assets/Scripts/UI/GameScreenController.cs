using System;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameScreenController : Singleton<GameScreenController>
{
    [SerializeField] Button pauseButton;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI distanceText;

    int coinCollected = 0;
    public int CoinCollected
    {
        get
        {
            return coinCollected;
        }
    }
    protected override void Awake()
    {
        coinText.text = "";
        pauseButton.onClick.AddListener(() => { 
            Observer.Notify(GameEvent.OnGamePaused);
            pauseButton.gameObject.SetActive(false);
        });

        Observer.AddObserver(GameEvent.OnGameResume, EnablePauseButton);
        Observer.AddObserver(GameEvent.OnPlayerPickUpCoin, UpdateCoinUI);
        Observer.AddObserver(GameEvent.OnUpdateHealthUI, UpdateHealthUI);
        Observer.AddObserver(GameEvent.OnUpdateGameStatisticUI, UpdateStatisticUI);
        Observer.AddObserver(GameEvent.OnGameOver, DisableUI);
        base.Awake();
    }

    void EnablePauseButton(object[] datas)
    {
        if(pauseButton.gameObject != null)
        {
            pauseButton.gameObject.SetActive(true);
        }
    }

    void UpdateCoinUI(object[] datas)
    {
        int coinToAdd = (int)datas[0];
        coinCollected += coinToAdd;
        coinText.text = "x" + coinCollected.ToString();
    }

    void UpdateHealthUI(object[] datas)
    {
        int currentHealth = (int)datas[0];
        healthText.text = "x" + currentHealth.ToString();
    }
    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnGameResume, EnablePauseButton);
        Observer.RemoveListener(GameEvent.OnPlayerPickUpCoin, UpdateCoinUI);
        Observer.RemoveListener(GameEvent.OnUpdateHealthUI, UpdateHealthUI);
        Observer.RemoveListener(GameEvent.OnGameOver, DisableUI);
    }
    void UpdateStatisticUI(object[] datas)
    {
        float elapsedTime = (float)datas[0];
        float distanceSofar = (float)datas[1];
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int ticks = Mathf.FloorToInt((elapsedTime * 1000) % 1000 / 10);
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, ticks);
        distanceText.text = Mathf.FloorToInt(distanceSofar).ToString() + "m";
    }

    void DisableUI(object[] datas)
    {
        gameObject.SetActive(false);
    }
}
