using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HatItemUI : MonoBehaviour
{
    [SerializeField] Image hatImage;
    [SerializeField] TextMeshProUGUI priceText;
    private CanvasGroup canvasGroup;
    [SerializeField] private HatCustomizeData data;
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(()=>{
            Observer.Notify(GameEvent.OnPlayerSelectHatItem, data);
        });
    }

    void Update()
    {
        Display();
    }

    void Display()
    {
        if(data == null) return;
        hatImage.sprite = data.hatSprite;
        priceText.text = data.price.ToString();
    }

    public void SetProperties(HatCustomizeData data, CanvasGroup canvasGroup)
    {
        this.data = data;
        this.canvasGroup = canvasGroup;
    }

    

}
