using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    public GameObject pfBullet;
    public GameObject player;

    public float spacing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Vector3 playerPos = player.transform.position;
            Vector3 playerDirection = player.transform.forward;
            Quaternion playerRotation = player.transform.rotation;

            Vector3 spawnPos = playerPos + playerDirection * spacing;

            GameObject bullet = Instantiate(pfBullet, spawnPos, playerRotation);
        }
    }
}
