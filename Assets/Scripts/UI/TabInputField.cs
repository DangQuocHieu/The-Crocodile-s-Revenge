using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabInputField : MonoBehaviour
{
    public List<TMP_InputField> inputFields;  // Kéo thả các InputField vào danh sách này

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameObject currentObj = EventSystem.current.currentSelectedGameObject;
            if (currentObj != null)
            {
                int index = inputFields.IndexOf(currentObj.GetComponent<TMP_InputField>());
                if (index != -1)
                {
                    bool shiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                    int nextIndex = shiftPressed ? index - 1 : index + 1;

                    if (nextIndex >= 0 && nextIndex < inputFields.Count)
                    {
                        inputFields[nextIndex].Select();
                    }
                }
            }
        }
    }
}
