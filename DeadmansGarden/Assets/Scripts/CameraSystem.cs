
using UnityEngine;
using Photon.Pun;
public class CameraSystem : MonoBehaviourPunCallbacks
{
    private CameraSystem camSys;
    [PunRPC]
    private void Update()
    {
        if (camSys == null)
        {
            camSys = FindObjectOfType<CameraSystem>();
            if (!photonView.IsMine)
            {
                Destroy(camSys.gameObject);
            }
            else
            {
                camSys = this;
            }
        }
    }
}
