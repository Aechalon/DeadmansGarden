using UnityEngine;
using System.Collections;
using Photon.Pun;
[PunRPC]
public class ObjectReposition : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform reposition;
    public int collected;
    public int collectAmount;
    public Item item;
    [SerializeField] private bool onAnimate;
    [SerializeField] private bool onDemand;
    public bool boolRepos;
    private bool hasPlay;
    [SerializeField] private bool OnSpawn;
    [SerializeField] private Animation anim;
    [SerializeField] private GameObject plant;
    [SerializeField] private GameObject wall;
    [SerializeField] private Player playerData;
    public bool isRising;
    [SerializeField] private bool questDetail;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private bool isHaradin;
    [SerializeField] private bool ifDestinationPoint;
    [SerializeField] private Raster raster;
    [SerializeField] private bool isRaster;
    public bool sideQuest;
    [SerializeField] private bool isHub;
    [SerializeField] private GameObject mainQuestPoint;
    [SerializeField] private PlayerControllerScript player;
    [PunRPC]
    private void Awake()
    {

        collected = 0;
        gameManager = FindObjectOfType<GameManager>();

    }
    [PunRPC]
    private void Update()
    {
        if (photonView.IsMine)
        {
            CheckCollection();
        }


    }
    [PunRPC]
    public void CheckCollection()
    {
        if (collected > collectAmount - 1)
        {
            if (ifDestinationPoint)
            {
                photonView.RPC("SetQuest", RpcTarget.All);
                ifDestinationPoint = false;
            }

            if (isRising)
            {
                if(player != null)
                {
                    player.QuestFinish();       
                }
                photonView.RPC("SetQuest", RpcTarget.All);
                isRising = false;

            }
            if (!sideQuest)
            {
                photonView.RPC("SetQuest", RpcTarget.All);
                sideQuest = true;

            }
         

            if (onDemand)
            {
                if (boolRepos)
                {
                    transform.position = reposition.transform.position;
                    transform.rotation = reposition.transform.rotation;
                }
                if (wall != null)
                {
                    PhotonNetwork.Destroy(wall);
                }
            }
            if (onAnimate)
            {
                hasPlay = false;
                if (!hasPlay)
                {
                    photonView.RPC("PlayAnimation", RpcTarget.All);
                }
            }
            if (OnSpawn)
            {
                plant.SetActive(true);
                OnSpawn = false;
            }
            if (isRaster)
            {
                if (player != null)
                {
                    player.SetDialogue("Okay Raster we have to get outa here.");
                }
                raster.RasterQuest();

                mainQuestPoint.SetActive(true);
               
                
                isRaster = false;
            }
            questDetail = true;
        }


    }

    
    [PunRPC]
    void PlayAnimation()
    {
        anim.Play();
        hasPlay = true;
        onAnimate = false;
    }

    [PunRPC]
    void SetQuest()
    {
        if (photonView.IsMine)
        {

            if (isHaradin)
            {
                gameManager.haradinQuest = true;
                gameManager.gameStart = false;

            }
            else
            {
                gameManager.elliaQuest = true;
                gameManager.gameStart = false;


            }

        }
}
    [PunRPC]
    public bool GetQuest()
    {
        
            return questDetail;
        
    }

       
    [PunRPC]
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (photonView.IsMine)
            {
                player = other.GetComponent<PlayerControllerScript>();
                if (player != null)
                {
                    if (player.GetHaradin() && isHub)
                    {
                        photonView.RPC("SetQuest", RpcTarget.All);
                    }
                }

                if (isRising)
                {
                    if (player != null)
                    {
                        player.SetDialogue("I'm gonna have to take it down");
                    }
                }
                if (questDetail && isRising)
                {
                    if (player != null)
                    {
                        player.SetDialogue("Great now I can contact Ellia");
                    }
                }
                
            }
        }
    }
    [PunRPC]
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (photonView.IsMine)
            {
                player = null;
                playerData = null;
            }
        }
    }

   
}
