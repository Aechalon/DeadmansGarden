using UnityEngine.AI;
using System.Collections;
using UnityEngine;
using Photon.Pun;
[PunRPC]
public class AIScript : MonoBehaviourPunCallbacks
{
    [Header("AI Variables")]
    [SerializeField] private Animator anim;
    [SerializeField] private Transform[] destinationPoints;
    [SerializeField] private Transform target;
    [SerializeField] private PlayerControllerScript player;
    [SerializeField] private NavMeshAgent navMesh;
    private bool playerDetected;
    [SerializeField] private string destinationString;
    [SerializeField] private bool findingPath;
    [SerializeField] private bool isAlive;

    [PunRPC]
    private void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
      
          

            GameObject[] destinations = GameObject.FindGameObjectsWithTag(destinationString);

            for (int i = 0; i < destinations.Length; i++)
            {


                destinationPoints[i] = destinations[i].transform;

            }
            Debug.Log("this should work");
            CallNumerator();


    }
    [PunRPC]
    private void Update()
    {
        if (photonView.IsMine)
        {

            if (isAlive)
            {

                if (playerDetected && target != null)
                {
                    Vector3 currentPath = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    Vector3 _target = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z); // Declares a target

                    float distance = navMesh.remainingDistance;      //Translates it into float
                    photonView.RPC("EnableAnimLayer", RpcTarget.All, 1);

                    if (player != null)
                    {
                        if (!player.GetCrouchState())      // If Player is not Crouched
                        {

                            if (navMesh.remainingDistance < 1.8)          // If the remaining distance is less than 1.5f
                            {
                                photonView.RPC("EnableAnimLayer", RpcTarget.All, 2);
                                if (navMesh.speed > 0)
                                {
                                    navMesh.speed = 0;
                                }

                            }
                            if (navMesh.remainingDistance > 2)
                            {
                                photonView.RPC("DisableAnimLayer", RpcTarget.All, 2); ;
                                navMesh.speed = 4;
                            }
                        }

                        if (player.GetCrouchState() && distance != Mathf.Infinity && navMesh.remainingDistance > 3)
                        {

                            SetDestination(currentPath);

                            return;
                        }
                        SetDestination(_target);
                    }

                }
                else
                {

                    if (findingPath)
                    {
                        EnableAnimLayer(1);

                    }
                    else
                    {
                        DisableAnimLayer(1);
                    }


                }
            }
            else
            {
                EnableAnimLayer(3);
                navMesh.speed = 0;

            }
        }
        
    }
    [PunRPC]
    void SetDestination(Vector3 _destination)
    {
        
            navMesh.SetDestination(_destination);        // Sets a destination to a Target
        
    }

        
    [PunRPC]
    public void CallNumerator()
    {
      
            if (isAlive)
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
                if (!playerDetected)
                {
                    findingPath = true;

                    Vector3 destination = new Vector3(direction[i].transform.position.x, direction[i].transform.position.y, direction[i].transform.position.z);

                    SetDestination(destination);

                    yield return new WaitForSeconds(.5f); findingPath = false;
                    if (navMesh.pathStatus == NavMeshPathStatus.PathComplete && navMesh.remainingDistance < 1)
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
    private int RandomGen(int max)
    {
        int i = 0;
        i = Random.Range(0, max);
        return i;
    }
    [PunRPC]
    private bool GetPlayerStatus()
    {
        bool player = true;
        return player;
    }
    [PunRPC]
    private void DisableAnimLayer( int layer)
    {
        
            if (anim.GetLayerWeight(layer) > 0)
            {
                anim.SetLayerWeight(layer, Mathf.Lerp(anim.GetLayerWeight(layer), 0, Time.deltaTime * 10f));
            }
        

    }
    [PunRPC]
    private void EnableAnimLayer( int layer)
    {
       
            if (anim.GetLayerWeight(layer) < 1)
            {
                anim.SetLayerWeight(layer, Mathf.Lerp(anim.GetLayerWeight(layer), 1, Time.deltaTime * 10f));
            }

        
    }
    [PunRPC]

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
     
            
        }
    }
    [PunRPC]
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (target == null)
            {
                playerDetected = true;
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
            playerDetected = false;
            player = null;
            target = null;
            CallNumerator();
        }
    }

    [PunRPC]
    public void KillAI(bool newState)
    {
        isAlive = newState;
    }
    [PunRPC]
    public bool GetPlayerDetection()
    {
        return playerDetected;
    }
}
    
