using UnityEngine;
using System.Collections.Generic;
using MoriSkills;

[CreateAssetMenu(fileName = "ItemDamageAttr", menuName = "Scriptable Objects/ItemAttr/ItemDamageAttr")]
public class ItemDamageAttr : ItemAttr
{
    public int damageAmount;

    public DamageType damageType;

    public Element element;

     public ItemDamageAttr(int amount)
    {
        name = "DamageAttr";

        damageAmount = amount;
    }

    public override void ActivateItemAttrWIB(UnitBody target)
    {
        target.TakeDamage(damageAmount,damageType,element);
    }
}
