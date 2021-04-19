using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Explosion : MonoBehaviour
{
    void end()
    {
        if (!GameConfig.Multiplayer)
            Destroy(gameObject);
        else
            PhotonNetwork.Destroy(gameObject);
    }
}
