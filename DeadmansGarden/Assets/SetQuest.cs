
using UnityEngine;
using Photon.Pun;
public class SetQuest : MonoBehaviourPunCallbacks
{
    public int questNumber;
    PlayerControllerScript player;
 
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            if (photonView.IsMine)
            {
                player = other.GetComponent<PlayerControllerScript>();
              
                if (player != null)
                {
                    if (player.inQuest)
                    {
                        if (gameObject.tag == "Untagged")
                        {

                            player.SetQuestUpdate("", true);
                        }
                }
                }
                this.enabled = false;
             
            }
        }
    }

}
