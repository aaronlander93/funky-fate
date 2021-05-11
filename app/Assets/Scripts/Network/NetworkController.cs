using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkController : MonoBehaviourPunCallbacks
{
    public Button createButton;
    public Button joinButton;
    public Button backButton;

    // Start is called before the first frame update
    void Start()
    {
        createButton.interactable = false;
        joinButton.interactable = false;

        backButton.onClick.AddListener(PhotonDisconnect);

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        createButton.interactable = true;
        joinButton.interactable = true;

        Debug.Log("Were are now connected to the " + PhotonNetwork.CloudRegion + " server.");
    }

    void PhotonDisconnect()
    {
        PhotonNetwork.Disconnect();
    }
}
