using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AccountScreenController : UIScreen
{
    [SerializeField] RectTransform overlay;
    [SerializeField] Button loginButton;
    [SerializeField] Button registerButton;
    [SerializeField] TMPro.TMP_InputField usernameField;
    [SerializeField] TMPro.TMP_InputField passwordInputField;
    private RectTransform rectTransform;
 

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        loginButton.onClick.AddListener(() =>
        {
            Observer.Notify(GameEvent.OnPlayerLogin, usernameField.text, passwordInputField.text);
        });
        registerButton.onClick.AddListener(() =>
        {

        });
        Observer.AddObserver(GameEvent.OnLoginSuccess, OnLoginSuccess);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnLoginSuccess, OnLoginSuccess);
    }

    void OnLoginSuccess(object[] datas)
    {
        StartCoroutine(UITransitionController.SlideTransition(Callback()));
    }
    IEnumerator Callback()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Menu Scene");
        while(!asyncLoad.isDone)
        {
            yield return null;
        }
        ScreenManager.Instance.TransitionTo(ScreenID.HomeScreen);
    }

    public override IEnumerator Hide()
    {
        overlay.gameObject.SetActive(false);
        gameObject.SetActive(false);
        yield return null;
    }

    public override IEnumerator Show()
    {
        overlay.gameObject.SetActive(true);
        gameObject.SetActive(true);
        yield return null;
    }

}
