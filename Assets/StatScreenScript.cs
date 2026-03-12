using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.InputSystem;



public class StatScreenScript : MonoBehaviour
{
    public TMP_Text currentParty;

    public TMP_Text statText;

    public TMP_Text inventoryText;

    public GameObject statScreen;

    public GameObject inventoryScreen;

    public bool screenOn = false;

     public GameObject buttonPrefab;

      public GameObject buttonParent;

      public GameObject buttonParentInv;

      public GameObject equipMenuParent;

      public List<GameObject> partyMemberButton;

      public List<GameObject> inventoryButtons;

      public GameObject equipmentButtonPrefab;


      public GameObject weaponButton,armorButton,accessoryButton,unequipButton,equipMenu;

      public Sprite defaultSprite;

      public PlayerCharacter selectedCharacter;

      public int remove = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         for(int i = 0; i<inventoryButtons.Count; i++)
        {
            if(inventoryButtons[i].GetComponent<EquipmentToolTip>() != null)
            {
                inventoryButtons[i].GetComponent<EquipmentToolTip>().hoverEvent.AddListener(ToolTipAdder);
                inventoryButtons[i].GetComponent<EquipmentToolTip>().unHoverEvent.AddListener(ToolTipRemover);
            }
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           if(screenOn == false)
            {
                OpenMenu();
            }
            else
            {
                CloseMenu();
            }
        }
        currentParty.text = "Current Party: ";
        for(int i = 0; i<PartyManager.Instance.party.Count; i++)
        {
            currentParty.text += PartyManager.Instance.party[i].name + ", ";
        }

        
        
        
       
    }

     
    
       

    public void OpenMenu()
    {
        weaponButton.SetActive(false);
        armorButton.SetActive(false);
        accessoryButton.SetActive(false);
         statScreen.SetActive(true);
         statText.text = "";
        screenOn = true;
        partyMemberButton.Clear();
         foreach (Transform child in buttonParent.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        for(int i = 0; i < PartyManager.Instance.party.Count; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonParent.transform);
            newButton.GetComponent<InventoryCharacter>().character = PartyManager.Instance.party[i];
            newButton.GetComponent<Button>().onClick.AddListener(()=> showStats(newButton.GetComponent<InventoryCharacter>().character));
            partyMemberButton.Add(newButton);
            
            
        }
         for(int i = 0; i < partyMemberButton.Count; i++)
        {
            partyMemberButton[i].GetComponentInChildren<TextMeshProUGUI>().text = PartyManager.Instance.party[i].name;
        }
        showStats(partyMemberButton[0].GetComponent<InventoryCharacter>().character);
    
    }

    public void CloseMenu()
    {
         statScreen.SetActive(false);
         inventoryScreen.SetActive(false);
        screenOn = false;
        weaponButton.SetActive(false);
        armorButton.SetActive(false);
        accessoryButton.SetActive(false);
        equipMenu.SetActive(false);
        unequipButton.SetActive(false);
    }

    public void showStats(PlayerCharacter character)
    {
         unequipButton.SetActive(false);
        for (int i = 0; i < PartyManager.Instance.party.Count; i++)
        {
           PlayerCharacter temp = PartyManager.Instance.party[i];
       
           temp.equipmentStats = new List<int>(){0,0,0,0,0,0};

            
            for(int j = 0; j<temp.equipmentStats.Count; j++)
            {
                
            
                if((PartyManager.Instance.party[i]).weapon != null){
                 temp.equipmentStats[j] += temp.weapon.stats[j];
                }
                if((PartyManager.Instance.party[i]).armor != null){
                temp.equipmentStats[j] += temp.armor.stats[j];
                }
                if((PartyManager.Instance.party[i] ).accessory != null){
                temp.equipmentStats[j] += temp.accessory.stats[j];
                }

            }
            
             if(PartyManager.Instance.party[i].currentHP > PartyManager.Instance.party[i].maxHP)
            {
                PartyManager.Instance.party[i].currentHP = PartyManager.Instance.party[i].maxHP;
            }
            

            
        }
        
            weaponButton.SetActive(true);
            armorButton.SetActive(true);
            accessoryButton.SetActive(true);
      for(int i = 0; i < partyMemberButton.Count; i++)
        {
            selectedCharacter = character;
            statText.text = "" + selectedCharacter.name + " Lv:" + selectedCharacter.level + "\n" +
            "HP: " + selectedCharacter.currentHP + "/" + (selectedCharacter.maxHP + selectedCharacter.equipmentStats[4] ) + " ("+selectedCharacter.maxHP+"+"+selectedCharacter.equipmentStats[4]+")" + "\n" +
            "Attack: " + selectedCharacter.attack + " + " + selectedCharacter.equipmentStats[0] +"\n" +
            "Defense: " + selectedCharacter.defense + " + " + selectedCharacter.equipmentStats[1] + "\n" +
            "Magic Defense: " + selectedCharacter.mDefense + " + " + selectedCharacter.equipmentStats[2] + "\n" +
            "Speed: " + selectedCharacter.speed + " + " + selectedCharacter.equipmentStats[3] + "\n" +
            "AP Gain: " + selectedCharacter.APGain + " + " + selectedCharacter.equipmentStats[5];

            if(selectedCharacter.weapon != null)
            {
                statText.text +=  "\nWeapon: " + selectedCharacter.weapon.name;
                weaponButton.GetComponentInChildren<TextMeshProUGUI>().text =  selectedCharacter.weapon.itemName;
                weaponButton.GetComponentInChildren<Image>().sprite = selectedCharacter.weapon.itemSprite;
            }
            else
            {
                    statText.text +=  "\nWeapon: None";
                    weaponButton.GetComponentInChildren<TextMeshProUGUI>().text =  "None";
                    weaponButton.GetComponentInChildren<Image>().sprite = defaultSprite;
            }
            if(selectedCharacter.armor != null)
            {
                statText.text +=  "\nArmor: " + selectedCharacter.armor.name;
                armorButton.GetComponentInChildren<TextMeshProUGUI>().text =  selectedCharacter.armor.itemName;
                armorButton.GetComponentInChildren<Image>().sprite = selectedCharacter.armor.itemSprite;
            }
            else
            {
                statText.text +=  "\nArmor: None";
                armorButton.GetComponentInChildren<TextMeshProUGUI>().text =  "None";
                armorButton.GetComponentInChildren<Image>().sprite = defaultSprite;
            }
            if(selectedCharacter.accessory != null)
            {
                statText.text +=  "\nAccessory: " + selectedCharacter.accessory.name;
                accessoryButton.GetComponentInChildren<TextMeshProUGUI>().text =  selectedCharacter.accessory.itemName;
                accessoryButton.GetComponentInChildren<Image>().sprite = selectedCharacter.accessory.itemSprite;
            }
            else
            {
                statText.text +=  "\nAccessory: None";
                accessoryButton.GetComponentInChildren<TextMeshProUGUI>().text =  "None";
                accessoryButton.GetComponentInChildren<Image>().sprite = defaultSprite;
            }
        }       
    }

     public void ToolTipAdder(GameObject button)
    {
        Equipment temp = button.GetComponent<EquipmentToolTip>().equipment;
        inventoryText.text = temp.itemDescription;
    }
      public void ToolTipRemover()
    {
        inventoryText.text = "";
    }

   

    public void ShowInventory()
    {
        statScreen.SetActive(false);
        inventoryScreen.SetActive(true);

        inventoryButtons.Clear();

        foreach (Transform child in buttonParentInv.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }

         
        

            foreach(KeyValuePair<Equipment, int> pair in InventoryManager.Instance.inventoryEquipment)
            {
                
                
                GameObject newButton = Instantiate(equipmentButtonPrefab, buttonParentInv.transform);
                inventoryButtons.Add(newButton);
                 
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = pair.Key.itemName +" X"+ pair.Value;
                newButton.GetComponentInChildren<Image>().sprite = pair.Key.itemSprite;
                newButton.GetComponent<EquipmentToolTip>().equipment = pair.Key ;
             
                
                
            }
        

         for(int i = 0; i<inventoryButtons.Count; i++)
        {
            if(inventoryButtons[i].GetComponent<EquipmentToolTip>() != null)
            {
                inventoryButtons[i].GetComponent<EquipmentToolTip>().hoverEvent.AddListener(ToolTipAdder);
                inventoryButtons[i].GetComponent<EquipmentToolTip>().unHoverEvent.AddListener(ToolTipRemover);
            }
             
            
        }
        
    }

    public void EquipMenuWeapon()
    {
        equipMenu.SetActive(true);
        remove = 2;
        if(selectedCharacter.weapon == null)
        {
             unequipButton.SetActive(false);
        }
        else
        {
             unequipButton.SetActive(true);
        }
       
        foreach (Transform child in equipMenuParent.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach(KeyValuePair<Equipment, int> pair in InventoryManager.Instance.inventoryEquipment)
        {
            if(pair.Key is Weapon && pair.Key.availableNumber > 0)
            {
                GameObject newButton = Instantiate(equipmentButtonPrefab, equipMenuParent.transform);
                inventoryButtons.Add(newButton);
                newButton.GetComponent<Button>().onClick.AddListener(()=> EquipWeapon(newButton));
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = pair.Key.itemName +" X"+ pair.Key.availableNumber;
                newButton.GetComponentInChildren<Image>().sprite = pair.Key.itemSprite;
                newButton.GetComponent<EquipmentToolTip>().equipment = pair.Key ;
                newButton.GetComponent<Button>().onClick.AddListener(()=>showStats(selectedCharacter));
                
            }
        }
    }
     public void EquipMenuArmor()
    {
        
        equipMenu.SetActive(true);
         remove = 1;
       if(selectedCharacter.armor == null)
        {
             unequipButton.SetActive(false);
        }
        else
        {
             unequipButton.SetActive(true);
        }
        foreach (Transform child in equipMenuParent.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach(KeyValuePair<Equipment, int> pair in InventoryManager.Instance.inventoryEquipment)
        {
            if(pair.Key is Armor && pair.Key.availableNumber > 0)
            {
                GameObject newButton = Instantiate(equipmentButtonPrefab, equipMenuParent.transform);
                inventoryButtons.Add(newButton);
                newButton.GetComponent<Button>().onClick.AddListener(()=> EquipArmor(newButton));
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = pair.Key.itemName +" X"+ pair.Key.availableNumber;
                newButton.GetComponentInChildren<Image>().sprite = pair.Key.itemSprite;
                newButton.GetComponent<EquipmentToolTip>().equipment = pair.Key ;
                newButton.GetComponent<Button>().onClick.AddListener(()=>showStats(selectedCharacter));
                
            }
        }
    }
     public void EquipMenuAccessory()
    {
        unequipButton.SetActive(true);
         remove = 3;
       if(selectedCharacter.accessory == null)
        {
             unequipButton.SetActive(false);
        }
        else
        {
             unequipButton.SetActive(true);
        }
        foreach (Transform child in equipMenuParent.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach(KeyValuePair<Equipment, int> pair in InventoryManager.Instance.inventoryEquipment)
        {
            if(pair.Key is Accessory && pair.Key.availableNumber > 0)
            {
                GameObject newButton = Instantiate(equipmentButtonPrefab, equipMenuParent.transform);
                inventoryButtons.Add(newButton);
                newButton.GetComponent<Button>().onClick.AddListener(()=> EquipAccessory(newButton));
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = pair.Key.itemName +" X"+ pair.Key.availableNumber;
                newButton.GetComponentInChildren<Image>().sprite = pair.Key.itemSprite;
                newButton.GetComponent<EquipmentToolTip>().equipment = pair.Key ;
                newButton.GetComponent<Button>().onClick.AddListener(()=>showStats(selectedCharacter));
            }
        }
    }
    public void EquipArmor(GameObject button)
    {
        unequipButton.SetActive(false);
    
         foreach(KeyValuePair<Equipment, int> pair in InventoryManager.Instance.inventoryEquipment)
        {
            if(pair.Key is Armor && pair.Key.availableNumber > 0)
            {
                if(selectedCharacter.armor != null)
                {
                    selectedCharacter.armor.availableNumber += 1;
                }
                selectedCharacter.armor = button.GetComponent<EquipmentToolTip>().equipment as Armor;
                button.GetComponent<EquipmentToolTip>().equipment.availableNumber -= 1;
                EquipMenuArmor();
            }
        }
    }
     public void EquipWeapon(GameObject button)
    {
        unequipButton.SetActive(false);
         foreach(KeyValuePair<Equipment, int> pair in InventoryManager.Instance.inventoryEquipment)
        {
            if(pair.Key is Weapon && pair.Key.availableNumber > 0)
            {
                 if(selectedCharacter.weapon != null)
                {
                    selectedCharacter.weapon.availableNumber += 1;
                }
               selectedCharacter.weapon = button.GetComponent<EquipmentToolTip>().equipment as Weapon;
               button.GetComponent<EquipmentToolTip>().equipment.availableNumber -= 1;
                EquipMenuWeapon();
            }
        }
    }
     public void EquipAccessory(GameObject button )
    {
       
        unequipButton.SetActive(false);
         foreach(KeyValuePair<Equipment, int> pair in InventoryManager.Instance.inventoryEquipment)
        {
            if(pair.Key is Accessory && pair.Key.availableNumber > 0)
            {
                 if(selectedCharacter.accessory != null)
                {
                    selectedCharacter.accessory.availableNumber += 1;
                }
                selectedCharacter.accessory = button.GetComponent<EquipmentToolTip>().equipment as Accessory;
                button.GetComponent<EquipmentToolTip>().equipment.availableNumber -= 1;
                EquipMenuAccessory();
            }
        }
    }

    public void UnEquip()
    {
        if(remove == 1)
        {
            selectedCharacter.armor.availableNumber += 1;
             selectedCharacter.armor = null;
             EquipMenuArmor();
        }
        if(remove == 2)
        {
            selectedCharacter.weapon.availableNumber += 1;
             selectedCharacter.weapon = null;
             EquipMenuWeapon();
        }
        if(remove == 3)
        {
            selectedCharacter.accessory.availableNumber += 1;
             selectedCharacter.accessory = null;
             EquipMenuAccessory();
        }
        showStats(selectedCharacter);
       
    }
}
