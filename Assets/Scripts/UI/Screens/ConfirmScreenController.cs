using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmScreenController : UIScreen
{
    private RectTransform rectTransform;
    [SerializeField] GameObject overlay;
    [SerializeField] Button backButton;
    [SerializeField] TextMeshProUGUI messageText;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        backButton.onClick.AddListener(()=>{
            ScreenManager.Instance.HidePopUp();
            Observer.Notify(GameEvent.OnConfirmScreenClosed);
        });
    }
    public override IEnumerator Hide()
    {
        yield return UIAnimationController.ScaleUIAnimation(rectTransform, isIn: false).WaitForCompletion();
        overlay.SetActive(false);
        gameObject.SetActive(false);
    }

    public override IEnumerator Show()
    {
        gameObject.SetActive(true);
        overlay.SetActive(true);
        yield return UIAnimationController.ScaleUIAnimation(rectTransform, isIn: true).WaitForCompletion();
    }

    public void SetMessage(string message)
    {
        messageText.text = message;
    }
}
