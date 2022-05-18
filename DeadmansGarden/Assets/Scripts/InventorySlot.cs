
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class InventorySlot : MonoBehaviourPunCallbacks
{
    public Image icon;          // Reference to the Icon image
    public Button removeButton; // Reference to the remove button
    public Text txtStack;
    public int stack;
    [SerializeField] private Player playerData;
    [SerializeField] private PlayerControllerScript player;
    Item item;  // Current item in the slot

    private void Awake()
    {
        if(photonView.IsMine)
        {

            playerData = GetComponentInParent<Player>();
            player = GetComponentInParent<PlayerControllerScript>();
         
        }
    }

    public void ItemQuantity(int count)
    {
        txtStack.text = count.ToString();
    }

    // Add item to the slot
    public void AddItem(Item newItem, bool isBag)
    {
        InventorySystem instance = InventorySystem.instance;
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
        if(isBag)
        {
           stack = instance.GetItemQuantity(instance.ItemIndex(item));
            txtStack.text = stack.ToString(); ;
        }

    }

    // Clear the slot
    public void ClearSlot(bool isBag)
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
        if(isBag)
        {
            txtStack.text = "";
        }
  
    }

    // Called when the remove button is pressed
    public void OnRemoveButton()
    {
        if (PhotonNetwork.IsConnected)
        {
            InventorySystem.instance.Remove(item, true);
            txtStack.text = "";
        }
        return;
    }

    // Called when the item is pressed
    public void UseItem()
    {

        InventorySystem instance = InventorySystem.instance;
        if (PhotonNetwork.IsConnected)
        {
            if (item != null)
            {
                if (instance.GetItemQuantity(instance.ItemIndex(item)) > 1)
                {

                    if (item.isThrowable)
                    {
                        playerData.itemQuantity[instance.ItemIndex(item)] -= 1;        
                        Use(instance, false);
                    }

                    //Item is Passive;
                }
            else
                {
                    if (item.isThrowable)
                    {
                        Use(instance, true);
                    }

                }

            }

            else
            {
                return;
            }
             
        }
        else
        {
            return;
        }
       
    }

    private void Use(InventorySystem instance, bool isZero)
    {
        player.SetItemName(item.name);
        player.SetItemUse(true);
        if (isZero)
        {
            instance.Remove(item, false);
        }
    }
    public int GetItemQuantity()
    {
        return 0;
    }

   

}
