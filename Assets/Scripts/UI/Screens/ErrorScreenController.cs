using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class ErrorScreenController : UIScreen
{
    [SerializeField] TextMeshProUGUI errorText;
    [SerializeField] Button backButton;
    [SerializeField] RectTransform overlay;
    private RectTransform rectTransform;

    void Awake()
    {
        backButton.onClick.AddListener(() =>
        {
            ScreenManager.Instance.HidePopUp();
        });
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetErrorMessage(string message)
    {
        errorText.text = message;
    }

    public void SetBackButton(bool active)
    {
        backButton.gameObject.SetActive(active);
    }


    public override IEnumerator Show()
    {   
        gameObject.SetActive(true);
        overlay.gameObject.SetActive(true);
        yield return UIAnimationController.ScaleUIAnimation(rectTransform, isIn: true).WaitForCompletion();
    }
    public override IEnumerator Hide()
    {
        yield return UIAnimationController.ScaleUIAnimation(rectTransform, isIn: false).WaitForCompletion();
        gameObject.SetActive(false);
        overlay.gameObject.SetActive(false);
    }
}
