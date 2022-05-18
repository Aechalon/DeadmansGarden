using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerCamera : MonoBehaviourPunCallbacks
{
    private NavMeshAgent _agent;
    [SerializeField] private PlayerController playerController;


    [SerializeField] private  Vector2 _move;
    [SerializeField] private  Vector2 _look;
    [SerializeField] private  float prevLookX;
    [SerializeField] private  float prevLookY;
    [SerializeField] private  float aimValue;
    [SerializeField] private  float fireValue;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    float rotationX = 0;

    [SerializeField] private  Vector3 nextPosition;
    [SerializeField] private  Quaternion nextRotation;

    [SerializeField] private  float rotationPower = 3f;
    [SerializeField] private  float rotationLerp = 0.5f;

    [SerializeField] private  float speed = 1f;

    [SerializeField] private  Camera camera;
    [SerializeField] private GameObject followTransform;

    [SerializeField] private bool invertLookUp;
    [SerializeField] private bool btnMLock;
    [SerializeField] private bool normalCamera;

    [PunRPC]
    private void Awake()
    {
        if (photonView.IsMine)
        {
            _agent = GetComponent<NavMeshAgent>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            playerController = GetComponent<PlayerController>();
          

        }
        if (camera != null && !photonView.IsMine)
        {
            camera = GetComponentInChildren<Camera>();
        }

    }


    [PunRPC]
    private void Update()
    {
        if (photonView.IsMine)
        {
            btnMLock = Input.GetKey(KeyCode.Tab);
            if (!btnMLock)
            {
                if (normalCamera)
                {
                    SimpleCameraController();
                }
                else
                {
                    CameraController();
                }
            }
                MouseLock();
        }
      
        
    }
    [PunRPC]
    private void CameraController()
    {
       
            #region Player Input

            _move.x = Input.GetAxis("Vertical");
            _move.y = Input.GetAxis("Horizontal");

            _look.x = Input.GetAxis("Mouse X");
            _look.y = Input.GetAxis("Mouse Y");

            #endregion

            #region Player Based Rotation

            //Move the player based on the X input on the controller
            //transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);

            #endregion

            #region Follow Transform Rotation

            //Rotate the Follow Target transform based on the input
            followTransform.transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);

            #endregion

            #region Vertical Rotation

            if (invertLookUp)
            {
                followTransform.transform.rotation *= Quaternion.AngleAxis(_look.y * rotationPower, Vector3.right);
            }
            else
            {
                followTransform.transform.rotation *= Quaternion.AngleAxis(_look.y * rotationPower, Vector3.left);
            }


            var angles = followTransform.transform.localEulerAngles;
            angles.z = 0;

            var angle = followTransform.transform.localEulerAngles.x;

            //Clamp the Up/Down rotation
            if (angle > 180 && angle < 340)
            {
                angles.x = 340;
            }
            else if (angle < 180 && angle > 40)
            {
                angles.x = 40;
            }


            followTransform.transform.localEulerAngles = angles;
            #endregion

            nextRotation = Quaternion.Lerp(followTransform.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);

            // Present difficulties conflicted with player rotation, temporary disabled.
            #region Free-look
        /*  
           if (Input.GetKey(KeyCode.Tab) || !playerController.isMoveAllowed)
            {
                nextPosition = transform.position;

                if (aimValue == 1)
                {
                    //Set the player rotation based on the look transform
                    transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
                    //reset the y rotation of the look transform
                    followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
                }

                return;
            }
            */
        #endregion


            float moveSpeed = speed / 100f;
            Vector3 position = (transform.forward * _move.y * moveSpeed) + (transform.right * _move.x * moveSpeed);
            nextPosition = transform.position + position;

           
            //Set the player rotation based on the look transform
            transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0) ;
            //reset the y rotation of the look transform
            followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
            
        
    }

    public void MouseLock()
    {
        if (!btnMLock)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void SimpleCameraController()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        camera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}
