using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using System;
[System.Serializable]
public class InventorySystem : MonoBehaviourPunCallbacks
{

    #region Singleton
    public static Player player;
    public static InventorySystem instance;

    [SerializeField] private Item item;
    [SerializeField] private Transform itemReturn;
    [SerializeField] private PlayerControllerScript playerController;

    void Awake()
    {
        itemRef = FindObjectOfType<ItemReferences>();
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");

            return;
        }
        if (photonView.IsMine)
        {
            player = FindObjectOfType<Player>();
            playerController = GetComponentInParent<PlayerControllerScript>();
            instance = this;

        }
        if(!photonView.IsMine)
        {
            GameObject inventory = FindObjectOfType<InventorySystem>().gameObject;
            Destroy(inventory);
        }
    }

    #endregion

    // Callback which is triggered when
    // an item gets added/removed.
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    [SerializeField] private ItemReferences itemRef;

    public int space = 20;  // Amount of slots in inventory
    public int handItem = 2;  //  0 Indicates Left-hand item and such.
    public int bagSpace = 1;

    // Current list of items in inventory
    public List<Item> items;

    public List<Item> pItems;
    // Add a new item. If there is enough room we
    // return true. Else we return false.


    public bool Add(Item _item)
    {

        // Don't do anything if it's a default item
        if (_item == null)
        {
            return false;
        }
        else
        {
            item = _item;

            if (!_item.isPrimaryItem)
            {
                // Check if out of space
                if (items.Count >= space)
                {

                    return false;
                }

                if (!ItemDuplicate(_item)) // duplicated
                {
                    player.itemQuantity[player.bagSpace] += 1; // + 1 _item count
                    items.Add(itemRef.gameItems[_item.itemID]); // + 1 Inventory UI
                    player.itemID[player.bagSpace] = _item.itemID; // ItemID[bagspace] = itemID
                    player.bagSpace += 1; // Inventory + 1 
                }
                else //different new _item picked
                {

                    player.itemQuantity[ItemIndex(_item)] += 1; // Current Item = +1

                }




                // Trigger callback
                if (onItemChangedCallback != null)
                    onItemChangedCallback.Invoke();

            }
            if (_item.isPrimaryItem)
            {
                if (pItems.Count >= handItem)
                {

                    return false;

                }

                pItems.Add(itemRef.gameItems[_item.itemID]);
                player.handItem[player.handSpace] = _item.itemID;
                player.handSpace += 1;


                // Trigger callback
                if (onItemChangedCallback != null)
                    onItemChangedCallback.Invoke();

            }


        }
        return true;
    }

    [PunRPC]
    public void Remove(Item item, bool manual)
    {

        if (!item.isPrimaryItem)
        {

            if (manual)
            { ItemInstantiate(item); }  //Instantiates if its removed from inventory

            if (player.itemQuantity[PlayerIndex(item)] >= 1)
            {
                player.itemQuantity[PlayerIndex(item)] -= 1;

            }
            if (player.itemQuantity[PlayerIndex(item)] < 1)
            {
                player.itemQuantity =  RemoveIndex(player.itemQuantity, ItemIndex(item));
                items.Remove(item);     // Remove item from list
                player.bagSpace -= 1;
                ItemChecker(item, items.Count);
                player.itemID[items.Count] = 0;

            }


        }
        if (item.isPrimaryItem)
        {

            player.handSpace -= 1;
            if (manual)
            { ItemInstantiate(item); }
            pItems.Remove(item);
            //        ItemChecker(item);
            //       player.handItem = player.handItem.Except(new int[2] { item.itemID }).ToArray();

        }
        // Trigger callback
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void ItemChecker(Item item, int itemCount)
    {
        if (!item.isPrimaryItem)
        {
            for (int i = 0; i < itemCount; i++)
            {
                player.itemID[i] = items[i].itemID;
            }
        }
        if (item.isPrimaryItem)
        {
            for (int i = 0; i < pItems.Count; i++)
            {
                player.handItem[i] = pItems[i].itemID;
            }
        }
    }
    public void QuantityChecker(Item item, int itemCount)
    {

        for (int i = 0; i < itemCount; i++)
        {
            player.itemQuantity[i] = items[i].itemID;
        }


    }

    public void ItemInstantiate(Item item)
    {
        string[] items = new string[itemRef.gameItems.Count];
        string itemName;
        for (int i = 0; i < itemRef.gameItems.Count; i++)
        {
            items[i] = itemRef.gameItems[i].itemName;
        }
        itemName = item.itemName;
        PhotonNetwork.Instantiate(itemName, itemReturn.position, Quaternion.identity);
    }
    public bool ItemDuplicate(Item _item)
    {
        if (items.Exists(x => x.itemName == _item.itemName))
        {
            return true;
        }
        return false;

    }

    public int PlayerIndex(Item item)
    {
        int index = Array.IndexOf(player.itemID, item.itemID);
        return index;
    }
    public int ItemIndex(Item _item)
    {
        int index = items.FindIndex(x => x == _item);
        return index;
    }
    private int[] RemoveIndex(int[] items, int item)
    {
        int[] newIndicesArray = new int[items.Length - 1];

        int i = 0;
        int j = 0;
        while (i < items.Length)
        {
            if (i != item)
            {
                newIndicesArray[j] = items[i];
                j++;
            }

            i++;
        }

        int[] returnItem = new int[items.Length];

        for (int k = 0; k < newIndicesArray.Length; k++)
        {
            returnItem[k] = newIndicesArray[k];
        }

        return returnItem;
    }
    

    public int GetItemQuantity(int i)
    {
        return player.itemQuantity[i];
    }

  



}
