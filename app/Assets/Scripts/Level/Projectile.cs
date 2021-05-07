using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Projectile : MonoBehaviour
{
    public GameSetupController gsc;
    Rigidbody2D _rb;
    private Rigidbody2D closestPlayer;

    private Vector2 direction;
    public float moveSpeed;
    public float yOffset;
    public float lifeTime;
    public float distance;
    public LayerMask collidables;

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
            Destroy(gameObject.GetComponent<MultiplayerSync>());
        }

        FindNearestPlayer();
        
        Invoke("DestroyProjectile", lifeTime);
        //error with direction spawning tomato in same spot in singleplayer
        direction = (closestPlayer.position - _rb.position).normalized * moveSpeed;
        _rb.velocity = new Vector2(direction.x, direction.y + yOffset);
    }

    // Update is called once per frame
    void Update()
    {
;
    }

    private void FindNearestPlayer()
    {
        List<Rigidbody2D> players = gsc.GetPlayers();

        float closestDist = Mathf.Infinity;

        int index = 0;  //for debugging purposes
        foreach (Rigidbody2D player in players)
        {
            // dist = rb.position.x - player.transform.position.x;
            float dist = Vector2.Distance(transform.position, player.position);

            if (dist < closestDist)
            {
                closestDist = dist;
                closestPlayer = player;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        if(collision.gameObject.tag =="Player")
        {
            // Debug.Log("Hit");
            //damage player
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(1);
            // Destroy(gameObject);
            DestroyProjectile();
        }
        else if (collision.transform.parent != null && collision.transform.parent.tag == "ground")
        {
            // Debug.Log("Hit the ground");
            // Destroy(gameObject);
            DestroyProjectile();
        }
        else if (screenPos.y > Screen.height || screenPos.y < 0)
        {
            // Debug.Log("Left cam");
            // Destroy(gameObject);
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
