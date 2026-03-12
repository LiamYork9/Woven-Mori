using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class InventoryManager : MonoBehaviour
{
    
    public  SerializedDictionary<Item,int> inventoryStandard = new  SerializedDictionary<Item, int>();
    public SerializedDictionary<Equipment,int> inventoryEquipment = new SerializedDictionary<Equipment, int>();
    public  SerializedDictionary<KeyItem,int> inventoryKeyitems = new  SerializedDictionary<KeyItem, int>();

   

    public static InventoryManager Instance;
    public static InventoryManager GetInstance()
    {
        return Instance;
    }
     public void Awake()
    {
        if (InventoryManager.Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);

    }

    public void AddItem(Item item, int amount = 1)
    {
        

        if (inventoryStandard.ContainsKey(item))
        {
            if(item.stackable == true)
            {
                inventoryStandard[item] += amount;
            }
            if(inventoryStandard[item] > item.maxStack)
            {
                inventoryStandard[item] = item.maxStack;
            }
        }
        else
        {
            inventoryStandard.Add(item,amount);
            if(inventoryStandard[item] > item.maxStack)
            {
                inventoryStandard[item] = item.maxStack;
            }
        }
    }

     public void AddEquipment(Equipment equipment, int amount = 1)
    {
        

        if (inventoryEquipment.ContainsKey(equipment))
        {
            
            if(equipment.stackable == true)
            {
                equipment.availableNumber += amount;
                inventoryEquipment[equipment] += amount;
               
            }
            if(inventoryEquipment[equipment] > equipment.maxStack)
            {
                equipment.availableNumber -= inventoryEquipment[equipment] -equipment.maxStack;
                inventoryEquipment[equipment] = equipment.maxStack;
            }
        }
        else
        {
            equipment.availableNumber = amount;
            inventoryEquipment.Add(equipment,amount);
            if(inventoryEquipment[equipment] > equipment.maxStack)
            {
                equipment.availableNumber -= inventoryEquipment[equipment] -equipment.maxStack;
                inventoryEquipment[equipment] = equipment.maxStack;
            }
        }
    }
     public void AddKeyItem(KeyItem item, int amount = 1)
    {
        

        if (inventoryKeyitems.ContainsKey(item))
        {
            if(item.stackable == true)
            {
                inventoryKeyitems[item] += amount;
            }
            if(inventoryKeyitems[item] > item.maxStack)
            {
                inventoryKeyitems[item] = item.maxStack;
            }
        }
        else
        {
            inventoryKeyitems.Add(item,amount);
            if(inventoryKeyitems[item] > item.maxStack)
            {
                inventoryKeyitems[item] = item.maxStack;
            }
        }
    }

    public void PickUp(Item item, int amount = 1)
    {
        if(item is Equipment)
        {
            AddEquipment(item as Equipment, amount);
        }
        else if( item is KeyItem)
        {
            AddKeyItem(item as KeyItem, amount);
        }
        else
        {
            AddItem(item,amount);
        }
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Standard:");
            foreach(KeyValuePair<Item,int> pair in inventoryStandard)
            {
                Debug.Log(pair.Key.itemName + ": "+pair.Value);
            }
            Debug.Log("Equipment:");
             foreach(KeyValuePair<Equipment,int> pair in inventoryEquipment)
            {
                Debug.Log(pair.Key.itemName + ": "+pair.Value);
            }
            Debug.Log("KeyItem:");
            foreach(KeyValuePair<KeyItem,int> pair in inventoryKeyitems)
            {
                Debug.Log(pair.Key.itemName + ": "+pair.Value);
            }
            
        }
    }
}

