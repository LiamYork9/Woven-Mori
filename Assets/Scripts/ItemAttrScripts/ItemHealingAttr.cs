using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemHealingAttr", menuName = "Scriptable Objects/ItemAttr/HealingAttr")]
public class ItemHealingAttr : ItemAttr
{
    public int healAmount;

    public ItemHealingAttr(int amount)
    {
        name = "HealingAttr";

        healAmount = amount;
    }

    public override void ActivateItemAttrOFB(PlayerCharacter target)
    {
        target.currentHP += healAmount;
        PlayerCharacter temp = target;
       
           temp.equipmentStats = new List<int>(){0,0,0,0,0,0};

            
            for(int j = 0; j<temp.equipmentStats.Count; j++)
            {
                
            
                if((target).weapon != null){
                 temp.equipmentStats[j] += temp.weapon.stats[j];
                }
                if((target).armor != null){
                temp.equipmentStats[j] += temp.armor.stats[j];
                }
                if((target ).accessory != null){
                temp.equipmentStats[j] += temp.accessory.stats[j];
                }

            }
            
             if(target.currentHP > target.maxHP + target.equipmentStats[4])
            {
                target.currentHP = target.maxHP + target.equipmentStats[4];
            }
    }

     public override void ActivateItemAttrWIB(UnitBody target)
    {
        target.currentHP += healAmount;

        if (target.currentHP>target.maxHP)
            {
                target.currentHP = target.maxHP;
            }
    }
}
