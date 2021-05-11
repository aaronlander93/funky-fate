using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BossTomatoes : MonoBehaviour
{
    public GameSetupController gsc;
    Rigidbody2D _rb;
    private Rigidbody2D closestPlayer;

    private Vector2 direction;
    public float lifeTime;

    void Awake()
    {
        gsc = GameObject.Find("GameSetupController").GetComponent<GameSetupController>();
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
        
        Invoke("DestroyProjectile", lifeTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
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
            
            DestroyProjectile();
        }
        else if (collision.transform.parent != null && collision.transform.parent.parent.tag == "ground")
        {
            // Debug.Log("Hit the ground");
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
