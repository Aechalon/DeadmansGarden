using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class PhotonMethods : MonoBehaviourPunCallbacks
{
    public static PhotonMethods instance;

    private Text playerName;
    [SerializeField] private InputField createRoomInput, joinRoomInput;
    [SerializeField] private string roomName;
    [SerializeField] private byte maxPlayersPerRoom = 4;
    [SerializeField] private Transform roomListContent;
    [SerializeField] private GameObject roomListItemPrefab;
    [SerializeField] private string loadScene;
    [SerializeField] private GameObject loadPanel;
        
    // Start is called before the first frame update

    private void Awake()
    {

        instance = this;
    }

    public void Update()
    {
        roomName = joinRoomInput.text;
    }
    public void onClickCreateRoom()
    {
        
        if (createRoomInput.text.Length >= 1)
        {
            Debug.Log("Trying to create a new room");
            RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = maxPlayersPerRoom };
            PhotonNetwork.CreateRoom(createRoomInput.text, roomOps);
            Debug.Log(createRoomInput.text + " Created");
          

        }
    }
    public override void OnJoinedRoom()
    {
        loadPanel.SetActive(true);
        PhotonNetwork.LoadLevel(loadScene);

        Debug.Log("We are connected to the room!");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon Master server");
        PhotonNetwork.AutomaticallySyncScene = true;
 
    }
    public void onClickJoinRoom()
    {
        if (PhotonNetwork.IsConnected)
        {

            if (roomName == "")
            {
                PhotonNetwork.JoinRandomRoom();
                Debug.Log("Random Room Joined");     
            }
            else
            {
                PhotonNetwork.JoinRoom(roomName);
      
            }
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();

        }
      
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random game but failed. There must be no open room");
        
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must be a room with the same name");
        onClickCreateRoom();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public void JoinRoom(RoomInfo info)
    {
        loadPanel.SetActive(true);
        PhotonNetwork.JoinRoom(info.Name);
         
    }
        


}
