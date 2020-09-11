using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonNetworkConnector : MonoBehaviourPunCallbacks
{
    public static PhotonNetworkConnector connector;

    void Awake()
    {
        connector = this;
    }

    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon master server");
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        PhotonNetwork.CreateRoom("Room 1", roomOps);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("We are now in a room");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from server for reason " + cause.ToString());
    }

    // Update is called once per frame
    void Update()
    {
    }
}
