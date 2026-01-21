using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EncounterManager : MonoBehaviour
{
    public List<Enemy> encounterPool;
    
    public List<Enemy> encounteredEnemies;

    public static EncounterManager Instance;

    
    public int battleCheck = 1;

    public int encounterMod;

    public int min = 1;

    public int max = 256;
    public int maxEnemies = 3;

    public int stepsSinceEncounter = 0;
    public string sceneName = "LiamYorkTestScene";
    public DialogueInteraction fightText;

    public bool fightArea = false;

    public static EncounterManager GetInstance()
    {
        return Instance;
    }

    public void Awake()
    {
        if (EncounterManager.Instance != this && EncounterManager.Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
        
        

    }
    
    void Start()
    {
        if (fightText != null)
        {
            fightText.text.EndDialogueEvent.AddListener(StartEncounter);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EncounterCheck()
    {
        stepsSinceEncounter += 1;
        battleCheck = Random.Range(min, max);
        if (battleCheck <= stepsSinceEncounter + encounterMod)
        {
            stepsSinceEncounter = 0;
            AnounceEncounter();
        }
    }

    public void AnounceEncounter()
    {

        Debug.Log("FIGHT!");
        RollEncounter();
        if (fightText != null)
        {

            fightText.text.lines[2] = "";
            for (int i = 0; i < encounteredEnemies.Count; i++)
            {
                if (i + 1 == encounteredEnemies.Count && encounteredEnemies.Count != 1)
                {
                    fightText.text.lines[2] += "and ";
                }
                fightText.text.lines[2] += encounteredEnemies[i].unitName;
                if (i + 1 < encounteredEnemies.Count)
                {
                    fightText.text.lines[2] += ", ";
                }
                else
                {
                    fightText.text.lines[2] += "!!!";
                }
            }
            fightText.StartDialogue();
        }
    }
    
    public void StartEncounter()
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void RollEncounter(int count = -1)
    {
        encounteredEnemies.Clear();
        if (count == -1)
        {
            count = Random.Range(1, maxEnemies+1);
        }
        
        for (int i = 0; i<count; i++)
        {
            encounteredEnemies.Add(RollEnemy());
        }
    }

    public Enemy RollEnemy()
    {
        return encounterPool[Random.Range(0, encounterPool.Count)];

    }
    
    
}
