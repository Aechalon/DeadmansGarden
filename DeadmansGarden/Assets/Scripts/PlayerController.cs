
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviourPunCallbacks
{

    /// <summary>
    /// Player Controller dictates how the player will behave and interact within the environment
    /// </summary>

    #region Player Movement Variables

    [Header("Player Movement Variables")]


    Vector3 moveDirection = Vector3.zero;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private UpperBound headCheck;

    [SerializeField] private float gravity = 10.0f;
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 4.0f;
    [SerializeField] private float characterHeight = 1.8f;

    [SerializeField] private float curSpeedX;
    [SerializeField] private float curSpeedY;
    [SerializeField] private float xPos;
    [SerializeField] private float yPos;

    [SerializeField] private ItemPickUp itemPickUp;
    [SerializeField] private Item item;

    [SerializeField] private float jumpForce;
    [SerializeField] private float distanceToGround;
    [SerializeField] private float flightTime;

    [SerializeField] private Text txtFlightTime;
    [SerializeField] private Text txtDistance;

    #endregion

    #region Player Input Variables

    [Header("Player Input Variables")]

    [SerializeField] private string forward;
    [SerializeField] private string backward;
    [SerializeField] private string left;
    [SerializeField] private string right;
    [SerializeField] private string grab;
    [SerializeField] private string push;

    #endregion

    #region Player Animator Variables

    [Header("Player Animator Variables")]

    [SerializeField] private Animator anim;
    [SerializeField] private Animation pickUpAnim;

    [SerializeField] public bool isMoveAllowed;

    [SerializeField] private bool isJumping;
    [SerializeField] private bool isHaradin;
    [SerializeField] private bool collectible;
    [SerializeField] private bool isGrabbing;
    [SerializeField] private bool isFalling;
    [SerializeField] private bool isGrounded;

    [SerializeField] private bool moveWalk;
    [SerializeField] private bool moveLeft;
    [SerializeField] private bool moveRight;
    [SerializeField] private bool moveBackward;
    [SerializeField] private bool moveRun;
    [SerializeField] private bool moveGrab;
    [SerializeField] private bool moveJump;
    [SerializeField] private bool movePush;
    [SerializeField] private bool moveCrouch;


  

    #endregion

    #region Void Awake Function
    [PunRPC]
    private void Awake()
    {



        /// <summary> 
        /// Check if the Camera is Foreign and Destroy it.
        /// </summary>>
     

        isMoveAllowed = true;
    }
    #endregion

    #region Void Update Function

    [PunRPC]
    private void Update()
    {
        if (!photonView.IsMine)
        {

       

            Destroy(GetComponentInChildren<UpperBound>());
        }
        else
        {
            headCheck = GetComponentInChildren<UpperBound>();

        }

        if (photonView.IsMine)                                    // Check if the photonView is local.
        {
            PlayerInput();                                        // Calls Player Input Function.
            PlayerMovement();
            DistanceGround();                                     // Check for fDistanceToGound
            anim.SetFloat("hVelocity", distanceToGround);
        }




        /// <summary>
        /// Provides an output regarding how the Max Behaviour should behave.
        /// </summary>

    }
    #endregion

    /// <summary>
    /// Player Keyboard Inputs.
    /// </summary>

    #region Player Input Function

    [PunRPC]
    private void PlayerInput()
    {
        moveWalk = Input.GetKey(forward);
        moveLeft = Input.GetKey(left);
        moveRight = Input.GetKey(right);
        moveBackward = Input.GetKey(backward);
        if (!pickUpAnim.isPlaying)
        {
            moveRun = Input.GetKey(KeyCode.LeftShift) && !headCheck.inBound;
        }

        
        moveGrab = Input.GetKey(grab);
        movePush = Input.GetKey(push);
        moveJump = Input.GetKeyDown(KeyCode.Space);
      
    }
    #endregion

    /// <summary>
    /// Player Movement and Navigation.
    /// </summary>

    #region Player Movement Function
    [PunRPC]
    private void PlayerMovement()
    {

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float xMovement = Input.GetAxis("Vertical");
        float yMovement = Input.GetAxis("Horizontal");

        float movementDirectionY = moveDirection.y;

        bool walk = (moveWalk || moveBackward || moveLeft || moveRight) ? true : false;      // Defines any directional movement as walk.

        // Calculations

        curSpeedX = (moveRun ? runSpeed : walkSpeed) * xMovement;
        curSpeedY = (moveRun ? runSpeed : walkSpeed) * yMovement;

        isFalling = (!isJumping && !isGrounded && distanceToGround > 3 && flightTime > .6f) ? true : false;
        isGrounded = (characterController.isGrounded) ? true : false;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);


        // Variable Application
        if (isMoveAllowed)
        {
            if (!isJumping)
            {
                if (walk && !moveRun)              //For Walking          
                {
                    xPos = xMovement ;
                    yPos = yMovement ;
                }
                else if (moveRun && walk)          //For Running
                {
                    xPos = xMovement * 2.1f;
                    yPos = yMovement * 2.1f;

                }
                else if (moveCrouch && walk)
                {
                    xPos = xMovement * .03f;
                    yPos = yMovement * .03f;
                }
                else
                {
                    xPos = xMovement;
                    yPos = yMovement;
                }
            }
      
        }
      
        #region Player Jumping

        if (moveJump && !isJumping && moveRun                     // Check multiple player states. Such as if the player jumped, is jumping
                && isGrounded && isMoveAllowed)                   // is the character grounded and if movement is allowed.
        {
            moveDirection.y = jumpForce;
            isJumping = true;
            isMoveAllowed = false;
            // Disables movement upon jumping.


        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (isJumping && moveDirection.y <= 0)
        {
            isJumping = false;

        }

        if (!isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;

        }

        #endregion

        #region Object Interactions
        // Player Object Interactions
        #endregion

        #region Crouch Animation

        if (isMoveAllowed && distanceToGround < .3f)
        {
            bool isCrouchHit = Input.GetKeyDown(KeyCode.LeftControl);

            if (isCrouchHit && moveCrouch == false)
            {
                moveCrouch = true;
            }
            else if (moveCrouch == true && isCrouchHit && !headCheck.inBound)
            {
                moveCrouch = false;
            }

        }
        if (moveRun && !headCheck.inBound)
        {
            moveCrouch = false;

        }
        

        float i = characterHeight;
        if(moveCrouch && i > .7f)
        {
                i -= .1f;
            

        }
        else if(!moveCrouch && i < 1.8f)
        {
            
                i += .1f;
            

        }

        characterHeight = i;
        if (isHaradin)
        {
            characterController.height = (moveCrouch) ? .8f : 1.8f;
            characterController.center = (moveCrouch) ? new Vector3(0, .55f, 0) : new Vector3(0, .9f, 0);
        }
       else
        {
            characterController.height = (moveCrouch) ? .8f : 1.58f;
            characterController.radius = (moveCrouch) ? 0.48f : 0.4f;
            characterController.center = (moveCrouch) ? new Vector3(0, .55f, 0) : new Vector3(0, .8f, 0);
        }
        #endregion




        #region Assignation

        if (!isJumping || !moveCrouch)
        {
            anim.SetFloat("PosX", xPos, .1f, Time.deltaTime);                      // Assign xPos for Animator Float X Position.
            anim.SetFloat("PosY", yPos, .1f, Time.deltaTime);                      // Assign yPos for Animator Float Y Position.
        }
            anim.SetBool("isCrouch", moveCrouch);
        anim.SetFloat("cHeight", characterHeight);


        characterController.Move(moveDirection * Time.deltaTime);               // Applies Character Movement.


        #endregion


    }
    #endregion

    /// <summary>
    /// Calculates the Distance from Ground to Air and Flight Time.
    /// </summary>

    #region Distance to Ground Function

    private void DistanceGround()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            distanceToGround = hit.distance;
     //       txtDistance.text = distanceToGround.ToString();
            if (distanceToGround > .3f && !isGrounded)
            {
                anim.SetBool("isJump", true);
            }
            else
            {
                anim.SetBool("isJump", false);
            }
        }

        if (!isGrounded)
        {
            flightTime += Time.deltaTime;                 //Increase Flight Time
         //   txtFlightTime.text = flightTime.ToString();
            isMoveAllowed = false;
        }
        else
        {
            flightTime = 0;
            isMoveAllowed = true;
        }
    }


    #endregion

    /// <summary>
    /// Functions that disables the character animations. Often used with Invoke method.
    /// </summary>
    /// 

    #region Additional Animations
    [PunRPC]
    private void GrabAnim(Collider other, Item item)
    {

        if (!moveRun && item != null)
        {

            if (!moveCrouch)
            {
                pickUpAnim.Play();
               
            }

            /*    if (InventorySystem.instance.Add(item) == true)   // Add to inventory
                {
                    Debug.Log("Picking up " + item.name);

                    PhotonNetwork.Destroy(other.gameObject);    // Destroy item from scene

                }
            */

        }


    }



    #endregion
    


    #region End Functions

    [PunRPC]
    private void StopGrab()                                        // Function to re-enable grabbing and movement.
    {
        isGrabbing = false;
        isMoveAllowed = true;
        anim.SetBool("isGrab", false);
        characterController.enabled = true;
    }
    #endregion

    /// <summary>
    /// Collisions and Triggers that the player may interact with.
    /// </summary>

    #region Colliders and Collision

    [PunRPC]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectibles"))
        {
            collectible = true;                                   //  Enable grab animation if an item is collectible.
            itemPickUp = other.gameObject.GetComponent<ItemPickUp>();
            item = itemPickUp.item;
        }


    }
    [PunRPC]
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Collectibles"))
        {
            if (moveGrab)
            {
                GrabAnim(other, item);                                   //  Enable grab animation if an item is collectible.
            }
        }


    }


    [PunRPC]
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Collectibles"))
        {
            collectible = false;                                  //  Disables the collectible boolean upon exit.
            item = null;
        }
    }
    #endregion


}
