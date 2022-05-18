using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
public class Raster : MonoBehaviourPunCallbacks
{
    [SerializeField] private NavMeshAgent raster;
    [SerializeField] private Animation anim;
    [SerializeField] private PlayerControllerScript player;
    [SerializeField] private bool commandStay;
    [SerializeField] private bool isAlive;
    [SerializeField] private string[] animation;
    
    private Transform target;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            raster = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animation>();
        }
    }
    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            if (photonView.IsMine)
            {
                if (player != null)
                {
                    if (isAlive)
                    {

                        if (!player.GetHaradin())
                        {
                            target = player.gameObject.transform;
                        }
                        else
                        {
                            player = null;
                        }

                        if (!commandStay)
                        {


                            Vector3 _target = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z); // Declares a target

                            raster.SetDestination(_target);

                            if (raster.pathStatus == NavMeshPathStatus.PathComplete && raster.remainingDistance < 2)
                            {
                                anim.CrossFade(animation[2]);
                                if (raster.speed > 0)
                                {
                                    raster.speed -= 1;
                                }
                            }
                            if (raster.remainingDistance > 2)
                            {
                                anim.CrossFade(animation[3]);
                                raster.speed = 3;
                            }
                        }
                        if (commandStay)
                        {

                            anim.CrossFade(animation[2]);


                        }
                    }
                    else
                    {
                        anim.CrossFade(animation[0]);

                    }
                }
            }
        }
    }
 

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (photonView.IsMine)
            {
                if (player == null)
                {
                    player = other.GetComponent<PlayerControllerScript>();
                }
            }
        }
    }
    [PunRPC]
    public void SetCommand(bool newState)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            if (photonView.IsMine)
            {
                commandStay = newState;
            }
        }
    }
    [PunRPC]
    public void SetAlive()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            if (photonView.IsMine)
            {
                isAlive = true;
            }
        }

    }
    [PunRPC]
    public void RasterQuest()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("SetAlive", RpcTarget.All);
        }
    }
}
