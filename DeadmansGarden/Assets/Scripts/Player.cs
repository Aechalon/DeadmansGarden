using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Photon.Pun;
using StarterAssets;
public class Player  : MonoBehaviourPunCallbacks
{
    InventorySystem inventory;
    ItemReferences itemRef;

    [SerializeField] private ThirdPersonController thirdPersonController;
 


    #region Character Stats
    public int health;
    public int hunger;
    public int scraps;
    public int bagSpace;
    public int handSpace;
    public int[] handItem = new int[2];

  
    public float[] pos; 

    #endregion

    #region Inventory Items

    public int [] itemID = new int[20], itemQuantity = new int[20];


    #endregion

    [PunRPC]
    public void Start()
    {
        if (photonView.IsMine)
        {
            inventory = InventorySystem.instance;
            itemRef = FindObjectOfType<ItemReferences>();
        }


    }
    [PunRPC]
    public void SavePlayer()
    {
      
            thirdPersonController.enabled = false;
            SaveSystem.SavePlayer(this);
            thirdPersonController.enabled = true;
        
    }
    [PunRPC]
    public void LoadPlayer()
    {
      
            thirdPersonController.enabled = false;
            PlayerData data = SaveSystem.LoadPlayer();

            Vector3 position;
            position.x = data.pos[0];
            position.y = data.pos[1];
            position.z = data.pos[2];


            hunger = data.hunger;
            scraps = data.scraps;
            health = data.health;
            bagSpace = data.bagSpace;
            handSpace = data.handSpace;
            itemID = new int[20];
            handItem = new int[2];
            itemQuantity = new int[20];




            for (int i = 0; i < bagSpace; i++)
            {
                itemID[i] = data.itemID[i];
            }
            for (int i = 0; i < data.itemQuantity.Length; i++)
            {
                itemQuantity[i] = data.itemQuantity[i];
            }

            for (int i = 0; i < handSpace; i++)
            {
                handItem[i] = data.handItem[i];
            }

            inventory.items.Clear();
            inventory.pItems.Clear();

            for (int i = 0; i < bagSpace; i++)
            {
                inventory.items.Add(itemRef.gameItems[data.itemID[i]]);

            }
            for (int i = 0; i < handSpace; i++)
            {
                inventory.pItems.Add(itemRef.gameItems[data.handItem[i]]);

            }



            if (inventory.onItemChangedCallback != null)
                inventory.onItemChangedCallback.Invoke();

            transform.position = position;
            thirdPersonController.enabled = true;
        
    }



    [Header("UI Components")]
    [SerializeField] private Image imgHealth;
    [SerializeField] private Image imgHunger;
    [SerializeField] private Image imgStamina;
    [SerializeField] private Text txtScraps;


    #region UI
    private void Update()
    {
        float health = (float)this.health;
        float hunger = (float)this.hunger;
        txtScraps.text = scraps.ToString();
        imgHealth.fillAmount = health / 100;
        imgHunger.fillAmount = hunger / 100;
    }

  

    #endregion
}
