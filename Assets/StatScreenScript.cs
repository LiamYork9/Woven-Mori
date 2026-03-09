using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;



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

      public List<GameObject> partyMemberButton;

      public List<GameObject> inventoryButtons;

      public GameObject equipmentButtonPrefab;

      public GameObject weaponButton,armorButton,accessoryButton;

      public Sprite defaultSprite;
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
            newButton.GetComponent<Button>().onClick.AddListener(()=> showStats(newButton));
            partyMemberButton.Add(newButton);
            
            
        }
         for(int i = 0; i < partyMemberButton.Count; i++)
        {
            partyMemberButton[i].GetComponentInChildren<TextMeshProUGUI>().text = PartyManager.Instance.party[i].name;
        }
        showStats(partyMemberButton[0]);
    
    }

    public void CloseMenu()
    {
         statScreen.SetActive(false);
         inventoryScreen.SetActive(false);
        screenOn = false;
        weaponButton.SetActive(false);
        armorButton.SetActive(false);
        accessoryButton.SetActive(false);
    }

    public void showStats(GameObject button)
    {
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
            if(partyMemberButton[i] == button)
            {
                statText.text = "" + PartyManager.Instance.party[i].name + " Lv:" + PartyManager.Instance.party[i].level + "\n" +
                "HP: " + PartyManager.Instance.party[i].currentHP + "/" + (PartyManager.Instance.party[i].maxHP + PartyManager.Instance.party[i].equipmentStats[4] ) + " ("+PartyManager.Instance.party[i].maxHP+"+"+PartyManager.Instance.party[i].equipmentStats[4]+")" + "\n" +
                "Attack: " + PartyManager.Instance.party[i].attack + " + " + PartyManager.Instance.party[i].equipmentStats[0] +"\n" +
                "Defense: " + PartyManager.Instance.party[i].defense + " + " + PartyManager.Instance.party[i].equipmentStats[1] + "\n" +
                "Magic Defense: " + PartyManager.Instance.party[i].mDefense + " + " + PartyManager.Instance.party[i].equipmentStats[2] + "\n" +
                "Speed: " + PartyManager.Instance.party[i].speed + " + " + PartyManager.Instance.party[i].equipmentStats[3] + "\n" +
                "AP Gain: " + PartyManager.Instance.party[i].APGain + " + " + PartyManager.Instance.party[i].equipmentStats[5];

                if(PartyManager.Instance.party[i].weapon != null)
                {
                    statText.text +=  "\nWeapon: " + PartyManager.Instance.party[i].weapon.name;
                    weaponButton.GetComponentInChildren<TextMeshProUGUI>().text =  PartyManager.Instance.party[i].weapon.name;
                    weaponButton.GetComponentInChildren<Image>().sprite = PartyManager.Instance.party[i].weapon.itemSprite;
                }
                else
                {
                     statText.text +=  "\nWeapon: None";
                     weaponButton.GetComponentInChildren<Image>().sprite = defaultSprite;
                }
                if(PartyManager.Instance.party[i].armor != null)
                {
                    statText.text +=  "\nArmor: " + PartyManager.Instance.party[i].armor.name;
                    armorButton.GetComponentInChildren<TextMeshProUGUI>().text =  PartyManager.Instance.party[i].armor.name;
                    armorButton.GetComponentInChildren<Image>().sprite = PartyManager.Instance.party[i].armor.itemSprite;
                }
                else
                {
                    statText.text +=  "\nArmor: None";
                    armorButton.GetComponentInChildren<Image>().sprite = defaultSprite;
                }
                if(PartyManager.Instance.party[i].accessory != null)
                {
                    statText.text +=  "\nAccessory: " + PartyManager.Instance.party[i].accessory.name;
                    accessoryButton.GetComponentInChildren<TextMeshProUGUI>().text =  PartyManager.Instance.party[i].accessory.name;
                    accessoryButton.GetComponentInChildren<Image>().sprite = PartyManager.Instance.party[i].accessory.itemSprite;
                }
                else
                {
                    statText.text +=  "\nAccessory: None";
                    accessoryButton.GetComponentInChildren<Image>().sprite = defaultSprite;
                }
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
}
