using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class UpperBound : MonoBehaviourPunCallbacks
{
     public bool inBound;

    [PunRPC]
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Obstacle") )
        {
            inBound = true;
        }
    }
   
    [PunRPC]
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            inBound = false;
        }
    }
}

