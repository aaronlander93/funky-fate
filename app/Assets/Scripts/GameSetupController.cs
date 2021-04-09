using Photon.Pun;
using Photon.Realtime;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetupController : MonoBehaviourPunCallbacks
{
    private List<GameObject> players;
    private List<GameObject> enemies;

    public GameObject playerPrefab;
    public GameObject hecklerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        players = new List<GameObject>();
        enemies = new List<GameObject>();

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
        }
        else
        {
            // Game is in multiplayer
            var player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), new Vector2(5f, .6f), Quaternion.identity);
        } 
    }

    void CreateEnemies()
    {

        if (!GameConfig.Multiplayer)
        {
            // Hard-coding this for now
            var enemy = Instantiate(hecklerPrefab, new Vector2(5f, .6f), Quaternion.identity);

            enemy.GetComponentInChildren<PhotonView>().enabled = false;
            enemy.GetComponentInChildren<PhotonAnimatorView>().enabled = false;
            enemy.GetComponentInChildren<PhotonTransformViewClassic>().enabled = false;

            enemies.Add(enemy);
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            var enemy = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Heckler"), new Vector2(17f, 2f), Quaternion.identity);

            enemies.Add(enemy);
        }
    }

    private void UpdatePlayerList()
    {
        var allPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach(var player in allPlayers)
        {
            if (!players.Contains(player))
            {
                players.Add(player);
            }
        }
    }

    public List<GameObject> GetEnemies() { return enemies; }

    public List<GameObject> GetPlayers() { return players; }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }
}
