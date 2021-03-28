using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	private GameObject rootMenuPanel;

    // Start is called before the first frame update
    void Start() 
    {
		rootMenuPanel = GameObject.Find("Root Menu");
		rootMenuPanel.SetActive(true);
	}

	public void OnSingleplayerClick()
	{
		SceneManager.LoadScene("TTTLocal");
	}

	public void OnMultiplayerClick()
	{
		SceneManager.LoadScene("TTTNetwork");
	}

	public void OnExitClick()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
