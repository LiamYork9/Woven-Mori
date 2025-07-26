using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyPool : MonoBehaviour
{
     public List<Enemy> encounterTable;

     public static EnemyPool Instance;

    public static EnemyPool GetInstance()
    {
        return Instance;
    }

    public void Awake()
    {
        if (EnemyPool.Instance != this && EnemyPool.Instance != null)
        {
            Destroy(EnemyPool.Instance);
            Instance = this;
        }
        else
        {
            Instance = this;
        }

    }

    void UsePool()
    {
        EncounterManager.Instance.encounterPool = encounterTable;
    }
    
    void Start()
    {
        UsePool();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
