using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class StaticEncounter : MonoBehaviour
{
    public List<Enemy> staticEnemy;

    public DialogueInteraction currentlog;

    public Dialogue postBattle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(PartyManager.Instance.staticEncounter == true)
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
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void StartStaticEncounter()
    {
        PartyManager.Instance.staticEncounter = true;
        EncounterManager.Instance.encounteredEnemies = staticEnemy;
         EncounterManager.Instance.startEncounter.Invoke();
       EncounterManager.Instance.StartEncounter();
    }

    public void StartStaticTutorial()
    {
        PartyManager.Instance.staticEncounter = true;
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
