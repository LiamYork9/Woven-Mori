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

    public GameObject statScreen;

    public bool screenOn = false;

     public GameObject buttonPrefab;

      public GameObject buttonParent;

      public List<GameObject> partyMemberButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
}
