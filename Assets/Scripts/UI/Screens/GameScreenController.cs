using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] GameObject warningImagePrefab;
    [SerializeField] RectTransform warningImageContainer;
    [SerializeField] float warningSpawnOffsetX = 10f;
    int coinCollected = 0;

    private Queue<GameObject> warningImageSpawned = new Queue<GameObject>();
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
        Observer.AddObserver(GameEvent.OnVehicleWarning, OnVehicleWarning);
        Observer.AddObserver(GameEvent.OnDisableVehicleWarning, OnDisableWarning);
        base.Awake();
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnGameResume, EnablePauseButton);
        Observer.RemoveListener(GameEvent.OnPlayerPickUpCoin, UpdateCoinUI);
        Observer.RemoveListener(GameEvent.OnUpdateHealthUI, UpdateHealthUI);
        Observer.RemoveListener(GameEvent.OnGameOver, DisableUI);
        Observer.RemoveListener(GameEvent.OnVehicleWarning, OnVehicleWarning);
        Observer.RemoveListener(GameEvent.OnDisableVehicleWarning, OnDisableWarning);
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

    public void OnVehicleWarning(object[] datas)
    {
        Vector3 vehiclePositionY = (Vector3)datas[0];
        float screenPosY = Camera.main.WorldToScreenPoint(vehiclePositionY).y;
        Vector3 spawnPostition = new Vector3(Screen.width - warningSpawnOffsetX, screenPosY);
        warningImageSpawned.Enqueue(Instantiate(warningImagePrefab, spawnPostition, Quaternion.identity, warningImageContainer));
    }
    
    public void OnDisableWarning(object[] datas)
    {
        GameObject currentWarningGO = warningImageSpawned.Dequeue();
        Destroy(currentWarningGO);
    }
}
