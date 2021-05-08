using Photon.Pun;
using Photon.Realtime;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameSetupController : MonoBehaviourPunCallbacks
{
    private List<Rigidbody2D> players;
    private List<Rigidbody2D> enemies;
    private Rigidbody2D boss;

    public GameObject playerPrefab;
    public GameObject hecklerPrefab;
    public GameObject bossPrefab;
    public GameObject keyPrefab;

    public PhotonView pv;

    //prevents enemies from moving in sync
    private System.Random rand;

    private string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        players = new List<Rigidbody2D>();
        enemies = new List<Rigidbody2D>();

        sceneName = SceneManager.GetActiveScene().name;

        CreatePlayer();
        CreateEnemies();
    
        if(sceneName == "LV00-Backstage")
        {
            CreateKeys();
        }
        else if(sceneName == "LVL01-Boss")
        {
            CreateBoss();
        }
    }

    void CreatePlayer()
    {
        if (!GameConfig.Multiplayer)
        {
            var player = Instantiate(playerPrefab, GetPlayerSpawn(), Quaternion.identity);

            // Game is not in multiplayer, disable multiplayer components.
            player.GetComponentInChildren<PhotonView>().enabled = false;
            player.GetComponentInChildren<PhotonAnimatorView>().enabled = false;
            player.GetComponentInChildren<PhotonTransformViewClassic>().enabled = false;
            player.GetComponentInChildren<MultiplayerSync>().enabled = false;
            player.GetComponentInChildren<AudioNetwork>().enabled = false;
            player.GetComponentInChildren<ChatManager>().enabled = false;

            Destroy(GameObject.Find("Chatbox"));

            players.Add(player.GetComponentInChildren<Rigidbody2D>());
        }
        else
        {
            // Game is in multiplayer
            var player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), GetPlayerSpawn(), Quaternion.identity);

            PhotonView photonView = player.GetComponentInChildren<PhotonView>();

            // Set player nickname
            photonView.Owner.NickName = GameConfig.Nickname;

            // Save photon view if it's mine
            if (photonView.IsMine)
                pv = photonView;

            // Set player material and sync it with other players
            player.GetComponentInChildren<MultiplayerSync>().SetMaterialMessage(PhotonNetwork.PlayerList.Length - 1);

            // Alert other clients that player has been added
            player.GetComponentInChildren<MultiplayerSync>().PlayerCreatedMessage();

            // Alert chat that player has joined
            player.GetComponentInChildren<ChatManager>().SendMessage(GameConfig.Nickname + " has entered the room.");

            // Add player's rigidbody to the list
            players.Add(player.GetComponentInChildren<Rigidbody2D>());
        } 
    }

    void CreateEnemies()
    {
        if (!GameConfig.Multiplayer)
        {
            NonMultiplayerEnemy(17f, 2f);
            NonMultiplayerEnemy(0f, 2f);
            NonMultiplayerEnemy(25f, 2f);
            NonMultiplayerEnemy(-26f, -2f);
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            MultiplayerEnemy(17f, 2f);
            MultiplayerEnemy(0f, 2f);
            MultiplayerEnemy(25f, 2f);
            MultiplayerEnemy(-26f, -2f);
        }
    }

    void CreateBoss()
    {
        if (!GameConfig.Multiplayer)
        {
            var boss1 = Instantiate(bossPrefab, new Vector2(68f, 3f), Quaternion.identity);
        }
    }

    void CreateKeys()
    {
        var key = Instantiate(keyPrefab, new Vector2(-28.297f, -4.439f), Quaternion.identity);
        key.GetComponentInChildren<ItemPickup>().KeyID = 0;

        key = Instantiate(keyPrefab, new Vector2(19.8f, -22.83f), Quaternion.identity);
        key.GetComponentInChildren<ItemPickup>().KeyID = 1;
    }

    Vector2 GetPlayerSpawn()
    {
        Vector2 spawnLoc = default;
        switch (sceneName)
        {
            case "LV00-Backstage":
                spawnLoc = new Vector2(5f, .6f);
                break;
            case "LVL01-Boss":
                spawnLoc = new Vector2(10f, 2f);
                break;
        }

        return spawnLoc;
    }

    void NonMultiplayerEnemy(float x, float y)
    {
        // Hard-coding this for now
        GameObject enemy = Instantiate(hecklerPrefab, new Vector2(x, y), Quaternion.identity);

        enemy.GetComponentInChildren<PhotonView>().enabled = false;
        enemy.GetComponentInChildren<PhotonAnimatorView>().enabled = false;
        enemy.GetComponentInChildren<PhotonTransformViewClassic>().enabled = false;

        // enemy.GetComponent<EnemyAI>().randHandler = (float)rand.Next(0, 50);

        enemies.Add(enemy.GetComponentInChildren<Rigidbody2D>());
    }

    void MultiplayerEnemy(float x, float y)
    {
        GameObject enemy = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Heckler"), new Vector2(x, y), Quaternion.identity);

        enemies.Add(enemy.GetComponentInChildren<Rigidbody2D>());
    }

    public void FindEnemies()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(var enemy in allEnemies)
        {
            enemies.Add(enemy.GetComponent<Rigidbody2D>());
        }
    }

    public void RemoveEnemy(Rigidbody2D enemyDefeated)
    {
        enemies.Remove(enemyDefeated);
    }

    public void RespawnPlayer()
    {
        GameObject myPlayer = default;

        if (GameConfig.Multiplayer)
        {
            // Find my player
            var allPlayers = GameObject.FindGameObjectsWithTag("Player");

            foreach(var player in allPlayers)
            {
                if(player.GetComponentInChildren<PhotonView>().IsMine)
                {
                    myPlayer = player;
                }
            }    
        }
        else
        {
            myPlayer = GameObject.FindGameObjectWithTag("Player");
        }

        myPlayer.GetComponentInChildren<Rigidbody2D>().position = GetPlayerSpawn();
        myPlayer.GetComponentInChildren<CharacterHealth>().FullHealth();
    }

    public void UpdatePlayerList()
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        players.Clear();
        
        foreach(var player in allPlayers)
        {
            players.Add(player.GetComponentInChildren<Rigidbody2D>());
        }
    }

    public List<Rigidbody2D> GetEnemies() { return enemies; }

    public List<Rigidbody2D> GetPlayers() { return players; }
}
