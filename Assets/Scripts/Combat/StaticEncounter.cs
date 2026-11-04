using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class StaticEncounter : MonoBehaviour
{
    public List<Enemy> staticEnemy;

    public string encounterID;

    public DialogueInteraction currentlog;

    public Dialogue postBattle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(PartyManager.Instance.staticEncounter == true && PartyManager.Instance.encounterID == encounterID)
        {
         StartCoroutine(LateStart(1));
        }
        
    }
     IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        currentlog.text = postBattle;
        currentlog.StartDialogue();
        PartyManager.Instance.staticEncounter = false;
        PartyManager.Instance.encounterID = "";
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void StartStaticEncounter()
    {
        PartyManager.Instance.staticEncounter = true;
        PartyManager.Instance.encounterID = encounterID;

        EncounterManager.Instance.encounteredEnemies = staticEnemy;
         EncounterManager.Instance.startEncounter.Invoke();
       EncounterManager.Instance.StartEncounter();
    }

    public void StartStaticTutorial()
    {
        PartyManager.Instance.staticEncounter = true;
        PartyManager.Instance.encounterID = encounterID;
        EncounterManager.Instance.encounteredEnemies = staticEnemy;
         EncounterManager.Instance.startEncounter.Invoke();
         EncounterManager.Instance.StartTutorial();
    }

    public void BossDeath()
    {
        //PartyManager.Instance.staticEncounter = false;
        Destroy(gameObject);
    }
}
