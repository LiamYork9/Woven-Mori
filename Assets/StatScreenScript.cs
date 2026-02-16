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
        
        currentParty.text = "Current Party: " + PartyManager.Instance.party[0].name;
    }

    public void OpenMenu()
    {
         statScreen.SetActive(true);
        screenOn = true;
        for(int i = 0; i < PartyManager.Instance.party.Count; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonParent.transform);
        }
    }

    public void CloseMenu()
    {
         statScreen.SetActive(false);
            screenOn = false;
    }
}
