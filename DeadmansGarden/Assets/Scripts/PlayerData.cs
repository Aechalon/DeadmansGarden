
[System.Serializable]
public class PlayerData
{
    public int health;
    public int hunger;
    public int scraps;
    public int storageLength;
    public float[] pos;
    public int[] itemID, itemQuantity;
    public int bagSpace;
    public int handSpace;
    public int[] handItem;
    public bool hasFlashlight;
    public bool hasRaster;
    public PlayerData(Player player)
    {
        scraps = player.scraps;
        hunger = player.hunger;
        health = player.health;
        bagSpace = player.bagSpace;
        handSpace = player.handSpace;
        handItem = new int[2];
        itemID = new int[20];
        itemQuantity = new int[20];
   
  
       

        for (int i= 0; i <bagSpace; i++)
        {
            itemID[i] = player.itemID[i];
        }

        for (int i = 0; i < itemQuantity.Length; i++)
        {
            itemQuantity[i] = player.itemQuantity[i];
        }
    


        for(int i = 0; i < handSpace; i++ )
        {
            handItem[i] = player.handItem[i];
        }

        pos = new float[3];
        pos[0] = player.transform.position.x;
        pos[1] = player.transform.position.y;
        pos[2] = player.transform.position.z;
        
       
      
    }

}
