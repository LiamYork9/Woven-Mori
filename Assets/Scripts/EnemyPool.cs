using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Rendering.Universal.Internal;

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
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);

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
