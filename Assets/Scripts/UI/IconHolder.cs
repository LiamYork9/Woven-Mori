using UnityEngine;
using System;
using System.Collections.Generic;


[System.Serializable] 
public struct Holder
{
    public string key;

    public Sprite value;

    public Holder(string name, Sprite icon)
    {
        key = name;
        value = icon;
    }
}



public class IconHolder : MonoBehaviour
{

    public List<Holder> holders = new List<Holder>();

    public static IconHolder Instance;

    
   
     public void Awake()
    {
        if (IconHolder.Instance != this && IconHolder.Instance != null)
        {
            Destroy(BattleManager.Instance);
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
