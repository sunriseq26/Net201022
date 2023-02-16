using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    private TMP_Text _tmpText;
    
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _tmpText = GetComponentInChildren<TMP_Text>();
        _tmpText.color = Color.black;
    }
    
    private void Start()
    {
        Connect();
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = Application.version;
        }
    }

    public void DisconnectPhotonServer()
    {
        if(PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }
    
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Disconnect();
        Debug.Log("Disconnect: On left room");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.CreateRoom("NewRoom");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("OnDisconnected");
            _tmpText.color = Color.red;
        }
    }
}