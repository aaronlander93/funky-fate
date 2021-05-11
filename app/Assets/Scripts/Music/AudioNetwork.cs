using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AudioNetwork : MonoBehaviour
{
    public TriggerAudio trigger;

    void Start()
    {
        PhotonView photonView = PhotonView.Get(this);

        if(photonView.IsMine)
        {
            Movement2D.JumpEvent += PlaySoundMessage;
        }
    }

    public void PlaySoundMessage(string sound)
    {
        PhotonView photonView = PhotonView.Get(this);

        if (photonView.IsMine)
        {
            photonView.RPC("PlaySound", RpcTarget.All, sound);
        }
    }

    [PunRPC]
    void PlaySound(string sound)
    {
        PhotonView photonView = PhotonView.Get(this);
        
        if (!photonView.IsMine)
        {
            // Find my photon view
            var photonViews = GameObject.FindObjectsOfType<PhotonView>();

            PhotonView myPv = null;
            foreach(var pv in photonViews)
            {
                if (pv.IsMine)
                {
                    myPv = pv;
                }
            }

            float volume = CalculateVolume(myPv.GetComponent<Rigidbody2D>().position);
   
            myPv.gameObject.GetComponentInChildren<TriggerAudio>().PlayOneShot(sound, volume);
        }
        else
        {
            trigger.PlayOneShot(sound, 1f);
        }
    }

    private float CalculateVolume(Vector2 myPos)
    {
        Vector2 theirPos = GetComponent<Rigidbody2D>().position;

        float dist = Mathf.Sqrt(Mathf.Pow(Math.Abs(myPos.x - theirPos.x), 2) + Mathf.Pow(Math.Abs(myPos.y - theirPos.y), 2));

        float volume = 1 - (dist / 40);

        if(volume < 0)
        {
            volume = 0;
        }

        return volume;
    }

    void OnDestroy()
    {
        PhotonView photonView = PhotonView.Get(this);

        if (photonView.IsMine)
        {
            Movement2D.JumpEvent -= PlaySoundMessage;
        }
    }
}
