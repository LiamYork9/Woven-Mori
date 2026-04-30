using System;
using System.Collections;
using JetBrains.Annotations;
using MoriSkills;
using UnityEditor;
using UnityEngine;

public enum EffectTime
{
    TurnStart,
    PreAction,
    PostAction,
    TurnEnd
}

[System.Serializable]
public class Condition
{
    public string name;
    public UnitBody unit;
    public int duration;

    //Higher = earlier
    public int priority;

    public Condition(int effectDuration, int conditionPriority = 0)
    {
        name = "Blank Condition";

        //-1 is a permanent effect, -2 is unclearable effect (maybe)
        duration = effectDuration;
        priority = conditionPriority;
    }

    public virtual void OnApply(UnitBody appliedUnit)
    {
        unit = appliedUnit;
        unit.EndOfTurn.AddListener(CountDown);
    }

    public virtual void OnRemove()
    {

    }

    public virtual void Activate()
    {

    }

    public virtual void ApplyCondition(UnitBody appliedUnit)
    {
        
        foreach(Condition condition in appliedUnit.conditions)
        {
            if(condition.GetType() == this.GetType())
            {
                if(condition.duration < this.duration)
                {
                    condition.duration = duration;
                }

                return;
            }
        }
        appliedUnit.conditions.Add(this);
        OnApply(appliedUnit);
    }

    public void RemoveCondition()
    {
        // THESE STILL EXIST SOMEWHERE IN MEMORY SOMEHOW
        OnRemove();
        unit.EndOfTurn.RemoveListener(CountDown);
        unit.conditions.Remove(this);
    }

    public void CountDown()
    {
        if (duration >= 1)
        {
            duration -= 1;
        }
        if (duration == 0)
        {
            RemoveCondition();
        }
    }

}

[System.Serializable]
public class AttackBoostCondition : Condition
{
    public float multiplier; //Percent increase

    public AttackBoostCondition(int conditionStrength, int effectDuration,int conditionPriority = 0): base(effectDuration, conditionPriority)
    {
        multiplier = conditionStrength;
    }
    public override void OnApply(UnitBody appliedUnit)
    {
        unit = appliedUnit;
        appliedUnit.activeStats.Attack += (int)(appliedUnit.baseStats.Attack*(multiplier/100));
        unit.EndOfTurn.AddListener(CountDown);
    }
    public override void OnRemove()
    {
        unit.activeStats.Attack -= (int)(unit.baseStats.Attack*(multiplier/100));
    }

}
[System.Serializable]
public class AttackDropCondition : Condition
{
    public float multiplier; //Percent increase

    public AttackDropCondition(int conditionStrength, int effectDuration,int conditionPriority = 0): base(effectDuration, conditionPriority)
    {
        multiplier = conditionStrength;
    }
    public override void OnApply(UnitBody appliedUnit)
    {
        unit = appliedUnit;
        appliedUnit.activeStats.Attack -= (int)(appliedUnit.baseStats.Attack*(multiplier/100));
        unit.EndOfTurn.AddListener(CountDown);
    }
    public override void OnRemove()
    {
        unit.activeStats.Attack += (int)(unit.baseStats.Attack*(multiplier/100));
    }

}

[System.Serializable]
public class MagicDefenseBoostCondition : Condition
{
    public float multiplier; //Percent increase

    public MagicDefenseBoostCondition(int conditionStrength, int effectDuration,int conditionPriority = 0): base(effectDuration, conditionPriority)
    {
        multiplier = conditionStrength;
    }
    public override void OnApply(UnitBody appliedUnit)
    {
        unit = appliedUnit;
        appliedUnit.activeStats.Defense += (int)(appliedUnit.baseStats.Defense*(multiplier/100));
        unit.EndOfTurn.AddListener(CountDown);
    }
    public override void OnRemove()
    {
        unit.activeStats.Defense -= (int)(unit.baseStats.Defense*(multiplier/100));
    }

}
[System.Serializable]
public class DefenseDropCondition : Condition
{
    public float multiplier; //Percent increase

    public DefenseDropCondition(int conditionStrength, int effectDuration,int conditionPriority = 0): base(effectDuration, conditionPriority)
    {
        multiplier = conditionStrength;
    }
    public override void OnApply(UnitBody appliedUnit)
    {
        unit = appliedUnit;
        appliedUnit.activeStats.Defense -= (int)(appliedUnit.baseStats.Defense*(multiplier/100));
        unit.EndOfTurn.AddListener(CountDown);
    }
    public override void OnRemove()
    {
        unit.activeStats.Defense += (int)(unit.baseStats.Defense*(multiplier/100));
    }

}
[System.Serializable]
public class DefenseBoostCondition : Condition
{
    public float multiplier; //Percent increase

    public DefenseBoostCondition(int conditionStrength, int effectDuration,int conditionPriority = 0): base(effectDuration, conditionPriority)
    {
        multiplier = conditionStrength;
    }
    public override void OnApply(UnitBody appliedUnit)
    {
        unit = appliedUnit;
        appliedUnit.activeStats.Mdefense += (int)(appliedUnit.baseStats.Mdefense*(multiplier/100));
        unit.EndOfTurn.AddListener(CountDown);
    }
    public override void OnRemove()
    {
        unit.activeStats.Mdefense -= (int)(unit.baseStats.Mdefense*(multiplier/100));
    }

}
[System.Serializable]
public class MagicDefenseDropCondition : Condition
{
    public float multiplier; //Percent increase

    public MagicDefenseDropCondition(int conditionStrength, int effectDuration,int conditionPriority = 0): base(effectDuration, conditionPriority)
    {
        multiplier = conditionStrength;
    }
    public override void OnApply(UnitBody appliedUnit)
    {
        unit = appliedUnit;
        appliedUnit.activeStats.Mdefense -= (int)(appliedUnit.baseStats.Mdefense*(multiplier/100));
        unit.EndOfTurn.AddListener(CountDown);
    }
    public override void OnRemove()
    {
        unit.activeStats.Mdefense += (int)(unit.baseStats.Mdefense*(multiplier/100));
    }

}

[System.Serializable]
public class SpeedBoostCondition : Condition
{
    public float multiplier; //Percent increase

    public SpeedBoostCondition(int conditionStrength, int effectDuration,int conditionPriority = 0): base(effectDuration, conditionPriority)
    {
        multiplier = conditionStrength;
    }
    public override void OnApply(UnitBody appliedUnit)
    {
        unit = appliedUnit;
        appliedUnit.activeStats.Speed += (int)(appliedUnit.baseStats.Speed*(multiplier/100));
        foreach(Turn turn in TurnOrderManager.Instance.turnOrder)
        {
            if(turn.unit == unit)
            {
                turn.initiative += unit.baseStats.Speed;
            }
        }
        TurnOrderManager.Instance.InitiativeSort();
        unit.EndOfTurn.AddListener(CountDown);
    }
    public override void OnRemove()
    {
        unit.activeStats.Speed -= (int)(unit.baseStats.Speed*(multiplier/100));
        foreach(Turn turn in TurnOrderManager.Instance.turnOrder)
        {
            if(turn.unit == unit)
            {
                turn.initiative -= unit.baseStats.Speed;
            }
        }
        TurnOrderManager.Instance.InitiativeSort();
    }

}
[System.Serializable]
public class SpeedDropCondition : Condition
{
    public float multiplier; //Percent increase

    public SpeedDropCondition(int conditionStrength, int effectDuration,int conditionPriority = 0): base(effectDuration, conditionPriority)
    {
        multiplier = conditionStrength;
    }
    public override void OnApply(UnitBody appliedUnit)
    {
        unit = appliedUnit;
        appliedUnit.activeStats.Speed -= (int)(appliedUnit.baseStats.Speed*(multiplier/100));
        foreach(Turn turn in TurnOrderManager.Instance.turnOrder)
        {
            if(turn.unit == unit)
            {
                turn.initiative -= unit.baseStats.Speed;
            }
        }
        TurnOrderManager.Instance.InitiativeSort();
        unit.EndOfTurn.AddListener(CountDown);
    }
    public override void OnRemove()
    {
        unit.activeStats.Speed += (int)(unit.baseStats.Speed*(multiplier/100));
        foreach(Turn turn in TurnOrderManager.Instance.turnOrder)
        {
            if(turn.unit == unit)
            {
                turn.initiative += unit.baseStats.Speed;
            }
        }
        TurnOrderManager.Instance.InitiativeSort();
    }

}


[System.Serializable]
public class DamageOverTimeCondition : Condition
{
    public int category;
    public int damageValue;
    public int attackMult = 1;
    
    public DamageOverTimeCondition(int damageCat /*1 = set damage, 2 = attack power, 3 = fixed percent*/, int conditionStrength, int effectDuration,int conditionPriority = 0): base(effectDuration, conditionPriority)
    {
        category = damageCat;
        damageValue = conditionStrength;
        if(damageCat == 2)
        {
            attackMult = TurnOrderManager.Instance.turnPlayer.activeStats.Attack;
        }
        name = "Poison Condition";
    }

    public override void OnApply(UnitBody appliedUnit)
    {
        unit = appliedUnit;
        unit.EndOfTurn.AddListener(Activate);
        unit.EndOfTurn.AddListener(CountDown);
    }

    public override void OnRemove()
    {
        unit.EndOfTurn.RemoveListener(Activate);
    }

    public override void Activate()
    {
        switch(category)
        {
            case 1: //Set Damage
                unit.TakeDamage(damageValue, DamageType.Destined, Element.None);
                break;
            case 2: //Base Power Damage
                unit.TakeDamage(damageValue*attackMult, DamageType.Magic, Element.Dark);
                break;
            case 3: //%HP Damage
                int temp =(damageValue * unit.activeStats.MaxHP)/100;
                if(temp<1)
                {
                    temp = 1;
                }
                unit.TakeDamage(temp, DamageType.Destined, Element.None);
                break;
            default:
                unit.TakeDamage(damageValue, DamageType.Destined, Element.None); 
                break;
        }
    }
}
