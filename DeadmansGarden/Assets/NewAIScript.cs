using UnityEngine.AI;
using System.Collections;
using UnityEngine;
using Photon.Pun;
[PunRPC]
public class NewAIScript : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform destination;
    [SerializeField] private Transform target;
    [SerializeField] private Animation anim;
    [SerializeField] private string[] animation;
    [SerializeField] private PlayerControllerScript player;
    [SerializeField] private Transform[] destinationPoints;
    [SerializeField] private string destinationString;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private bool pathFind;

    [PunRPC]
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animation>();
        GameObject[] destinations = GameObject.FindGameObjectsWithTag(destinationString);

        for (int i = 0; i < destinations.Length; i++)
        {


            destinationPoints[i] = destinations[i].transform;

        }
        Debug.Log("this should work");
        SetFindPath();
    }
    [PunRPC]
    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            if (target != null)
            {
                Vector3 _target = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z); // Declares a target
                Vector3 _currentPath = new Vector3(transform.position.x, transform.position.y, transform.position.z); // Declares a lastPath
                anim.CrossFade(animation[1]);
          
                if (player != null)      // If Player is not Crouched
                {
                    if (!player.GetCrouchState())
                    {
                        if (_agent.remainingDistance < 2)          // If the remaining distance is less than 1.5f
                        {
                            anim.CrossFade(animation[2]);
                            //      photonView.RPC("EnableAnimLayer", RpcTarget.All, 2); //move
                            if (_agent.speed > 0)
                            {

                                _agent.speed = 0;
                            }
                            

                        }
                        if (_agent.remainingDistance > 2)
                        {
                            anim.CrossFade(animation[1]);
                            //     photonView.RPC("DisableAnimLayer", RpcTarget.All, 2); ; //idle
                            _agent.speed = 4;
                        }
                    }

                    if (player.GetCrouchState() && _agent.remainingDistance != Mathf.Infinity && _agent.remainingDistance > 3)
                    {
                        _agent.SetDestination(_currentPath);

                        return;
                    }

                    _agent.SetDestination(_target);

                }
               
            }
            if (pathFind)
            {
                anim.CrossFade(animation[1]);
            }
            else
            {
                anim.CrossFade(animation[0]);
            }
        
           
        }

    }
    [PunRPC]
    void SetFindPath()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            if (player == null)
            {
                StartCoroutine(FindPath(destinationPoints, destinationPoints.Length));
            }
        }
    }

    [PunRPC]
    IEnumerator FindPath(Transform[] direction, int max)
    {
        Debug.Log("this should work");
        int n = 1;
        int i = 0;

        while (i < n)
        {
            while (n < max + 1)
            {
                if (player == null)
                {
                    pathFind = true;

                    Vector3 destination = new Vector3(direction[i].transform.position.x, direction[i].transform.position.y, direction[i].transform.position.z);

                    _agent.SetDestination(destination);

                    yield return new WaitForSeconds(.5f); pathFind = false;
                    if (_agent.pathStatus == NavMeshPathStatus.PathComplete && _agent.remainingDistance < 1)
                    {
                        Debug.Log("this should work");
                        yield return new WaitForSeconds(2);
                        i++;
                        n++;

                    }
                }
                else
                {
                    yield return null;
                }

            }

            yield return null;

        }


    }
    [PunRPC]
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            target = other.gameObject.transform;
            player = other.GetComponent<PlayerControllerScript>();
        }
    }
    [PunRPC]
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (target == null)
            {
       
                target = other.gameObject.transform;
            }
            if (player == null)
            {
                player = other.GetComponent<PlayerControllerScript>();
            }

        }
    }
    [PunRPC]
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
            player = null;
            SetFindPath();
        }
    }

}
