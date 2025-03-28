using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterScreenController : UIScreen
{
    [SerializeField] Button registerButton;
    [SerializeField] TMP_InputField usernameField;
    [SerializeField] TMP_InputField passwordField;
    [SerializeField] TMP_InputField emailField;
    [SerializeField] Button backButton;

    private CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        registerButton.onClick.AddListener(() =>
        {
            canvasGroup.interactable = false;
            Register();
        });
        backButton.onClick.AddListener(() =>
        {
            ScreenManager.Instance.HidePopUp();
        });
    }    
    public override IEnumerator Hide()
    {
        gameObject.SetActive(false);
        yield return null;
    }

    public override IEnumerator Show()
    {
        gameObject.SetActive(true);
        yield return null;
    }

    public void Register()
    {
        if(DataValidate.IsEmpty(usernameField.text, "Username") || DataValidate.IsEmpty(passwordField.text, "Password")
        || !DataValidate.IsStartWithLetter(usernameField.text, "Username") || DataValidate.IsTooShort(passwordField.text, "Password")
        || DataValidate.IsTooShort(usernameField.text, "Username"))
        {
            return;
        }
        PlayFabManager.Instance.Register(usernameField.text, passwordField.text, emailField.text);
    }

}
