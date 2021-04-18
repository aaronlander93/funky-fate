using Photon.Pun;
using Photon.Realtime;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSetupController : MonoBehaviourPunCallbacks
{
    private List<Rigidbody2D> players;
    private List<Rigidbody2D> enemies;
    private Rigidbody2D boss;

    public GameObject playerPrefab;
    public GameObject hecklerPrefab;
    public GameObject bossPrefab;

    // Start is called before the first frame update
    void Start()
    {
        players = new List<Rigidbody2D>();
        enemies = new List<Rigidbody2D>();

        CreatePlayer();
        CreateEnemies();
    }

    void CreatePlayer()
    {
        if (!GameConfig.Multiplayer)
        {
            var player = Instantiate(playerPrefab, new Vector3(5f, .6f, 0f), Quaternion.identity);

            // Game is not in multiplayer, disable multiplayer components.
            player.GetComponentInChildren<PhotonView>().enabled = false;
            player.GetComponentInChildren<PhotonAnimatorView>().enabled = false;
            player.GetComponentInChildren<PhotonTransformViewClassic>().enabled = false;
            player.GetComponentInChildren<MovementLagSync>().enabled = false;
            player.GetComponentInChildren<AudioNetwork>().enabled = false;
            player.GetComponentInChildren<ChatManager>().enabled = false;

            Destroy(GameObject.Find("Chatbox"));

            players.Add(player.GetComponentInChildren<Rigidbody2D>());
        }
        else
        {
            // Game is in multiplayer
            var player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), new Vector2(5f, .6f), Quaternion.identity);

            // Set player nickname
            player.GetComponentInChildren<PhotonView>().Owner.NickName = GameConfig.Nickname;

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
            // Hard-coding this for now
            var enemy = Instantiate(hecklerPrefab, new Vector2(17f, 2f), Quaternion.identity);

            enemy.GetComponentInChildren<PhotonView>().enabled = false;
            enemy.GetComponentInChildren<PhotonAnimatorView>().enabled = false;
            enemy.GetComponentInChildren<PhotonTransformViewClassic>().enabled = false;

            enemies.Add(enemy.GetComponentInChildren<Rigidbody2D>());
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            var enemy = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Heckler"), new Vector2(17f, 2f), Quaternion.identity);

            enemies.Add(enemy.GetComponentInChildren<Rigidbody2D>());
        }
    }

    void CreateBoss()
    {
        if (!GameConfig.Multiplayer)
        {
            var boss1 = Instantiate(bossPrefab, new Vector2(5f, .6f), Quaternion.identity);

            
        }
    }

    private void UpdatePlayerList()
    {
        var allPlayers = GameObject.FindGameObjectsWithTag("Player");

        
        foreach(var player in allPlayers)
        {
            var rb = player.GetComponentInChildren<Rigidbody2D>();
            if (!players.Contains(rb))
            {
                players.Add(rb);
            }
        }
    }

    public List<Rigidbody2D> GetEnemies() { return enemies; }

    public List<Rigidbody2D> GetPlayers() { return players; }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }
}
