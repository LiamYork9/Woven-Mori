using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

[CreateAssetMenu(fileName = "ItemStatGrowthAttr", menuName = "Scriptable Objects/ItemAttr/StatGrowthAttr")]
public class StatGrowthAttr : ItemAttr
{
    public StatTarget statTarget;
    public GrowthTarget growthTarget;
    public int changeAmount;
    public bool applyInstant = false;

    public StatGrowthAttr( StatTarget stat, GrowthTarget growth,int amount, bool instant = false)
    {
        name = "StatGrowthAttr";
        statTarget = stat;
        growthTarget = growth;
        changeAmount = amount;
        applyInstant = instant;
    }

    public override void ActivateItemAttrOFB(PlayerCharacter target)
    {
        target.ChangeGrowth(statTarget, growthTarget, changeAmount, applyInstant);
    }

     public override void ActivateItemAttrWIB(UnitBody target)
    {
    }
}
