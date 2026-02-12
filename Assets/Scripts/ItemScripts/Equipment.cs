using System.Collections.Generic;
using UnityEngine;
using System;

[Flags]
public enum Classes
    {
        Protag=1<<0,
        Barbarian=1<<1,
        Cleric=1<<2,
        Rogue=1<<3,
        All=~0
    }

[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObjects/Item/Equipment")]


public class Equipment : Item
{
    public Classes classList;
    public bool equiped;

    public Unit user;

    public List<int> stats = new List<int> {0,0,0,0,0,0};

    public void OnEquip()
    {
        
    }

    public void OnUnequip()
    {
        
    }
}
