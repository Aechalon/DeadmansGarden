using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;
    public GameObject startButton;
    public GameObject cancelButton;
    public GameObject offlineButton;
    public bool isBtnCreate = false;

    //for navigation
    public GameObject gameManager;
    public GameObject startPanel;
    public InputField inputCreate;
    public InputField inputJoin;

    public string roomToJoin;

    private void Awake()
    {
        lobby = this;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); //connect to photton server
        offlineButton.gameObject.SetActive(true);
    }

    private void Update()
    {
        //Debug.Log(inputCreate.text);
        roomToJoin = inputJoin.text;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon Master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        startButton.SetActive(true);
        offlineButton.SetActive(false);
    }

    public void OnPlayButtonClicked()
    {
        if(roomToJoin == "")
        {
            PhotonNetwork.JoinRandomRoom();
            //PhotonNetwork.JoinRoom(roomToJoin);
            Debug.Log("Play Button was clicked");
            startButton.SetActive(false);
            cancelButton.SetActive(true);
        }
        else
        {
            PhotonNetwork.JoinRoom(roomToJoin);
            Debug.Log("Play Button was clicked");
            startButton.SetActive(false);
            cancelButton.SetActive(true);
        }
        


    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random game but failed. There must be no open room");
        CreateRoom();
    }

    void CreateRoom()
    {
        if (isBtnCreate)
        {
            Debug.Log("Trying to create a new room");
            //int randomRoomName = Random.Range(0, 10000);
            RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 10 };
            PhotonNetwork.CreateRoom(inputCreate.text, roomOps);
            Debug.Log(inputCreate.text + " Created");

        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("We are now in a room");
        startPanel.gameObject.SetActive(false);
        PhotonNetwork.LoadLevel("Test");
        //gameManager.gameObject.SetActive(true);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must be a room with the same name");
        CreateRoom();
    }

    public void OnCancelButtonClicked()
    {
        Debug.Log("Cancel Button Clicked");
        cancelButton.SetActive(false);
        startButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
