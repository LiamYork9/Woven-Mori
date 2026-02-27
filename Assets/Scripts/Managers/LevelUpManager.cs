using System.Collections.Generic;
using UnityEngine;

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
