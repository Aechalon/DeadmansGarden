using UnityEngine;
using Photon.Pun;
using Cinemachine;
using StarterAssets;
public class PlayerAimController : MonoBehaviourPunCallbacks
{
    [SerializeField] private CinemachineVirtualCamera[] camera;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private StarterAssetsInputs inputs;
    [SerializeField] private ThirdPersonController player;
    [SerializeField] private float normalSensitivity, aimSensitivity;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject debugTransform;

    [SerializeField] private LayerMask aimColliderMask = new LayerMask();

    [PunRPC]

    private void Awake()
    {
        if(photonView.IsMine)
        {
            camera = GetComponentsInChildren<CinemachineVirtualCamera>();
            camera[1].gameObject.SetActive(false);
            gameManager = FindObjectOfType<GameManager>();

            VoidGetComponents();
        }


    }

    [PunRPC]
    private void Update()
    {
      
       if (photonView.IsMine)
            {
          
                if (debugTransform != null || inputs != null || player != null)
                {

                    Vector3 mouseWorldPosition = Vector3.zero;
                    Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2);
                    Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
                    if (Physics.Raycast(ray, out RaycastHit raycashHit, 999f, aimColliderMask))
                    {
                        debugTransform.transform.position = raycashHit.point;
                        mouseWorldPosition = raycashHit.point;
                    }
                    else
                    {
                        debugTransform.transform.position = ray.GetPoint(20);
                        mouseWorldPosition = ray.GetPoint(20);
                    }

                    if (inputs.aim)
                    {
                        camera[0].gameObject.SetActive(false);
                        camera[1].gameObject.SetActive(true);
                        player.SetSensitivity(aimSensitivity);
                        crosshair.SetActive(true);
                        player.SetRotateOnMove(false);

                        SetAimAnimation(true);

                        Vector3 worldAimTarget = mouseWorldPosition;
                        worldAimTarget.y = player.gameObject.transform.position.y;
                        Vector3 aimDireciton = (worldAimTarget - player.gameObject.transform.position).normalized;
                        player.gameObject.transform.forward = Vector3.Lerp(player.gameObject.transform.forward, aimDireciton, Time.deltaTime * 20f);
                    }
                    else
                    {
                        SetAimAnimation(false);
                        camera[0].gameObject.SetActive(true);
                        camera[1].gameObject.SetActive(false);
                        player.SetSensitivity(normalSensitivity);
                        crosshair.SetActive(false);
                        player.SetRotateOnMove(true);
                    }

                }
                else
                {
                    VoidGetComponents();
                }

            }

        
        
    }

    [PunRPC]
    private void SetAimAnimation(bool newState)
    {
        player.SetAnimatorStrafe(newState);

        if (newState == true)
        {
            Vector3 direction = player.GetMovementDirection();

            player.SetAnimatorStrafeDirection(direction);

      
        }
    }
    public void SetNormalSensitivity(float newSensitivity)
    {   
        normalSensitivity = newSensitivity;
    }
    public void SetAimSensitivity(float newSensitivity)
    {
        aimSensitivity = newSensitivity;
    }

    public void VoidGetComponents()
    {
        if (debugTransform == null)
        {
            debugTransform = GameObject.FindGameObjectWithTag("DebugTransform");
        }

        if (inputs == null)
        {
            inputs = FindObjectOfType<StarterAssetsInputs>();

        }

        if (player == null)
        {
            player = FindObjectOfType<ThirdPersonController>();
        }
    }
}
