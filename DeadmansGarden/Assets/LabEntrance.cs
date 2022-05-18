using Photon.Pun;
using UnityEngine;

public class LabEntrance : MonoBehaviourPunCallbacks
{
    [SerializeField] PlayerControllerScript player;
    [SerializeField] private Animation anim;
    [SerializeField] private string[] animationString;
    [SerializeField] private bool inBound;
    [SerializeField] private bool isPlayOnce;
    [SerializeField] private bool activate;
    [SerializeField]  private bool onPlay;
    private int i;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerControllerScript>();

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            activate = false;
   
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (activate && isPlayOnce)
            {

                if (!inBound )
                {
                    anim.Play();
                    inBound = true;
                    this.enabled = false;
                    this.gameObject.tag = "Obstacle";
                }
            }
            if (activate && !isPlayOnce)
            {
                if (!inBound)
                {
                    if (!onPlay && !anim.IsPlaying(animationString[1]))
                    {
                        anim.Play(animationString[0]);
                        onPlay = true;
               

                    }
                    if (onPlay && !anim.IsPlaying(animationString[0]))
                    {
                        anim.Play(animationString[1]);
                        onPlay = false;
                   
                    }
                }
                Invoke("ElevatorCall", 3.32f);
            }
          
            

        }
    }

    public void SetActivate(bool state)
    {
        activate = state;
    }
 
    public void ElevatorCall()
    {
        activate = false;
    }
    
}   
 