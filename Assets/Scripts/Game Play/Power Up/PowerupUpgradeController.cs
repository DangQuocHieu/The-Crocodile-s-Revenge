using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupUpgradeController : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] RectTransform durationTextContainer;
    [SerializeField] TextMeshProUGUI currentDurationText;
    [SerializeField] TextMeshProUGUI nextDurationText;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] Button upgradeButton;

    private CanvasGroup parentCanvasGroup;
    private PowerupData data;
    private Coroutine currentCoroutine;


    void Awake()
    {
        upgradeButton.onClick.AddListener(()=>{
            OnUpgrade();
            parentCanvasGroup.interactable = false;

        });
    }

    void Update()
    {
        Display();
    }

    public void SetProperties(PowerupData data, CanvasGroup canvasGroup)
    {
        this.data = data;
        this.parentCanvasGroup = canvasGroup;
    }
    public void Display()
    {
        if(data == null) return;
        int currentLevel = GetCurrentLevel(data.type);
        float duration = data.baseDuration + data.durationIncreasePerLevel * currentLevel;
        
        image.sprite = data.sprite;
        currentDurationText.text = duration.ToString() + "s";
        if(currentLevel < data.maxLevel)
        {
            nextDurationText.text = (duration + 1).ToString() + "s";
            priceText.text = data.prices[currentLevel].ToString() + " coins";
        }
        else
        {
            priceText.text = "Max Level";
            durationTextContainer.gameObject.SetActive(false);
            upgradeButton.gameObject.SetActive(false);
        }
    }

    int GetCurrentLevel(PowerupType type)
    {
        GameData gameData = DataPersistenceManager.Instance.GameData;
        switch(type)
        {
            case PowerupType.DoubleCoin:
                return gameData.doublecoinLevel;
            case PowerupType.Shield:
                return gameData.shieldLevel;
            case PowerupType.Magnet:
                return gameData.magnetLevel;
            case PowerupType.TripleJump:
                return gameData.tripleJumpLevel;
        }
        return -1;
    }

    public void OnUpgrade()
    {
        if(currentCoroutine != null) return;
        int currentLevel = GetCurrentLevel(data.type);
        if(currentLevel == data.maxLevel) {
            return;
        }
        int coinToBuy = data.prices[currentLevel];
        if(PlayFabManager.Instance.Coins < coinToBuy || currentLevel == data.maxLevel)
        {
            Observer.Notify(GameEvent.OnPlayFabError, "Not enough coins");
            Debug.Log("CAN'T BUY");
        }
        else
        {
            currentCoroutine = StartCoroutine(OnUpgradeCoroutine(coinToBuy));
            Debug.Log("CAN BUY");
        }
    }

    IEnumerator OnUpgradeCoroutine(int coinToBuy)
    {
        yield return PlayFabManager.Instance.BuyItem(coinToBuy);
        yield return PlayFabManager.Instance.GetVirtualCurrencies();
        DataPersistenceManager.Instance.Upgrade(data.type);
        yield return PlayFabManager.Instance.SavePlayerData();
        Observer.Notify(GameEvent.OnConfirmNotification, "Successfully upgraded !");
        currentCoroutine = null;
    }
}
