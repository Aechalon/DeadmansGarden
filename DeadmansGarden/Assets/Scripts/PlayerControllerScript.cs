
using UnityEngine;
using System.Collections;
using StarterAssets;
using Photon.Pun;
using UnityEngine.UI;
[PunRPC]
public class PlayerControllerScript : MonoBehaviourPunCallbacks
{

    [Header("Game Components")]
    [SerializeField] private Player player;
    [SerializeField] private MiniGame miniGame;
    [SerializeField] private GameObject miniGameParent;
    [SerializeField] private StarterAssetsInputs inputs;
    [SerializeField] private ThirdPersonController tpController;
    [SerializeField] private Animator anim;
    [SerializeField] private ItemPickUp itemPickUp;
    [SerializeField] private Item item;
    [SerializeField] private UpperBound upperBound;
    [SerializeField] private Transform debugLog;
    [SerializeField] private GameObject objectThrow;
    [SerializeField] private GameObject flashLight;
    [SerializeField] private GameObject spotLight;
    [SerializeField] private GameObject keyPad;
    [SerializeField] private KeyPad keyPadScript;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject end;
    public Transform throwPoint;
    public GameObject screenLoad;
    [SerializeField] private Loadscreen loadScreen;
    [Header("Player Input Values")]
    [SerializeField] private int damageCount = 7;
    [SerializeField] private string pickUpKey;
    [SerializeField] private string itemName;
    [SerializeField] private string flashKey;
    [SerializeField] private bool onPickUp;
    [SerializeField] private bool cursorLock;
    [SerializeField] private bool itemUse;
    [SerializeField] private bool onPlayArea;
    [SerializeField] private bool isHaradin;
    [SerializeField] private bool miniGameStatus;
    [SerializeField] private bool miniGameBegin;
    [SerializeField] private bool isAlive;
    [SerializeField] private Transform playArea;
    [SerializeField] private bool tpReady;
    [SerializeField] private PlayerTeleport playerTP;
    public float throwPower;
    [SerializeField] private ObjectReposition obj;
    [SerializeField] private LabEntrance lab;
    [SerializeField] private GameObject tutorial;

    [Header("Inventory System")]
    [SerializeField] private InventorySystem inventory;

    [Header("User Interface")]
    [SerializeField] private Text txtMsg;
    [SerializeField] private Text txtInstruction;
    [SerializeField] private Text txtName;
    [SerializeField] private Text txtRec;
    [SerializeField] private Text txtQuest;
    [SerializeField] private Slider dialogueSlider;
    [SerializeField] private GameTutorial gameTutorial;
    [SerializeField] private Slider questSlider;


    [Header("Public Boolean")]
    public bool inQuest;
  

    #region System Voids

    [PunRPC]
    private void Awake()
    {
        if (photonView.IsMine)
        {
            inputs = GetComponent<StarterAssetsInputs>();
            anim = GetComponent<Animator>();
            tpController = GetComponent<ThirdPersonController>();
            player = GetComponent<Player>();
            upperBound = GetComponentInChildren<UpperBound>();
            gameManager = FindObjectOfType<GameManager>();
            loadScreen = screenLoad.GetComponent<Loadscreen>();
            gameTutorial = GetComponentInChildren<GameTutorial>();
            cursorLock = true;
           
            Invoke("ClearText", 5f);
  
         
        }


    }

    [PunRPC]
    private void Update()
    {
        if (photonView.IsMine)
        {
       

                if (txtInstruction == null)
                {
                    txtInstruction = GetComponentInChildren<Text>();
                }

                if (isHaradin)
                {

                }
                else
                {

                }

                inputs.SetCursorState(!cursorLock);
                if (isAlive)
                {

                    ManualInputs();
                    PlayerInputs();
                }
                else
                {
                    if (inputs.crouch)
                    {
                        anim.SetLayerWeight(8, Mathf.Lerp(anim.GetLayerWeight(8), 1f, Time.deltaTime * 10f));
                    }
                    else
                    {
                        anim.SetLayerWeight(7, Mathf.Lerp(anim.GetLayerWeight(7), 1f, Time.deltaTime * 10f));
                    }
                }
                if (player.health < 1)
                {
                    isAlive = false;
                }
                if (!playArea)
                {

                }



            }
            if (inQuest)
            {
                if (questSlider.value > 0)
                {
                    questSlider.value -= .2f;
                }
            }
            if(!inQuest)
            {
                if (questSlider.value < 1)
                {
                    questSlider.value += .2f;
                }
            }
        


    }

    #endregion

    #region PlayerControl
    [PunRPC]
    private void PlayerInputs()
    {
        if (tpController.Grounded)
        {

            if (Input.GetKeyDown(flashKey))
            {
                photonView.RPC("StateObjects", RpcTarget.All, true, 1);
                bool _flashlight = (spotLight.activeSelf) ? false : true;

                photonView.RPC("StateObjects", RpcTarget.All, _flashlight, 0);

            }


            if (inputs.crouch)
            {
                anim.SetLayerWeight(1, Mathf.Lerp(anim.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
                tpController._animCrouch = true;
                anim.SetBool("Crouch", true);
            }
            else if (upperBound.inBound)
            {
                anim.SetLayerWeight(1, Mathf.Lerp(anim.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            }
            else
            {
                anim.SetLayerWeight(1, Mathf.Lerp(anim.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
                tpController._animCrouch = false;
                anim.SetBool("Crouch", false);
            }


            if (onPickUp && itemPickUp != null && item != null)
            {
                if (photonView.IsMine)
                {
                    txtInstruction.text = "";
                }
                item = itemPickUp.item;
                if (tpController.targetSpeed > 5)
                {
                    anim.SetLayerWeight(3, Mathf.Lerp(anim.GetLayerWeight(3), 1f, Time.deltaTime * 10f));
                    anim.SetBool("PickUp", true);
                    _AnimatorApplyRoot();
                }
                else
                {
                    anim.SetLayerWeight(2, Mathf.Lerp(anim.GetLayerWeight(2), 1f, Time.deltaTime * 10f));
                    anim.SetBool("PickUp", true);

                }
            }
            if (!onPickUp)
            {

                anim.SetLayerWeight(2, Mathf.Lerp(anim.GetLayerWeight(2), 0f, Time.deltaTime * 10f));
                anim.SetLayerWeight(3, Mathf.Lerp(anim.GetLayerWeight(3), 0f, Time.deltaTime * 10f));

            }
        


            if (inputs.aim)
            {
                throwPoint.transform.LookAt(debugLog.transform);
            }
            if (itemUse)
            {
                if (inputs.trigger && !inputs.crouch)
                {
                    anim.SetLayerWeight(4, Mathf.Lerp(anim.GetLayerWeight(4), 1f, Time.deltaTime * 10f));
                    anim.SetBool("Throw", true);

                }
                if (inputs.trigger && inputs.crouch)
                {
                    anim.SetLayerWeight(5, Mathf.Lerp(anim.GetLayerWeight(5), 1f, Time.deltaTime * 10f));
                    anim.SetBool("Throw", true);
                }
            }
            else { return; }

        }



    }
    #endregion

    #region Other Voids

    public void SetQuestUpdate(string quest, bool completed)
    {
        if (photonView.IsMine)
        {
            if (!inQuest && !completed)
            {
                txtQuest.text = quest;
                //questSlider +
                inQuest = true;
            }
            if (inQuest)
            {
                if (completed)
                {
                    txtQuest.text = "Completed";
                  //slider -
                    inQuest = false;

                }
            }
        }
    }


     public void QuestFinish()
    {
        StartCoroutine(InitiateDialogue(CharacterName(isHaradin), "That should be enough", 4f, .05f, true));
        txtInstruction.text = "";
        SetQuestUpdate("", true); 
        obj.enabled = false;
        obj.gameObject.tag = "Untagged";

    }


    [PunRPC]
    private void ManualInputs()
    {
        cursorLock = inputs.tab;

    }



    [PunRPC]
    void StateObjects(bool state, int i)
    {
        switch (i)
        {
            case 0:
                spotLight.SetActive(state);
                break;
            case 1:
                flashLight.SetActive(state);
                break;
        }

    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }

    public void ClearText()
    {
        if (photonView.IsMine)
        {
            txtInstruction.text = "";
        }
    }

    [PunRPC]
    public void ThrowObject()
    {
        GameObject Throwable = PhotonNetwork.Instantiate(itemName, throwPoint.position, throwPoint.rotation);
        Physics.IgnoreCollision(Throwable.GetComponent<Collider>(), GetComponent<Collider>());
        Throwable.GetComponent<Rigidbody>().velocity = throwPoint.transform.forward * throwPower;

    }

    [PunRPC]
    public void EndThrowAnim()
    {
        anim.SetBool("Throw", false);
        itemUse = false;
        anim.SetLayerWeight(4, 0f);
        anim.SetLayerWeight(5, 0f);
    }

    #endregion

    #region IEnumerators

    IEnumerator InitiateDialogue(string name, string message, float interval, float txtInterval, bool me)
    {
        SetDialogueSlider(true);
        MyDialogue(name, me);
        StartCoroutine(TypeWriter(message, txtInterval));
        yield return new WaitForSeconds(interval);
        SetDialogueSlider(false);
    }
    IEnumerator TypeWriter(string message, float interval)
    {
        if (photonView.IsMine)
        {
            string result = "";
            foreach (char c in message)
            {
                result += c;
                yield return new WaitForSeconds(interval);

                txtMsg.text = result;
            }

        }
    }

    #endregion

    #region Void Dialogues

    private void MyDialogue(string name, bool me)
    {
        if (photonView.IsMine)
        {
            if (me)
            {
                txtName.text = name;

                txtRec.text = "";

            }
            else
            {
                txtRec.text = name;
                txtName.text = "";
            }
        }
    }

    private void SetDialogueSlider(bool onState)
    {
        if (onState)
        {

            dialogueSlider.value = 0;

        }
        else
        {
            dialogueSlider.value = 1;

        }

    }
    private void DialogueSwitch(int i)
    {
        if (photonView.IsMine)
        {
            switch (i)
            {
                case 0:
                    StartCoroutine(InitiateDialogue(CharacterName(isHaradin), "I may have to fill in the tank.", 4f, .05f, true));
                    break;
                case 1:
                    StartCoroutine(InitiateDialogue(CharacterName(isHaradin), "Exactly what I need.", 4f, .05f, true));
                    break;
                case 2:
                    StartCoroutine(InitiateDialogue(CharacterName(isHaradin), "I should put these back in the trashbin.", 4f, .05f, true));
                    break;
                case 3:
                    StartCoroutine(InitiateDialogue(CharacterName(isHaradin), "Here we go", 4f, .05f, true));
                    break;
                case 4:
                    StartCoroutine(InitiateDialogue(CharacterName(isHaradin), "Found the keycard", 4f, .05f, true));
                    break;
                case 5:
                    StartCoroutine(InitiateDialogue(CharacterName(isHaradin), "The lights working again", 4f, .05f, true));
                    break;
                case 6:
                    StartCoroutine(InitiateDialogue(CharacterName(isHaradin), "We need to fix the lights", 4f, .05f, true));
                    break;
            }
        }
    }
    public void SetDialogue(string message)
    {
        if (photonView.IsMine)
        {
            StartCoroutine(InitiateDialogue(CharacterName(isHaradin), message, 4f, .05f, true));
        }
    }

    private string CharacterName(bool isHaradin)
    {
        string name = "";
        if (isHaradin)
        {
            return name = "Haradin";
        }
        return name = "Ellia";

    }



    #endregion

    #region Void Triggers


    [PunRPC]
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Collectibles"))
        {
            itemPickUp = other.GetComponent<ItemPickUp>();
            item = itemPickUp.item;
            if (photonView.IsMine)
            {
                txtInstruction.text = "Pick Up " + itemPickUp.item.name;

            }
        }
        if (other.CompareTag("Container"))
        {
            obj = other.gameObject.GetComponent<ObjectReposition>();

            if (obj != null)
            {
                if (photonView.IsMine)
                {
                    txtInstruction.text = "E to Interact";
                }
                if (obj.isRising && !obj.GetQuest())
                {

                    StartCoroutine(InitiateDialogue(CharacterName(isHaradin), "I'm gonna need a source power, I may have to take that AI down.", 5f, .05f, true));

                }
            }

        }
        if (other.CompareTag("Teleport"))
        {
            playerTP = other.gameObject.GetComponent<PlayerTeleport>();
            if (photonView.IsMine)
            {
                txtInstruction.text = "E to Interact";
            }
        }
        if (other.CompareTag("End"))
        {

            if (photonView.IsMine)
            {
                end.SetActive(true);
                Invoke("Leave", 3f);
            }
        }
        if (other.CompareTag("Wall"))
        {
            Debug.Log("You hit a wall");
            //InitiateDialogue(name,message,interval,txtinterval,isMe);

            StartCoroutine(InitiateDialogue(CharacterName(isHaradin), "I should probably pick it up", 4f, .05f, true));


        }
        if (other.CompareTag("SafeZone"))
        {

            StartCoroutine(InitiateDialogue(CharacterName(isHaradin), "I think its safe now.", 4f, .05f, true));
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("DialogueSphere"))
        {
            DialogueInt dialogue = other.GetComponent<DialogueInt>();
            DialogueSwitch(dialogue.dialogueNum);
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("Metropolis"))
        {

            StartCoroutine(InitiateDialogue(CharacterName(isHaradin), "I'm gonna have to hide around here until Haradin contacts me", 4f, .05f, true));

            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("LabEntrance"))
        {
            lab = other.GetComponent<LabEntrance>();
            if (photonView.IsMine)
            {
                txtInstruction.text = "E to Interact";
            }


        }
        if (other.CompareTag("Elevator"))
        {
       
            if (photonView.IsMine)
            {
                txtInstruction.text = "E to call the Elevator";
            }


        }
        if (other.CompareTag("GameArea"))
        {
            if (photonView.IsMine)
            {
                onPlayArea = true;
            }
        }

        if(other.CompareTag("Tutorial"))
        {
            if(photonView.IsMine)
            {
                int tutorialNumber = other.GetComponent<SetTutorial>().tutorialNumber;
                    switch (tutorialNumber)
                    {
                    case 0:
                        // Message, BeginDelay, TextSpeed, TutorialNumber
                        gameTutorial.SetTutorial("WASD Key to Move", 1f, .04f, tutorialNumber);
                        break;
                    case 1:
                        // Message, BeginDelay, TextSpeed, TutorialNumber
                        gameTutorial.SetTutorial("Shift to Run", 1f, .04f, tutorialNumber);
                        break;
                    case 2:
                        // Message, BeginDelay, TextSpeed, TutorialNumber
                        gameTutorial.SetTutorial("E to Pickup Objects", 1f, .04f, tutorialNumber);
                        break;
                    case 3:
                        // Message, BeginDelay, TextSpeed, TutorialNumber
                        gameTutorial.SetTutorial("CTRL to Crouch/Hide", 1f, .04f, tutorialNumber);
                        break;
                    case 4:
                        // Message, BeginDelay, TextSpeed, TutorialNumber
                        gameTutorial.SetTutorial("Hold Tab to open Menu, and use an Item by clicking it.", 1.4f, .04f, tutorialNumber);
                        break;
                    case 5:
                        // Message, BeginDelay, TextSpeed, TutorialNumber
                        gameTutorial.SetTutorial("You can interact with objects", 1f, .04f, tutorialNumber);
                        break;
                    case 6:
                        // Message, BeginDelay, TextSpeed, TutorialNumber
                        gameTutorial.SetTutorial("Right click to Aim, Left click to Throw", 1.3f, .04f, tutorialNumber);
                        break;

                }
                
                
            }
        }
        if(other.CompareTag("Quest"))
        {
            if (photonView.IsMine)
            {
                if (!inQuest)
                {
                    int questNumber = other.GetComponent<SetQuest>().questNumber;
                    
                    switch (questNumber)
                    {
                        case 0:
                            SetQuestUpdate("Find a way home", false);
                            break;
                        case 1:
                            SetQuestUpdate("Prevent being seen and find a safe place", false);
                            break;
                        case 2:
                            SetQuestUpdate("Find a hiding place", false);
                            break;
                        case 3:
                            SetQuestUpdate("Find a way to powerup the tower", false);
                            break;
                    }

                }
            }
        }

    }

    [PunRPC]
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Collectibles"))
        {
            if (Input.GetKeyDown(pickUpKey))
            {
                onPickUp = true;

            }
        }
        if (other.CompareTag("Teleport"))
        {
            if (Input.GetKeyDown(pickUpKey) && playerTP != null)
            {
                if (photonView.IsMine)
                {

                    txtInstruction.text = "";
                }
                screenLoad.SetActive(true);
                loadScreen.moveTeleport = true;
                onPlayArea = true;


            }
        }

        if (other.CompareTag("PowerSource"))
        {
            if (photonView.IsMine)
            {
                bool status = miniGameStatus;
                if (!status)
                {
                    txtInstruction.text = "E to Interact";
                    if (Input.GetKeyDown(pickUpKey))
                    {
                        miniGameParent.SetActive(true);
                    }
                }

            }
        }
        if (other.CompareTag("Keypad"))
        {

            if (!keyPadScript.GetStatus())
            {
                if (photonView.IsMine)
                {
                    txtInstruction.text = "E to Interact";
                }
                    if (Input.GetKeyDown(pickUpKey))
                {
                    if (keyPad != null)
                    {
                        keyPad.SetActive(true);
                    }
                }
            }
        }

        if (other.CompareTag("LabEntrance"))
        {
            if (Input.GetKeyDown(pickUpKey))
            {
                
                if (photonView.IsMine)
                {
                    lab.SetActivate(true);
                    txtInstruction.text = "";

                }
            }
        }
        if (other.CompareTag("Container"))
        {
            if (photonView.IsMine)
            {
                if (Input.GetKeyDown(pickUpKey))
                {
                    if (obj.GetQuest())
                    {
                        txtInstruction.text = "";
                        QuestFinish();
                    }
                    else
                    {
                        if (obj != null)
                        {
                            obj.boolRepos = true;


                            if (inventory.ItemDuplicate(obj.item))
                            {
                                int itemNeeded = obj.collectAmount - obj.collected;
                                txtInstruction.text = "I need " + itemNeeded.ToString() + " more";
                                if (obj.collected < obj.collectAmount)
                                {
                                
                                    if (player.itemQuantity[inventory.PlayerIndex(obj.item)] >= 1)
                                    {
                                        obj.collected += 1;
                                        player.itemQuantity[inventory.PlayerIndex(obj.item)] -= 1;

                                    }
                                    else
                                    {
                                        inventory.Remove(obj.item, false);
                                    }
                                }
                                else
                                {
                                   
                                }
                            }
                            else
                            {
                                StartCoroutine(InitiateDialogue(CharacterName(isHaradin), "I need " + obj.item.itemName.ToString(), 4f, .05f, true));
                            }


                        }
                    }
                  


                }
            }
        }

    }



    [PunRPC]
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Collectibles"))
        {                                                      //  Disables the collectible boolean upon exit.
            item = null;
            if (photonView.IsMine)
            {
                txtInstruction.text = "";
            }
        }
        if (other.CompareTag("Container"))
        {                                                      //  Disables the collectible boolean upon exit.
            obj = null;
        }
        if (other.CompareTag("Elevator"))
        {

            if (photonView.IsMine)
            {
                txtInstruction.text = "";
            }


        }
        if (other.CompareTag("Teleport"))
        {
            if (photonView.IsMine)
            {
                playerTP = null;
          
                txtInstruction.text = "";
            }
        }
        if (other.CompareTag("GameArea"))
        {
            if (photonView.IsMine)
            {
                onPlayArea = false;
                StartCoroutine(TakeDamage(2));
            }

        }
        if (other.CompareTag("Wall"))
        {

        }
        if (other.CompareTag("Keypad"))
        {
            if (photonView.IsMine)
            {
                keyPad.SetActive(false);
           
                txtInstruction.text = "";
            }
        }
        if (other.CompareTag("PowerSource"))
        {
            if (photonView.IsMine)
            {

                txtInstruction.text = "";
                miniGameParent.SetActive(false);
            }

        }
        if (other.CompareTag("LabEntrance"))
        {
            if (photonView.IsMine)
            {
                lab = null;
           
                txtInstruction.text = "";
            }
        }
        if (other.CompareTag("Tutorial"))
        {
            if (photonView.IsMine)
            {
            int tutorialNumber = other.GetComponent<SetTutorial>().tutorialNumber;
            gameTutorial.TutorialClear(tutorialNumber);
            }
        }
    }

    #endregion 

 
    #region Void Animations

    [PunRPC]
    public void _PickUpAnimationEnd()
    {

        if (photonView.IsMine)
        {
            onPickUp = false;
            anim.SetBool("PickUp", false);
        }
    }         
    [PunRPC]   
    public void _AnimatorApplyRoot()
    {

        anim.applyRootMotion = true;
        anim.updateMode = AnimatorUpdateMode.AnimatePhysics;

    }
    [PunRPC]
    public void _AnimatorNormalRoot()
    {

        anim.applyRootMotion = false;
        anim.updateMode = AnimatorUpdateMode.Normal;

    }
    [PunRPC]
    public void GetItem()
    {
        if (photonView.IsMine)
        {
            if (item != null)
            {
                inventory.Add(item);
                if (itemPickUp != null)
                {
                    PhotonNetwork.Destroy(itemPickUp.gameObject);
                }

                item = null;
                if (photonView.IsMine)
                {
                    txtInstruction.text = "";
                }
            }
          
        }


    }
    #endregion

    #region Get Voids
    public bool GetPlayerTeleportState()
    {
        bool _teleport = tpReady;

        return _teleport;
    }

    public bool GetAimState()
    {
        bool aim = inputs.aim;

        return aim;
    }

    [PunRPC]
    public bool GetMiniGameTask()
    {
        return miniGameStatus;
    }
    [PunRPC]
    public bool GetMiniGame()
    {
        return miniGameBegin;

    }
    [PunRPC]
    public string GetInteractString()
    {
        return pickUpKey;
    }
    public bool GetLifeStatus()
    {

        return isAlive;

    }

    public bool GetHaradin()
    {
        return isHaradin;
    }
    public bool GetCrouchState()
    {
        bool crouch = inputs.crouch;

        return crouch;
    }
    #endregion

    #region Set Voids

    [PunRPC]
    public void SetPlayerController(bool newState)
    {
        tpController.enabled = newState;
    }

    [PunRPC]
    public void SetMiniGameTask(bool newState)
    {
        miniGameStatus = newState;
    }

    public void SetItemName(string newItemName)
    {
        itemName = newItemName;

    }

    public void SetItemUse(bool newState)
    {
        itemUse = newState;
    }

    public void SetTeleport()
    {
        playerTP.SetTeleport();
    }

    #endregion


    #region Receive Damage
    private void DealDamage()
    {
        player.health -= damageCount;
    }

    IEnumerator TakeDamage(float duration)
    {

            int i = 4;
        if (photonView.IsMine)
        {
            while (player.health > 0)
            {
                if (!onPlayArea)
                {

                    switch (i)
                    {
                        case 0:
                            StartCoroutine(InitiateDialogue(CharacterName(isHaradin), "I can't breathe.", 4f, .05f, true));
                            break;
                        case 1:
                            StartCoroutine(InitiateDialogue(CharacterName(isHaradin), "It's toxic out here.", 4f, .05f, true));
                            break;
                        case 2:
                            StartCoroutine(InitiateDialogue(CharacterName(isHaradin), "I can't stay out here for too long", 4f, .05f, true));
                            break;
                     
                        default:
                            break;


                    }
                    yield return new WaitForSeconds(duration);



                }
                else
                {
                    yield return null;
                }
                i = Random.Range(0, 3);


            }
        }
    
    }
    #endregion


}
