using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject[] players;
    private GameObject closestPlayer;

    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        FindNearestPlayer();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FindNearestPlayer()
    {
        float dist;
        float closestDist = float.MaxValue;

        foreach(GameObject player in players)
        {
            dist = gameObject.transform.position.x - player.transform.position.x;
            if (Math.Abs(dist) < Math.Abs(closestDist))
            {
                closestPlayer = player;
            }
        }
    }
}
