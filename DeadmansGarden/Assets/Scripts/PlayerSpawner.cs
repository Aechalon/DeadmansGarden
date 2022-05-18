using UnityEngine;
using Photon.Pun;
public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform[] checkPoint;


    [PunRPC]
        void Awake()
        {
    

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate("Haradin", checkPoint[1].transform.position, Quaternion.identity);
                PhotonNetwork.Instantiate("CameraSystemHaradin", checkPoint[1].transform.position, Quaternion.identity);

            }
            else
            {

                PhotonNetwork.Instantiate("Ellia", checkPoint[0].transform.position, Quaternion.identity);
                PhotonNetwork.Instantiate("CameraSystemEllia", checkPoint[0].transform.position, Quaternion.identity);

            }


        
    }

    

}
