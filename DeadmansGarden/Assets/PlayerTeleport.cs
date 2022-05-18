
using UnityEngine;
using Photon.Pun;
[PunRPC]
public class PlayerTeleport : MonoBehaviourPunCallbacks
{

    [SerializeField] private Transform  teleportTo;
    [SerializeField] private GameObject player;




    public void SetTeleport()
    {
        if (photonView.IsMine)
        {
            player.transform.position = teleportTo.transform.position;
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {

            if(other.gameObject.CompareTag("Player"))
            {
            if (photonView.IsMine)
            {
                player = other.gameObject;
            }
              
            }
        
    }
   
}
