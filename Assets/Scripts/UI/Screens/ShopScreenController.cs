using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopScreenController : UIScreen
{
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] Button backButton;

    private CanvasGroup canvasGroup;
    private void Awake()
    {
        backButton.onClick.AddListener(() =>
        {
            ScreenManager.Instance.GoBack(isShowPrevScreen: true);
            canvasGroup.interactable = false;
        });
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
    }

    void Update()
    {
        coinsText.text = PlayFabManager.Instance.Coins.ToString();
    }
    public override IEnumerator Show()
    {
        gameObject.SetActive(true);
        HatSystemManager.Instance.HightLightEquippedHat();
        yield return StartCoroutine(UITransitionController.SlideTransition(ShowCallback()));
    }

    IEnumerator ShowCallback()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        yield return null;
    }
    public override IEnumerator Hide()
    {
        yield return StartCoroutine(UITransitionController.SlideTransition(HideCallback()));
        gameObject.SetActive(false);
    }
    
    IEnumerator HideCallback()
    {
        yield return null;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
    }
}
