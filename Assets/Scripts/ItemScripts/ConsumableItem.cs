using MoriSkills;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Consumable",  menuName = "ScriptableObjects/Item/Consumable")]
public class ConsumableItem : Item
{
    public Target itemTarget;

     public List<ItemAttr> itemAttrs;
    
    public bool usableOFB;

    public void ApplyItemAttrOFB(PlayerCharacter character)
    {
        foreach(ItemAttr attr in itemAttrs)
        {
            attr.ActivateItemAttrOFB(character);
        }
        InventoryManager.Instance.inventoryStandard[this] -= 1;
    }
    public void ApplyItemAttrOFB(List<PlayerCharacter> characters)
    {
        foreach(PlayerCharacter character in characters)
        {
            foreach(ItemAttr attr in itemAttrs)
            {
                attr.ActivateItemAttrOFB(character);
            }
        }
        InventoryManager.Instance.inventoryStandard[this] -= 1;
    }

     public void ApplyItemAttrWIB(UnitBody target)
    {
        foreach(ItemAttr attr in itemAttrs)
        {
            attr.ActivateItemAttrWIB(target);
        }
        InventoryManager.Instance.inventoryStandard[this] -= 1;
    }
}
