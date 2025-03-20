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

    private void Awake()
    {
        registerButton.onClick.AddListener(() =>
        {
            PlayFabManager.Instance.Register(usernameField.text, passwordField.text, emailField.text);
        });
    }
}
