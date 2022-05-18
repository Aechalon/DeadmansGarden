using UnityEngine;
/* The base item class. All items should derive from this. */

[CreateAssetMenu(fileName = "New Item" , menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public Sprite icon = null;              // Item icon

    public int itemID;
    
    public string itemName = "New Item";    // Name of the item
    public string itemDesc;
    public int stack;
    public bool isThrowable;
    public bool isPrimaryItem;      // Is the item default wear?

    // Called when the item is pressed in the inventory
    public virtual void Use()
    {
        // Use the item
        // Something might happen

        Debug.Log("Using " + name);
    }

    public void RemoveFromInventory()
    {
        InventorySystem.instance.Remove(this, true);
    }

}