using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Photon.Pun;
public class NavMeshController : MonoBehaviourPunCallbacks
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform target;
    [SerializeField] private Transform[] patrolArea;
    [SerializeField] private PlayerControllerScript player;
    private bool playerBound;
    [SerializeField] private int i;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        if (player != null)
        {

            if (player.GetCrouchState())
            {
                return;
            }
            else
            {
                //Idle

                if (playerBound)
                {

                    //Chase
                    anim.SetLayerWeight(1, Mathf.Lerp(anim.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
                    if (target != null)
                    {
                        Vector3 _target = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
                        if (agent.remainingDistance > 1)
                        {
                            anim.SetLayerWeight(2, Mathf.Lerp(anim.GetLayerWeight(2), 1f, Time.deltaTime * 10f));
                        }
                        else
                        {
                            anim.SetLayerWeight(2, Mathf.Lerp(anim.GetLayerWeight(2), 0, Time.deltaTime * 10f));

                            agent.SetDestination(_target);

                        }

                    }


                }

            }
        }
    }
  

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerBound = true;
            target = other.gameObject.transform;
            player = other.GetComponent<PlayerControllerScript>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerBound = false;
            anim.SetLayerWeight(2, 0);
            target = null;
            player = null;
        }
    }

}
