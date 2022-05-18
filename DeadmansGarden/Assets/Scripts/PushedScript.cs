using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
[PunRPC]

public class PushedScript : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text txtPrompt;
    public void Start()
    {
        txtPrompt = FindObjectOfType<Text>();
    }
    [PunRPC]
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag ("Player"))
        {
            transform.Translate(Vector3.left * Time.deltaTime);
        }
    }
    [PunRPC]
    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            txtPrompt.text = "T to Push";
        }
    }
    [PunRPC]
    public void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            txtPrompt.text = "";
        }
    }

}
