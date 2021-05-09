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

public class BossManager : MonoBehaviour
{
    public GameObject bossPrefab;
    public BoxCollider2D[] walls;

    // Start is called before the first frame update
    void Start()
    {
        walls = transform.Find("Bosswalls").GetComponentsInChildren<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SpawnBoss()
    {
        if (!GameConfig.Multiplayer)
        {
            var boss1 = Instantiate(bossPrefab, new Vector2(75f, 17f), Quaternion.identity);
        }
        else if(PhotonNetwork.IsMasterClient)
        {
            var boss1 = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Boss1"), new Vector2(75f, 17f), Quaternion.identity);
        }
    }

    public void SetWalls()
    {
        foreach (BoxCollider2D wall in walls)
            wall.enabled = true;
    }

    public void DisableWalls()
    {
        foreach (BoxCollider2D wall in walls)
            wall.enabled = false;
    }
}
