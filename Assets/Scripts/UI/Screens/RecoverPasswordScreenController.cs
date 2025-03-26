using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecoverPasswordScreenController : UIScreen
{
    [SerializeField] TMPro.TMP_InputField emailInputField;
    [SerializeField] Button backButton;
    [SerializeField] Button recoverButton;
    [SerializeField] TextMeshProUGUI messageText;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        recoverButton.onClick.AddListener(() =>
        {
            RecoverPassword();
            canvasGroup.interactable = false;
            
        });
        backButton.onClick.AddListener(() =>
        {
            ScreenManager.Instance.GoBack(isShowPrevScreen: true);
            canvasGroup.interactable = false;
        });
    }
    public override IEnumerator Show()
    {
        gameObject.SetActive(true);
        canvasGroup.interactable = true;
        yield return null;
    }

    public override IEnumerator Hide()
    {
        gameObject.SetActive(false);
        yield return null;
    }

    void RecoverPassword()
    {
        Observer.Notify(GameEvent.OnRecoverPassword, emailInputField.text);
    }
}
