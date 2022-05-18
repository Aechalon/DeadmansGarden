
using UnityEngine;
using Photon.Pun;
public class PersonalConsole : MonoBehaviourPunCallbacks
{
    private PersonalConsole perConsole;

    private void Update()
    {

    

            if (!photonView.IsMine)
            {
                perConsole = FindObjectOfType<PersonalConsole>();
                if (perConsole != null)
                {
                    Destroy(perConsole.gameObject);
                }
            }



            
       



        }
}
