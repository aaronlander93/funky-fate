using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Author:      Niall
// Purpose:     Manage the functions of the main menu buttons
// Changelog:   3/29/21 - created

public class MainMenu : MonoBehaviour
{
    public void OnStartGameClick() {
        Debug.Log("Start Game clicked");
        SceneManager.LoadScene("LV00-Backstage");
    }

    public void OnOptionsClick() {

    }

    public void OnQuitGameClick() {
        #if UNITY_EDITOR
		    UnityEditor.EditorApplication.isPlaying = false;
        #else
		    Application.Quit();
        #endif
    }
}
