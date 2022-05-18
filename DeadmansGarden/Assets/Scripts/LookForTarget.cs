
using UnityEngine;
using Photon.Pun;
using Cinemachine;
public class LookForTarget : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject targetGO;
    [SerializeField] private CinemachineVirtualCamera camera;
    [PunRPC]
 

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (camera == null)
            {
                camera = GetComponent<CinemachineVirtualCamera>();
               
            }
               
            if(camera.Follow == null)
            {
                camera.Follow = GameObject.FindGameObjectWithTag("CinemachineTarget").transform;
            }
        }
    }
}
