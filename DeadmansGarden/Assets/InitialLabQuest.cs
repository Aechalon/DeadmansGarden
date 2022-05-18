using Photon.Pun;
using UnityEngine;
[PunRPC]
public class InitialLabQuest : MonoBehaviourPunCallbacks
{
    GameManager gameManager;
    [SerializeField] private PlayerControllerScript player;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerControllerScript>();
        }
    }

    /// <summary>
    /// ine
    /// </summary>
    private void Update()
    {
        if (photonView.IsMine)
        {
            if (player != null)
            {
                if (player.GetMiniGameTask())
                {
                    gameManager.SetPower(true, 0); //Rpc
                    this.enabled = false;
                }
            }
        }
    }
   
}
