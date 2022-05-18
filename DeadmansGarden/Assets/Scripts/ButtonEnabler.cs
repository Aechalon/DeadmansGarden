using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEnabler : MonoBehaviour
{
    public Button button;
    public InputField ipField;

    // Update is called once per frame
    void Update()
    {
        if (ipField.text != "")
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}
