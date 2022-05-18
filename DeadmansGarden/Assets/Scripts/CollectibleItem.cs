using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
[PunRPC]
public class CollectibleItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text txtPrompt;
    public void Start()
    {
        txtPrompt = FindObjectOfType<Text>();
    }

    [PunRPC]
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            txtPrompt.text = "Salvage items";
        }
    }
    [PunRPC]
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            txtPrompt.text = "";
        }
    }

}
