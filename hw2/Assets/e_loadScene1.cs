using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class e_loadScene1 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("e_sceneTransition"))
        {
            SceneManager.LoadScene("Scene1");
        }
    }
}
