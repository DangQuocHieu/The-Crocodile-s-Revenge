using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreenController : UIScreen
{
    [SerializeField] RectTransform overlay;
    [SerializeField] Button loginButton;
    [SerializeField] Button registerButton;
    [SerializeField] Button recoverPasswordButton;
    [SerializeField] TMPro.TMP_InputField usernameField;
    [SerializeField] TMPro.TMP_InputField passwordInputField;
    [SerializeField] Button visibleButton;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        loginButton.onClick.AddListener(() =>
        {
            canvasGroup.interactable = false;
            Login();
        });
        registerButton.onClick.AddListener(() =>
        {
            ScreenManager.Instance.TransitionTo(ScreenID.RegisterScreen);
        });

        recoverPasswordButton.onClick.AddListener(() =>
        {
            ScreenManager.Instance.TransitionTo(ScreenID.RecoverPasswordScreen);
        });

    }
    public override IEnumerator Hide()
    {
        overlay.gameObject.SetActive(false);
        gameObject.SetActive(false);
        yield return null;
    }

    public override IEnumerator Show()
    {
        gameObject.SetActive(true);
        overlay.gameObject.SetActive(true);
        canvasGroup.interactable = true;
        yield return null;
    }

    public void Login()
    {
        
        if(DataValidate.IsEmpty(usernameField.text, "Username") || DataValidate.IsEmpty(passwordInputField.text, "Password"))
        {
            return;
        }
        Observer.Notify(GameEvent.OnPlayerLogin, usernameField.text, passwordInputField.text);    
    }
}
