using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class DoorTrigger : MonoBehaviourPunCallbacks
{
    public GameManager gameManager;

    public Text txtInteractMsg;

    [PunRPC]
    private void OnTriggerEnter(Collider actor)
    {
        if (actor.CompareTag("Player"))
        {
            txtInteractMsg.text = "Press[E] to Interact";
        }
    }

    [PunRPC]
    private void OnTriggerStay(Collider actor)
    {
        if (actor.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (gameManager.isKeyCollected)
                {
                    PhotonNetwork.Destroy(this.gameObject);
                }
                else
                {
                    txtInteractMsg.text = "It is locked";
                }
                
            }
        }
    }

    [PunRPC]
    private void OnTriggerExit(Collider actor)
    {
        if (actor.CompareTag("Player"))
        {
            txtInteractMsg.text = "";
        }
    }
}