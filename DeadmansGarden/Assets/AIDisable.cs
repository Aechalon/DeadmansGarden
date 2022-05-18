using UnityEngine;
using Photon.Pun;   
public class AIDisable : MonoBehaviourPunCallbacks
{
    [SerializeField] private AIScript aI;
    [SerializeField] private GameObject loot;
    [PunRPC]
    private void Awake()
    {
        aI = GetComponentInParent<AIScript>();
     
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Collectibles"))
        {
            
            aI.KillAI(false);
            photonView.RPC("LootDrop", RpcTarget.All);
        }
    }
    [PunRPC]
    public void LootDrop()
    {
        loot.SetActive(true);
    }

}
