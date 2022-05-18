using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class PhotonConnect : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject Connected;
    [SerializeField] private Text co;
    private string gameVersion = "1";
    
    private void Start()
    {
        
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        Connected.gameObject.SetActive(true);
        Debug.Log("Connecting to Photon");
     
        co.text = "Please Wait";
    }

    private void OnJoinedLobby()
    {
        Debug.Log("On Joined Lobby");
       
    }
    #region MonoBehaviorPunCallbacks Callbacks
    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("You have connected to " + PhotonNetwork.CloudRegion + " server");
            Connected.gameObject.SetActive(false);
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.JoinLobby();
        }
        else
        {
            co.text = "Could not Connect to the network";
       
            Connected.gameObject.SetActive(true);
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        Connected.gameObject.SetActive(true);
        co.text = "You have disconnected";
    }
    #endregion
}
