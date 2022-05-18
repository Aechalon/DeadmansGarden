using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
/* This object manages the inventory UI. */

public class InventoryUI : MonoBehaviourPunCallbacks {

    public GameObject inventoryUI;  // The entire UI
    public Transform itemsParent;	// The parent object of all the items
    [SerializeField] InventorySystem inventory;  // Our current inventory
    [SerializeField] private StarterAssets.StarterAssetsInputs inputs;
    [SerializeField] private bool isBag;
  
   
    [PunRPC]
    private void Start()
    {
        if (photonView.IsMine)
        {
           
            inventory.onItemChangedCallback += UpdateUI;

        }


    }
    [PunRPC]
    // Check to see if we should open/close the inventory
    void Update()
    {
        if (photonView.IsMine)
        {
            UpdateUI();
        }
    }

    // Update the inventory UI by:
    //		- Adding items 
    //		- Clearing empty slots
    // This is called using a delegate on the Inventory.
    [PunRPC]
    public void UpdateUI()
    {
        InventorySlot[] slots = GetComponentsInChildren<InventorySlot>();
        if (slots != null)
        {
            if (isBag)
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    if (i < inventory.items.Count)
                    {
                        slots[i].AddItem(inventory.items[i], true);

                        slots[i].ItemQuantity(inventory.GetItemQuantity(i));
                    }
                    else
                    {
                        slots[i].ClearSlot(true);

                    }
                }
            }
            else
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    if (i < inventory.pItems.Count)
                    {
                        slots[i].AddItem(inventory.pItems[i], false);
                    }
                    else
                    {
                        slots[i].ClearSlot(false);
                    }
                }
            }
        }
        else
        {
            return;
        }
    }

 

}
