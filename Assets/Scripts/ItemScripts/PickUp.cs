using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public List<Item> item;

    public List <int> amount;
   
   
   public void PickUpItem()
    {
        for(int i = 0; i < item.Count; i++)
        {
            InventoryManager.Instance.PickUp(item[i],amount[i]);
        }
        
    }

}
