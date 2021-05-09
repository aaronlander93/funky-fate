/*
Code By: Andrew Sha

This code damages the enemy when getting hit by this bullet
*/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;

public class Bullet: MonoBehaviour
{
    public int damage = 1;
    public float speed = 10f;
    public float lifeTime = 1f;
    public Rigidbody2D rb;
    public GameObject Effect;
    // Start is called before the first frame update
    private GameSetupController gsc;
    void Start()
    {
        if (!GameConfig.Multiplayer)
        {
            Destroy(gameObject.GetComponent<PhotonView>());
            Destroy(gameObject.GetComponent<PhotonTransformViewClassic>());
        }

        rb.velocity = transform.right * speed;
        Invoke("Explosion", lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //damage player
            if (GameConfig.Multiplayer)
            {
                collision.gameObject.GetComponent<MultiplayerSync>().EnemyDamageMessage(collision.gameObject.GetComponent<PhotonView>().ViewID, 1);
            }
            else
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(1, true);
            }
        }

        Explosion();
    }

    private void Explosion()
    {
        if (!GameConfig.Multiplayer)
        {
            Instantiate(Effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }  
        else if(gameObject.GetComponent<PhotonView>().IsMine)
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "FX", "Explosion"), transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(gameObject);
        }
            
    }
}