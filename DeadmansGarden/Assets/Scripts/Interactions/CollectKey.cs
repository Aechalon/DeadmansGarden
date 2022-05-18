using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CollectKey : MonoBehaviourPunCallbacks
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
                gameManager.isKeyCollected = true;
                PhotonNetwork.Destroy(this.gameObject);
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
