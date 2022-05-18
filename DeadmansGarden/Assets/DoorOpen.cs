                
using UnityEngine;
using Photon.Pun;
public class DoorOpen : MonoBehaviourPunCallbacks
{

    [Header("My Animations")]
    [SerializeField] public Animator anim;
 

    private void Awake()
    {
   
            anim = GetComponent<Animator>();
     
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            anim.SetBool("isOpen", true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("isOpen", false);
        }
    }


}
