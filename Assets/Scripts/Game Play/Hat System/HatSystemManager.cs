using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class HatSystemManager : Singleton<HatSystemManager>
{
    [Header("Hat Customize SO")]
    [SerializeField] private HatCustomizeData[] hatDatas;
    [SerializeField] private HatCustomizeData emptyHat;
    [SerializeField] private RectTransform hatItemContainer;
    [SerializeField] private GameObject hatItemPrefab;

    [Header("Buttons")]
    [SerializeField] private Button equipButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button equippedButton;

    [SerializeField] private CanvasGroup canvasGroup;
    private HatCustomizeData currentSelectedHat;
    private HatCustomizeData currentEquippedHat;
    private Coroutine currentCoroutine;
    private List<HatItemUI> hatItems = new List<HatItemUI>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        equipButton.onClick.AddListener(() => EquipHat(currentSelectedHat));
        buyButton.onClick.AddListener(BuyHat);
        Observer.AddObserver(GameEvent.OnPlayerSelectHatItem, OnPlayerSelectHatItem);
        Observer.AddObserver(GameEvent.OnLoginSuccessfully, OnLoginSuccessfully);
        Observer.AddObserver(GameEvent.OnConfirmScreenClosed, OnConfirmScreenClosed);
    }

    void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnPlayerSelectHatItem, OnPlayerSelectHatItem);
        Observer.RemoveListener(GameEvent.OnLoginSuccessfully, OnLoginSuccessfully);
        Observer.RemoveListener(GameEvent.OnConfirmScreenClosed, OnConfirmScreenClosed);
    }

    void OnLoginSuccessfully(object[] datas)
    {
        InitItems();
        LoadEquippedHat();
    }
    
    private void OnPlayerSelectHatItem(object[] datas)
    {
        currentSelectedHat = (HatCustomizeData)datas[0];
        UpdateButtonStates(currentSelectedHat);
        HightLight(currentSelectedHat);
    }

    private void UpdateButtonStates(HatCustomizeData data)
    {
        equippedButton.gameObject.SetActive(data == currentEquippedHat);
        buyButton.gameObject.SetActive(!data.owned && data != currentEquippedHat);
        equipButton.gameObject.SetActive(data.owned && data != currentEquippedHat);
    }

    private void InitItems()
    {
        List<CatalogItem> catalogItems = PlayFabManager.Instance.CatalogItems;
        List<string> ownedItems = PlayFabManager.Instance.OwnedHats;

        for (int i = 0; i < catalogItems.Count; i++)
        {
            hatDatas[i].owned = ownedItems.Contains(catalogItems[i].ItemId);
            hatDatas[i].price = catalogItems[i].VirtualCurrencyPrices["CN"];
            hatDatas[i].id = catalogItems[i].ItemId;

            HatItemUI current = Instantiate(hatItemPrefab, hatItemContainer).GetComponent<HatItemUI>();
            hatItems.Add(current);
            current.SetProperties(hatDatas[i], canvasGroup);
        }
    }

    private void BuyHat()
    {
        if (currentCoroutine != null || currentSelectedHat == null || currentSelectedHat.owned) return;

        if (PlayFabManager.Instance.Coins < currentSelectedHat.price)
        {
            Observer.Notify(GameEvent.OnPlayFabError, "Not enough coins!");
            return;
        }

        canvasGroup.interactable = false;
        currentCoroutine = StartCoroutine(OnBuyHatCoroutine(currentSelectedHat));
    }

    private IEnumerator OnBuyHatCoroutine(HatCustomizeData data)
    {
        yield return PlayFabManager.Instance.BuyItem(data.id, (int)data.price);
        yield return PlayFabManager.Instance.GetVirtualCurrencies();
        yield return PlayFabManager.Instance.GetCatalogItems();
        yield return PlayFabManager.Instance.GetOwnedHats();
        canvasGroup.interactable = true;
        currentCoroutine = null;
        EquipHat(data);
        ResetHatList();
        Observer.Notify(GameEvent.OnConfirmNotification, "Buy successful");
        
    }

    private void EquipHat(HatCustomizeData hatData)
    {
        currentEquippedHat = hatData;
        Observer.Notify(GameEvent.OnPlayerEquipHatItem, currentEquippedHat);
        DataPersistenceManager.Instance.GameData.hatEquippedId = currentEquippedHat.id;
        StartCoroutine(PlayFabManager.Instance.SavePlayerData());
        UpdateButtonStates(currentEquippedHat);
        HightLightEquippedHat();
    }

    void ResetHatList()
    {
        List<CatalogItem> catalogItems = PlayFabManager.Instance.CatalogItems;
        List<string> ownedItems = PlayFabManager.Instance.OwnedHats;
        for(int i = 0; i < catalogItems.Count; i++)
        {
            hatDatas[i].owned = ownedItems.Contains(catalogItems[i].ItemId);
            hatDatas[i].price = catalogItems[i].VirtualCurrencyPrices["CN"];
            hatDatas[i].id = catalogItems[i].ItemId;
            hatItems[i].SetProperties(hatDatas[i], canvasGroup);
        }
    }

    public void LoadEquippedHat()
    {
        string id = DataPersistenceManager.Instance.GameData.hatEquippedId;
        int index = Array.FindIndex(hatDatas, item => item.id == id);
        if (index >= 0)
        {
            currentSelectedHat = hatDatas[index];
        }
        else
        {
            currentSelectedHat = emptyHat;
        }
        EquipHat(currentSelectedHat);
    }

    private void HightLight(HatCustomizeData data)
    {
        int index = Array.FindIndex(hatDatas, item => item.id == data.id);
        if (index >= 0)
        {
            hatItemContainer.transform.GetChild(index + 1).GetComponent<Button>().Select();
        }
        else
        {
            hatItemContainer.transform.GetChild(0).GetComponent<Button>().Select();
        }
    }

    public void HightLightEquippedHat()
    {
        HightLight(currentEquippedHat);
    }

    void OnConfirmScreenClosed(object[] datas)
    {
        HightLightEquippedHat();
    }
}
