/*
Code By: Andrew Sha
Code for Game Manager

This code can respawn the player
and control some aspects of the game, like loading a scene
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject currentCheckpoint;
    private Movement2D player;
    public CharacterHealth playerHp;
    public GameObject deathParticle;
    public GameObject respawnParticle;
    public float respawnDelay;
    public static GameObject instance;
    private float gravityoStore;
    private Transform gm;
    private bool on = false;
    void Start()
    {
        player = GetComponentInChildren<Movement2D>();

        playerHp = GetComponentInChildren<CharacterHealth>();

        gm = transform.Find("_GameManager");

        if (gm) Debug.Log("Got GM " + Time.time);

    }
    void Awake()
    {

        if (!instance)
            instance = this.gameObject;
        else if (instance != this.gameObject)
        {
            Destroy(this.gameObject);
        }

        // DontDestroyOnLoad(this.gameObject);

        if (PlayerPrefs.GetString("FunkyFate-Menu") != "FunkyFate")
        {
            PlayerPrefs.SetString("FunkyFate-Menu", "FunkyFate");
        }
        Cursor.visible = true;
        Time.timeScale = 1.0f;
        // Screen.lockCursor = false;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("GameManager Awake");
    }
    void OnEnable()
    {
        Debug.Log("GameManager OnEnable");
    }
    public void LoadScene(string scene = "")
    {
        if (scene != "")
        {
            if (Application.CanStreamedLevelBeLoaded(scene))
            {
                SceneManager.LoadScene(scene);
            }
            else
            {
                Debug.Log("Scene:" + scene + "-Not Found");
            }
        }
        else
        {
            Debug.Log("Menu scene name required");
        }
    }
    void OnDestroy()
    {
        Time.timeScale = 1.0f; // reset
        Debug.Log("GameManager::OnDestroy");
    }
    public void RespawnPlayer()
    {
        
        StartCoroutine("RespawnPlayerCo");
    }
    public IEnumerator RespawnPlayerCo()
    {
        Instantiate(deathParticle, player.transform.position, player.transform.rotation);
        player.enabled = false;
        player.GetComponentInChildren<Renderer>().enabled = false;
        player.GetComponentInChildren<Rigidbody2D>().velocity = Vector2.zero;
        gravityoStore = player.GetComponentInChildren<Rigidbody2D>().gravityScale;
        player.GetComponentInChildren<Rigidbody2D>().gravityScale = 0f;
        Debug.Log("Player respawned");
        yield return new WaitForSeconds(respawnDelay);
        player.GetComponentInChildren<Rigidbody2D>().gravityScale = gravityoStore;
        player.transform.position = currentCheckpoint.transform.position;
        player.enabled = true;
        player.GetComponentInChildren<Renderer>().enabled = true;
        playerHp.FullHealth();
        playerHp.dead = false;
        Instantiate(respawnParticle, currentCheckpoint.transform.position, currentCheckpoint.transform.rotation);
    }
}

