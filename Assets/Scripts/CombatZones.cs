using UnityEngine;
using System.Collections.Generic;

public class CombatZones : MonoBehaviour
{
    public List<Enemy> encounterTable;

     public bool battleZone = false;

   
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    void UsePool()
    {
        EncounterManager.Instance.encounterPool = encounterTable;
    }

    private void OnTriggerEnter2D(Collider2D collison)
    {
        if (collison.gameObject.CompareTag("Player"))
        {
            UsePool();
            if(battleZone == true)
            {
                EncounterManager.Instance.fightArea = true;
            }
            else
            {
                EncounterManager.Instance.fightArea = false;
            }
        }
    }
}
