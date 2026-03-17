using MoriSkills;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Consumable",  menuName = "ScriptableObjects/Item/Consumable")]
public class ConsumableItem : Item
{
    public Target itemTarget;

     public List<ItemAttr> itemAttrs;
    
    public bool usableOFB;
}
