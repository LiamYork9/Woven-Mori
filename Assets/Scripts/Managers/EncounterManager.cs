using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EncounterManager : MonoBehaviour
{
     public List<Enemy> encounterPool;

     public static EncounterManager Instance;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
