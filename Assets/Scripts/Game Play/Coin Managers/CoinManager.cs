//using TMPro;
//using UnityEngine;

//public class CoinManager : Singleton<CoinManager>, IDataPersistence
//{
//    int totalCoin;
//    public int TotalCoin => totalCoin;
//    protected override void Awake()
//    {
//        base.Awake();
//        DontDestroyOnLoad(gameObject);
//        Observer.AddObserver(GameEvent.OnPlayerPickUpCoin, OnPickUpCoin);
//    }

//    private void Update()
//    {

//    }
//    public void LoadData(GameData data)
//    {
//        this.totalCoin = data.totalCoin;
//    }

//    public void SaveData(GameData data)
//    {
//        data.totalCoin = this.totalCoin;
//    }

//    void OnPickUpCoin(object[] datas)
//    {
//        totalCoin += (int)datas[0];
//    }

//    private void OnDestroy()
//    {
//        Observer.RemoveListener(GameEvent.OnPlayerPickUpCoin, OnPickUpCoin);
//    }

//}
