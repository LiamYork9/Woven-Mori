using UnityEngine;
using System.Collections.Generic;

public class MerchentStockScript : MonoBehaviour
{
     public List<Item> merchentList;

     public ShopScript shop;


     public void switchInvo()
    {
        shop.shopInventory = merchentList;
        shop.OpenShop();
    }
  
}
