using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour
{
    public static string fromLanguage = "zh-CN";
    public static string toLanguage = "en";
    public void OnPlayButton(){
        SceneManager.LoadScene("GoogleVisionApiOCRDemo");
    }

    public void OnQuitButton(){
        Application.Quit();
    }

    // public string getFromLanguage(){
    //     return fromLanguage;
    // }

    // public string getToLanguage(){
    //     return this.toLanguage;
    // }

  
}
