using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using MoriSkills;


public class ShopScript : MonoBehaviour
{
    public GameObject shopScreen;

    public List<Item> shopInventory;

    public List<Item> playerOrder;

    public GameObject shopButtonParent;

    public GameObject itemButtonPrefab,upButton,downButton,canelButton,buyButton;

    public TMP_Text shopText,buyAmountText;

    public GameObject selectedItemImage;

    public int buyAmount;

    public int price;

    public TMP_Text totalCost;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            shopScreen.SetActive(false);
        }
        
    }

    public void OpenShop()
    {
         foreach (Transform child in shopButtonParent.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        shopScreen.SetActive(true);
        for(int i = 0; i<shopInventory.Count; i++)
        {
            GameObject newButton = Instantiate(itemButtonPrefab, shopButtonParent.transform);
            newButton.GetComponent<ItemToolTipScript>().item = shopInventory[i];
            newButton.GetComponentInChildren<Image>().sprite = shopInventory[i].itemSprite;
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = shopInventory[i].itemName + " " + shopInventory[i].value+"G" ;
             newButton.GetComponent<Button>().onClick.AddListener(()=> AddToOrder(newButton.GetComponent<ItemToolTipScript>().item));
            
        }
    }

    public void AddToOrder(Item item)
    {
        buyAmount = 0;
        buyAmountText.text = "";
        totalCost.text = "";
        Debug.Log(buyAmount);
        upButton.SetActive(true);
        downButton.SetActive(true);
        canelButton.SetActive(true);
         buyButton.SetActive(true);
        shopText.text = "How many do you want?";
        selectedItemImage.SetActive(true);
        selectedItemImage.GetComponent<Image>().sprite = item.itemSprite;
        selectedItemImage.GetComponent<ItemToolTipScript>().item = item;
         AddAmount();
    }

    public void AddAmount()
    {
        Item temp =  selectedItemImage.GetComponent<ItemToolTipScript>().item;
        int max = FindMax(temp);
        buyAmount += 1;
        if(buyAmount > max && max != 0)
        {
            buyAmount = 1;
        }
        else if(buyAmount > max)
        {
            buyAmount = 0;
        }
      
        
        buyAmountText.text = "X " + buyAmount;
        totalCost.text =  temp.value * buyAmount + "G";
    }

    public void SubAmount()
    {
        Item temp =  selectedItemImage.GetComponent<ItemToolTipScript>().item;
        int max = FindMax(temp);
        buyAmount -=1;
        if(buyAmount < 0)
        {
            buyAmount = max;
        }
        buyAmountText.text = "X " + buyAmount;
         totalCost.text =  temp.value * buyAmount + "G";
    }

    public int FindMax(Item item)
    {
        int max = 0;
        if(item is KeyItem && InventoryManager.Instance.inventoryKeyitems.ContainsKey(item as KeyItem))
        {
            max = item.maxStack - InventoryManager.Instance.inventoryKeyitems[item as KeyItem];
        }
        else if(item is Equipment && InventoryManager.Instance.inventoryEquipment.ContainsKey(item as Equipment))
        {
             max = item.maxStack - InventoryManager.Instance.inventoryEquipment[item as Equipment];
        }
        else if( InventoryManager.Instance.inventoryStandard.ContainsKey(item))
        {
             max = item.maxStack - InventoryManager.Instance.inventoryStandard[item];
        }
        else
        {
            max = item.maxStack;
        }

        if(max * item.value > PartyManager.Instance.funds)
        {
            max = PartyManager.Instance.funds / item.value;
        }

        if(max < 0)
        {
            max = 0;
        }

        Debug.Log(max);


        return max;
    }

    public void Cancel()
    {
        buyAmount = 0;
        upButton.SetActive(false);
        downButton.SetActive(false);
        shopText.text = "";
        selectedItemImage.SetActive(false);
        buyAmountText.text = "";
        canelButton.SetActive(false);
        buyButton.SetActive(false);
        totalCost.text = "";

    }

    public void Buy()
    {
        if(selectedItemImage.GetComponent<ItemToolTipScript>().item != null && buyAmount > 0)
        {
             PartyManager.Instance.funds -= buyAmount * selectedItemImage.GetComponent<ItemToolTipScript>().item.value;
        
            InventoryManager.Instance.PickUp(selectedItemImage.GetComponent<ItemToolTipScript>().item,buyAmount);
        }
        Cancel();
       
        
    }
}
