using System.Collections.Generic;
using UnityEngine;
using System;

public enum GrowthTarget
{
    All,
    Next,
    Last
}

[Flags]
public enum StatTarget
{
    MaxHP = 1<<0,
    Attack = 1<<1,
    Defense = 1<<2,
    MDefense = 1<<3,
    Speed = 1<<4,
    All =~0,
}

public class LevelUpManager : MonoBehaviour
{
     public List <ClassGrowth> classGrowths;
     public static LevelUpManager Instance;
    public static LevelUpManager GetInstance()
    {
        return Instance;
    }
     public void Awake()
    {
        if (LevelUpManager.Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);

    }

   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
