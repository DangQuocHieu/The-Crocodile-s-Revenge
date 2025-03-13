using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupUIManager : MonoBehaviour
{
    [SerializeField] RectTransform doubleCoinUI;
    [SerializeField] RectTransform magnetUI;
    [SerializeField] RectTransform shieldUI;
    [SerializeField] RectTransform tripleJumpUI;
    Dictionary<PowerupType, RectTransform> powerupUIs = new Dictionary<PowerupType, RectTransform>();
    Dictionary<PowerupType, Slider> activeSlider = new Dictionary<PowerupType, Slider>();
    Dictionary<PowerupType, Coroutine> activeCoroutine = new Dictionary<PowerupType, Coroutine>();
    private void Awake()
    {
        powerupUIs.Add(PowerupType.DoubleCoin, doubleCoinUI);
        powerupUIs.Add(PowerupType.Magnet, magnetUI);
        powerupUIs.Add(PowerupType.Shield, shieldUI);
        powerupUIs.Add(PowerupType.TripleJump, tripleJumpUI);
        Observer.AddObserver(GameEvent.OnPlayerPickUpPowerup, ActiveCountdownUI);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnPlayerPickUpPowerup,ActiveCountdownUI);
    }
    void ActiveCountdownUI(object[] datas)
    {
        PowerupType type = (PowerupType)datas[0];
        float duration = (float)datas[1];   
        if(activeCoroutine.ContainsKey(type))
        {
            //Refresh
            Refresh(type, duration);
            return;
        }
        RectTransform rect = Instantiate(powerupUIs[type], this.transform);
        Slider slider = rect.GetChild(0).GetComponent<Slider>();
        Countdown(type, duration, slider);
    }

    void Countdown(PowerupType type, float duration, Slider slider)
    {
        Coroutine coroutine = StartCoroutine(CountdownCoroutine(duration,type, slider));
        activeCoroutine.Add(type, coroutine);
    }

    IEnumerator CountdownCoroutine(float duration, PowerupType type, Slider slider)
    {
        if(!activeSlider.ContainsKey(type))
        {
            activeSlider.Add(type, slider);
        }
        float timeLeft = duration;
        while(timeLeft > 0)
        {
            slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, timeLeft / duration);
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        slider.value = slider.minValue;
        activeCoroutine.Remove(type);
        activeSlider.Remove(type);
        Destroy(slider.transform.parent.gameObject);
    }

    void Refresh(PowerupType type, float duration)
    {
        Coroutine current = activeCoroutine[type];
        StopCoroutine(current);
        activeCoroutine.Remove(type);
        Slider currentSlider = activeSlider[type];
        Countdown(type, duration, currentSlider);
    }
}
