
using UnityEngine;
using Photon.Pun;
public class MinimapCamera : MonoBehaviourPunCallbacks
{
    [SerializeField] private MinimapCamera minimapCam;

    private void Update()
    {
        if (minimapCam == null)
        {
            if (photonView.IsMine)
            {
                minimapCam = FindObjectOfType<MinimapCamera>();
            }
        }
        if (!photonView.IsMine)
        {
            minimapCam = FindObjectOfType<MinimapCamera>();
            if (minimapCam != null)
                {
                    Destroy(minimapCam.gameObject);
                }
            }
        
    }
}
