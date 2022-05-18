using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
[PunRPC]
public class MiniGame : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerControllerScript player;
    [SerializeField] private Text txtTimer;
    [SerializeField] private ImageControl selection;
    [SerializeField] private string tool;
    [SerializeField] private GameObject[] tools;
    [SerializeField] private GameObject parent;
    [SerializeField] private float setTime;
    [SerializeField] private bool selected;
    [SerializeField] private bool time;
    [SerializeField] private bool taskCompleted;
    private Collider2D col;
    private int i;
    private float timer = 5f;
    private void Awake()
    {
        player = GetComponentInParent<PlayerControllerScript>();
    }
    private void OnEnable()
    {
        ResetActives(tools);
        i = Random.Range(0, tools.Length);
        tools[i].SetActive(true);
        SetTool(i);
        timer = setTime;
        time = true;
    }

    private void Update()
    {
        Timer();
      
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        col = collision;
        if (selected)
        {
            if (collision.CompareTag(tool))
            {
                Execute();
                taskCompleted = true;
                SetMiniGame(true);
               
            }
           
        } 
      
    }

    [PunRPC]
    private void SetMiniGame(bool newState)
    {
        player.SetMiniGameTask(newState); //rpc      
    }
    [PunRPC]
    private void Timer()
    {
        if (time)
        {
            string newMin = "";
            string newSec = "";
            float minutes = Mathf.Floor(timer / 60);
            float seconds = Mathf.RoundToInt(timer % 60);
            if (timer > 0)
            {
                timer -= 1 * Time.deltaTime;
            }
            else
            {
                Execute();

            }
            if(minutes < 10) { newMin = "0" + minutes.ToString(); }
            else { newMin = minutes.ToString(); }
            if(seconds < 10) { newSec = "0" + seconds.ToString(); }
            else { newSec = seconds.ToString(); }
            txtTimer.text = newMin + " : " + newSec; 
        }
    }
    [PunRPC]
    private void Execute()
    {
        selection.SetSliderValue(0);
        tools[i].SetActive(false);
        parent.SetActive(false);
        time = false;
        selected = false;
        txtTimer.text = "";
    }
    [PunRPC]
    public void SetTool(int i)
    {
        switch(i)
        {
            case 0:
                tool = "TopTool";
                break;
            case 1:
                tool = "MidTool";
                break;
            case 2:
                tool = "BottomTool";
                break;
            case 3:
                tool = "LeftTool";
                break;
            case 4:
                tool = "RightTool";
                break;
        }
    }
    [PunRPC]
    public void ResetActives(GameObject[] obj)
    {
        for(int i = 0; i < obj.Length; i++)
        {
            tools[i].SetActive(false);
        }
    }
    [PunRPC]
    public void SetSelection()
    {
        if (col.CompareTag(tool))
        {
            selection.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
            selected = true;
        }
    }
    [PunRPC]
    public bool GetTaskStatus()
    {
        bool task = taskCompleted;
        taskCompleted = false;
        return task;
    }
  
  
}
