using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class StatScreenScript : MonoBehaviour
{
    public TMP_Text currentParty;

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
        screenOn = true;
         foreach (Transform child in buttonParent.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        for(int i = 0; i < PartyManager.Instance.party.Count; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonParent.transform);
            partyMemberButton.Add(newButton);
        }
    }

    public void CloseMenu()
    {
         statScreen.SetActive(false);
            screenOn = false;
    }
}
