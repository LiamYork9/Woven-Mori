using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
    {
        Weapon,
        Armor,
        Accessory,
        Scroll
    }

[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObjects/Item/Equipment")]


public class Equipment : Item
{
    public EquipmentType equipmentType;
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
