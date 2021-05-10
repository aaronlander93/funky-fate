using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Guitar : MonoBehaviour
{
    Rigidbody2D _rb;

    public int direction;
    public float moveSpeed;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!GameConfig.Multiplayer)
        {
            Destroy(gameObject.GetComponent<PhotonView>());
            Destroy(gameObject.GetComponent<PhotonTransformViewClassic>());
        }
        
        _rb.velocity = new Vector2(direction * moveSpeed, 0f);
        _rb.AddForce(new Vector2((-direction * 1), 0f), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag =="Player")
        {
            //damage player
            if(GameConfig.Multiplayer)
            {
                collision.gameObject.GetComponent<MultiplayerSync>().PlayerDamageMessage(1);
            }
            else
            {
                collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(1);
            }
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            DestroyProjectile();
        }
    }

    void DestroyProjectile() 
    { 
        if (!GameConfig.Multiplayer)
            Destroy(gameObject);
        else if(gameObject.GetComponent<PhotonView>().IsMine)
            PhotonNetwork.Destroy(gameObject);
    }
}
