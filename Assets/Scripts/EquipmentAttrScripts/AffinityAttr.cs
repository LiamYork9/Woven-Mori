using System.Collections.Generic;
using MoriSkills;
using UnityEngine;

[CreateAssetMenu(fileName = "AffinityAttr", menuName = "ScriptableObjects/EqupmentAttr/AffinityAttrr")]
public class AffinityAttr : EquipmentAttr
{
    public List<Element> vulnerability;

    public List<Element> immunity;

    public List<Element> resistince;
    public override void ActivateAttr(UnitBody user)
    {
        user.vulnerability.AddRange(vulnerability);
        for(int i = 0; i<user.vulnerability.Count; i++)
        {
            Debug.Log(user.name + " vulnerable" + user.vulnerability[i] + " at " + i);
        }
        user.immunity.AddRange(immunity);
         for(int i = 0; i<user.immunity.Count; i++)
        {
            Debug.Log(user.name + " immune" + user.immunity[i] + " at " + i);
        }
        user.resistance.AddRange(resistince);
         for(int i = 0; i<user.resistance.Count; i++)
        {
            Debug.Log(user.name + " resist" + user.resistance[i] + " at " + i);
        }
    }
}
