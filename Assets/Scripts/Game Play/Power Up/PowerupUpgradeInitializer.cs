using UnityEngine;

public class PowerupUpgradeInitializer : MonoBehaviour
{
    [SerializeField] PowerupData[] datas;
    [SerializeField] RectTransform parentUI;
    [SerializeField] GameObject powerupUpgradeUIPrefab;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        foreach(var data in datas)
        {
            PowerupUpgradeController current = Instantiate(powerupUpgradeUIPrefab, parentUI).GetComponent<PowerupUpgradeController>();
            current.SetProperties(data, canvasGroup);
        }
    }
}
