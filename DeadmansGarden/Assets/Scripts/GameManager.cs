using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
[PunRPC]
public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("Hub")]
    [SerializeField] public bool haradinQuest,elliaQuest;

         
    [Header("Old Laboratory")]
    [SerializeField] private GameObject[] door;
    [SerializeField] private GameObject labDoor;
    [SerializeField] private LegacyAnimation legacyAnim;
    [SerializeField] private GameObject[] lights;
   


    [Header("GameManager")]
    [SerializeField] private string nextLevel;
    [SerializeField] private bool levelLoaded;
    public bool gameSpawn;
    public bool gameStart;
    public bool player1Spawn, player2Spawn;

    [Header("Map")]
    public bool metropolis;
    public bool lab;
    public bool hub;
    public bool hubSpawn;
    public bool rising;


    public int i;
    public bool isKeyCollected = false;

    [SerializeField] private Transform[] checkPoint;

    private void Awake()
    {
        if (!metropolis)
        {
            if (!lab)
            {
                SpawnPlayers();
            }
        }
  
    }

    private void Update()
    {

        if (haradinQuest && elliaQuest)
        {
            if (!levelLoaded)
            {

                PhotonNetwork.LoadLevel(nextLevel);
                levelLoaded = true;

            }
        }
   


    
         
       
          
        



    }
    public void SpawnPlayers()
    {
       
            if (PhotonNetwork.IsMasterClient )
            {
                PhotonNetwork.Instantiate("Haradin", checkPoint[1].transform.position, Quaternion.identity);
                PhotonNetwork.Instantiate("CameraSystemHaradin", checkPoint[1].transform.position, Quaternion.identity);
                
            }
            else 
            {
         
                PhotonNetwork.Instantiate("Ellia", checkPoint[0].transform.position, Quaternion.identity);
                PhotonNetwork.Instantiate("CameraSystemEllia", checkPoint[0].transform.position, Quaternion.identity);
            
            }
        
       
    }
  
    [PunRPC]
    void SetLabPower(bool newState)
    {
      
            for (int i = 0; i < door.Length; i++)
            {
                door[i].GetComponent<LegacyAnimation>().SetPower(true);

            }

            lights[0].SetActive(false);
            lights[1].SetActive(true);
        
    }
    [PunRPC]
    void SetDoor(bool newState)
    {
        labDoor.GetComponent<LegacyAnimation>().SetPower(true);
    }

    [PunRPC]
    public void SetPower(bool newState, int i)
    {
        if (photonView.IsMine)
        {
            switch (i)
            {
                case 0:
                    photonView.RPC("SetLabPower", RpcTarget.All, true);
                    break;
                case 1:
                    photonView.RPC("SetDoor", RpcTarget.All, true);
                    break;

            }
        }
    }
    [PunRPC]
    public void SpawnPlayer()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("Respawn", RpcTarget.All, true);
        }
    }
    [PunRPC]
    private void Respawn()
    {
        player1Spawn = false;
        player2Spawn = false;
    }
}
