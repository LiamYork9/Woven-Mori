using UnityEngine;

using System.Collections.Generic;
using MoriSkills;

[CreateAssetMenu(fileName = "ApGainAttr", menuName = "Scriptable Objects/ItemAttr/ItemApGainAttr")]
public class ItemApGainAttr : ItemAttr
{
    public int apAdd;
     public override void ActivateItemAttrWIB(UnitBody target)
    {
        target.AP += apAdd;
    }
}
