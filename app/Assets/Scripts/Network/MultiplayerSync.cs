using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Photon.Pun;

// Author:      Aaron
// Purpose:     Fix movement lag between players in multiplayer mode.
// Stolen from: https://doc.photonengine.com/en-us/pun/v1/demos-and-tutorials/package-demos/rpg-movement#extrapolate_options
// Changelog:   4/7/2021 - created

public class MultiplayerSync : MonoBehaviourPun, IPunObservable
{
    public PhotonView pv;

    public GameSetupController gsc;

    public Material[] materials = new Material[4];
    public Color[] colors = new Color[4];

    //Values that will be synced over network
    Vector3 latestPos;
    Quaternion latestRot;
    //Lag compensation
    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;
    Vector3 positionAtLastPacket = Vector3.zero;
    Quaternion rotationAtLastPacket = Quaternion.identity;

    void Awake()
    {
        gsc = GameObject.Find("GameSetupController").GetComponent<GameSetupController>();
    }

    void Start()
    {
        if(!GameConfig.Multiplayer)
        {
            Destroy(this);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            //Network player, receive data
            latestPos = (Vector3)stream.ReceiveNext();
            latestRot = (Quaternion)stream.ReceiveNext();

            //Lag compensation
            currentTime = 0.0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;
            positionAtLastPacket = transform.position;
            rotationAtLastPacket = transform.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            //Lag compensation
            double timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;

            //Update remote player
            transform.position = Vector3.Lerp(positionAtLastPacket, latestPos, (float)(currentTime / timeToReachGoal));
            transform.rotation = latestRot;
        }
    }

    public void PlayerCreatedMessage()
    {
        pv.RPC("PlayerCreated", RpcTarget.All);
    }

    [PunRPC]
    void PlayerCreated()
    {
        if (!pv.IsMine)
        {
            gsc.UpdatePlayerList();
        }
    }

    public void PlayerDamageMessage(int damage)
    {
        pv.RPC("PlayerDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void PlayerDamage(int damage)
    {
        if (pv.IsMine)
        {
            pv.gameObject.GetComponentInChildren<CharacterHealth>().TakeDamage(damage);
        }
    }

    public void EnemyDamageMessage(int id, int damage)
    {
        pv.RPC("EnemyDamage", RpcTarget.All, id, damage);
    }

    [PunRPC]
    void EnemyDamage(int id, int damage)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemies)
        {
            if (enemy.GetComponent<PhotonView>().ViewID == id)
            {
                var healthManager = enemy.GetComponent<Enemy>();

                if (healthManager)
                {
                    enemy.GetComponent<Enemy>().TakeDamage(damage, true);
                }
                else
                {
                    // Check if enemy is a boss
                    var bossManager = enemy.GetComponent<Boss>();
                    bossManager.TakeDamage(damage);
                }
            }
        }
    }

    public void EnemyDeadMessage(int id)
    {
        if (pv.IsMine)
        {
            pv.RPC("EnemyDead", RpcTarget.All, id);
        }
    }

    [PunRPC]
    void EnemyDead(int id)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemies)
        {
            if (enemy.GetComponent<PhotonView>().ViewID == id)
            {
                gsc.RemoveEnemy(enemy.GetComponent<Rigidbody2D>());
                break;
            }
        }
    }

    public void SetMaterialMessage(int playerNum)
    {
        if (pv.IsMine)
        {
            pv.RPC("SetMaterial", RpcTarget.AllBuffered, playerNum);
        }
    }

    [PunRPC]
    void SetMaterial(int playerNum)
    {
        Material mat = materials[playerNum];
        Color color = colors[playerNum];

        GameObject player = null;
        if (pv.IsMine)
        {
            player = gameObject;
        }
        else
        {
            var players = GameObject.FindGameObjectsWithTag("Player");

            foreach(var p in players)
            {
                if(p.GetComponentInChildren<PhotonView>().ViewID == pv.ViewID)
                {
                    player = p;
                    break;
                }
            }
        }
        
        player.GetComponent<Renderer>().material = mat;

        Light2D light2D = player.GetComponent<Light2D>();
        light2D.color = color;
        light2D.intensity = 3;
    }
}