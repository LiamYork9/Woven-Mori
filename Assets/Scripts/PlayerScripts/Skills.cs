using System;
using System.Collections.Generic;
using UnityEngine;
public class skillAttr
{

}

public class condtion
{
    
}
public class Skills : MonoBehaviour
{
    public enum skillId
    {
        None
    }
    public string skillName;

    public enum element
    {
        Fire,
        Water,
        Earth,
        Air,
        Dark,
        Light,
        Fate
    }

    public enum target
    {
        single,
        self,
        party,
        mutipleEnemy
    }

    public enum category
    {
        Physical,
        Magic,
        Buff,
        Debuff
    }

    public int power;

    public int accurcy;

    public int cost;

    public string toolTip;

    public int chance;

    public int turnShift;

    public List<skillAttr> attrs;

    public List<condtion> condtions;

    






    void Start()
    {

    }


    void Update()
    {

    }
}
