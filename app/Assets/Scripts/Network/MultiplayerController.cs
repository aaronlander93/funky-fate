using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MultiplayerController : MonoBehaviourPunCallbacks
{
    public GameObject createRoomButton;
    public GameObject joinRoomButton;
    public InputField roomNameInput;

    public int roomSize;

    private string roomName;

    void Start()
    {
        roomNameInput.onEndEdit.AddListener(SubmitRoom);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        joinRoomButton.SetActive(true);
    }

    public void OnCreateRoomClick()
    {
        CreateRoom();
    }

    public void OnJoinRoomClick()
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    void CreateRoom()
    {
        RoomOptions roomOps = new RoomOptions() { IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom(roomName, roomOps);
    }

    private void SubmitRoom(string room)
    {
        this.roomName = room;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room.");
    }
}
