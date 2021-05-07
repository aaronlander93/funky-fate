using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guitar : MonoBehaviour
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
            // Destroy(gameObject.GetComponent<PhotonView>());
            // Destroy(gameObject.GetComponent<PhotonTransformViewClassic>());
            // Destroy(gameObject.GetComponent<MultiplayerSync>());
        }
        
        // direction spawning Guitar in same spot in singleplayer
        // _rb.velocity = new Vector2(direction, direction.y + yOffset);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag =="Player")
        {
            // Debug.Log("Hit");
            //damage player
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(1);
            // Destroy(gameObject);
            DestroyProjectile();
        }
    }

    void DestroyProjectile() 
    { 
        if (!GameConfig.Multiplayer)
            Destroy(gameObject);
        // else if(gameObject.GetComponent<PhotonView>().IsMine)
        //     PhotonNetwork.Destroy(gameObject);
    }
}
