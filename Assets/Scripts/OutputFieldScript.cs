using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OutputFieldScript : MonoBehaviour
{
    public InputField inputField;
    public static string inputText = "en";

    public void GetTextFromInputField()
    {
        if(inputField.text != ""){
            inputText = inputField.text;
        }
        Debug.Log("Text from input field: " + inputText);
    }
}
