﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Author: Niall
This script controlld the behavior of the pause menu
*/

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            if(GameIsPaused) 
            {
                Resume();
            } else 
            {
                Pause();
            }
        }
    }

    public void Resume() 
    {
        pauseMenuUI.SetActive(false);

        if(!GameConfig.Multiplayer)
        {
            Time.timeScale = 1f;
        }
        
        GameIsPaused = false;
    }

    void Pause() 
    {
        pauseMenuUI.SetActive(true);

        if(!GameConfig.Multiplayer)
        {
            Time.timeScale = 0f;
        }

        GameIsPaused = true;
    }

    public void Options() 
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void Back() 
    {
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
		    UnityEditor.EditorApplication.isPlaying = false;
        #else
		    Application.Quit();
        #endif
    }
}
