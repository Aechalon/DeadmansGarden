
using UnityEngine;
using Photon.Pun;
public class EnableObject : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject questpoint;

     private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            
            questpoint.SetActive(true);
        }
    }
}
