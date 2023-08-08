using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputFieldScript : MonoBehaviour
{
    public InputField inputField;
    public static string inputText = "zh-CN";

    public void GetTextFromInputField()
    {
        if(inputField.text != ""){
            inputText = inputField.text;
        }
        Debug.Log("Text from input field: " + inputText);
    }
}
