using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;

public class EncounterManager : MonoBehaviour
{
    public List<Enemy> encounterPool;

    public static EncounterManager Instance;

    
    public int battleCheck = 1;

    public int encounterMod;

    public int min = 1;

    public int max = 256;

    public int stepsSinceEncounter = 0;

    public PlayerController player;
    public DialogueInteraction FightText;

    public static EncounterManager GetInstance()
    {
        return Instance;
    }

    public void Awake()
    {
        if (EncounterManager.Instance != this && EncounterManager.Instance != null)
        {
            Destroy(EncounterManager.Instance);
            Instance = this;
        }
        else
        {
            Instance = this;
        }
        
        

    }
    
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EncounterCheck()
    {
        stepsSinceEncounter += 1;
        battleCheck = UnityEngine.Random.Range(min, max);
        if (battleCheck <= stepsSinceEncounter + encounterMod)
        {
            stepsSinceEncounter = 0;
            StartEncounter();
        }
    }
    
    public void StartEncounter()
    {
        Debug.Log("FIGHT!");
    }
    
    
}
