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
        SetEquipmentButton();
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
    }

    public void CloseMenu()
    {
         statScreen.SetActive(false);
         inventoryScreen.SetActive(false);
        screenOn = false;
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
                }
                else
                {
                     statText.text +=  "\nWeapon: None";
                }
                if(PartyManager.Instance.party[i].armor != null)
                {
                    statText.text +=  "\nArmor: " + PartyManager.Instance.party[i].armor.name;
                }
                else
                {
                     statText.text +=  "\nArmor: None";
                }
                if(PartyManager.Instance.party[i].accessory != null)
                {
                    statText.text +=  "\nAccessory: " + PartyManager.Instance.party[i].accessory.name;
                }
                else
                {
                     statText.text +=  "\nAccessory: None";
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

    public void SetEquipmentButton()
    {
         for(int i = 0; i<inventoryButtons.Count; i++)
        {
            foreach(KeyValuePair<Equipment, int> pair in InventoryManager.Instance.inventoryEquipment)
            {
                inventoryButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = pair.Key.itemName;
                inventoryButtons[i].GetComponent<EquipmentToolTip>().equipment = pair.Key ;
            }
        }
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
        }

         for(int i = 0; i<inventoryButtons.Count; i++)
        {
            if(inventoryButtons[i].GetComponent<EquipmentToolTip>() != null)
            {
                inventoryButtons[i].GetComponent<EquipmentToolTip>().hoverEvent.AddListener(ToolTipAdder);
                inventoryButtons[i].GetComponent<EquipmentToolTip>().unHoverEvent.AddListener(ToolTipRemover);
            }
            
        }
        SetEquipmentButton();
    }
}
