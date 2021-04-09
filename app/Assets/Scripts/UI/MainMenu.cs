using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Author:      Niall
// Purpose:     Manage the functions of the main menu buttons
// Changelog:   3/29/21 - created

public class MainMenu : MonoBehaviour
{
    public void OnStartGameClick()
    {
        Debug.Log("Start Game clicked");
        GameConfig.Multiplayer = false;
        SceneManager.LoadScene("LV00-Backstage");
    }

    public void OnMultiplayerClick()
    {
        GameConfig.Multiplayer = true;
        SceneManager.LoadScene("MultiplayerMenu");
    }

    public void OnOptionsClick()
    {

    }

    public void OnQuitGameClick()
    {
        #if UNITY_EDITOR
		    UnityEditor.EditorApplication.isPlaying = false;
        #else
		    Application.Quit();
        #endif
    }
}
