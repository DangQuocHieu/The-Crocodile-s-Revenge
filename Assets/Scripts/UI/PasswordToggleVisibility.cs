using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordToggleVisibility : MonoBehaviour
{
    [SerializeField] TMP_InputField passwordField;
    [SerializeField] Button toggleButton;    

    [SerializeField] Sprite hideSprite;
    [SerializeField] Sprite showSprite;
    bool isHide = true;

    void Awake()
    {
        toggleButton.image.sprite = isHide ? showSprite : hideSprite;
        toggleButton.onClick.AddListener(Handle);
        
    }
    public void Handle()
    {
        isHide = !isHide;
        passwordField.contentType = isHide ? TMP_InputField.ContentType.Password : TMP_InputField.ContentType.Standard;
        toggleButton.image.sprite = isHide ? showSprite : hideSprite;
        passwordField.ForceLabelUpdate();
    }
}
