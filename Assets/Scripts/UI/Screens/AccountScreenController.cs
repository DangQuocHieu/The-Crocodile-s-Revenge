using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AccountScreenController : UIScreen
{
    [SerializeField] Button backButton;
    [SerializeField] RectTransform overlay;
    [SerializeField] Button loginButton;
    [SerializeField] Button registerButton;
    [SerializeField] TMPro.TMP_InputField usernameField;
    [SerializeField] TMPro.TMP_InputField passwordInputField;
    [SerializeField] TMPro.TMP_InputField emailField;
    private RectTransform rectTransform;
 

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        backButton.onClick.AddListener(() => { ScreenManager.Instance.GoBack(); });
        registerButton.onClick.AddListener(() =>
        {
            PlayFabManager.Instance.Register(usernameField.text, passwordInputField.text, emailField.text);
        });
        loginButton.onClick.AddListener(() =>
        {
            PlayFabManager.Instance.Login(usernameField.text, passwordInputField.text);
        });
    }
    public override Tweener Hide()
    {
        UIAnimationController.ScaleUIAnimation(rectTransform, isIn: false).OnComplete(() =>
        {
            overlay.gameObject.SetActive(false);
            gameObject.SetActive(false);
        });
        return null;
    }

    public override Tweener Show()
    {
        overlay.gameObject.SetActive(true);
        gameObject.SetActive(true);
        UIAnimationController.ScaleUIAnimation(rectTransform, isIn: true);
        return null;
    }
}
