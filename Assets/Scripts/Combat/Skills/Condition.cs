using System;
using System.Collections;
using JetBrains.Annotations;
using MoriSkills;
using Unity.VisualScripting;
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
    public Sprite conditionSprite;
    public UnitBody unit;
    public int duration;

    public string description;

    //Higher = earlier
    public int activationPriority;

    public Condition(int effectDuration, int conditionPriority = 0, string conditionName = "DefaultCondition")
    {
        name = conditionName;

        //-1 is a permanent effect, -2 is unclearable effect (maybe)
        duration = effectDuration;
        activationPriority = conditionPriority;
        Holder temp = new Holder();
        foreach (Holder item in IconHolder.Instance.holders)
        {
            if(item.key == conditionName)
            {
                temp = item;
                break;
            }
        }
        conditionSprite = temp.value;
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
                appliedUnit.updateConditions.Invoke();

                return;
            }
        }
        appliedUnit.conditions.Add(this);
        OnApply(appliedUnit);
        appliedUnit.updateConditions.Invoke();
    }

    public void RemoveCondition()
    {
        // THESE STILL EXIST SOMEWHERE IN MEMORY SOMEHOW
        OnRemove();
        unit.EndOfTurn.RemoveListener(CountDown);
        unit.conditions.Remove(this);
        unit.updateConditions.Invoke();
    }

    public void CountDown()
    {
        if (duration >= 1)
        {
            duration -= 1;
            unit.updateConditions.Invoke();
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

    public AttackBoostCondition(int conditionStrength, int effectDuration,int conditionPriority = 0): base(effectDuration, conditionPriority,"Attack Boost")
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

    public AttackDropCondition(int conditionStrength, int effectDuration,int conditionPriority = 0): base(effectDuration, conditionPriority,"Attack Down")
    {
        description = "Lowers Attack by 20%";
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
public class DefenseBoostCondition : Condition
{
    public float multiplier; //Percent increase

    public DefenseBoostCondition(int conditionStrength, int effectDuration,int conditionPriority = 0): base(effectDuration, conditionPriority,"Defense Boost")
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
public class DefenseDropCondition : Condition
{
    public float multiplier; //Percent increase

    public DefenseDropCondition(int conditionStrength, int effectDuration,int conditionPriority = 0): base(effectDuration, conditionPriority,"Defense Down")
    {
        description = "Lowers Defense by 20%";
        
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
public class MagicDefenseBoostCondition : Condition
{
    public float multiplier; //Percent increase

    public MagicDefenseBoostCondition(int conditionStrength, int effectDuration,int conditionPriority = 0): base(effectDuration, conditionPriority,"Magic Defense Boost")
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
public class MagicDefenseDropCondition : Condition
{
    public float multiplier; //Percent increase

    public MagicDefenseDropCondition(int conditionStrength, int effectDuration,int conditionPriority = 0): base(effectDuration, conditionPriority,"Magic Defense Down")
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
public class PriorityBoostCondition : Condition
{
    public int priority = 0;

    public PriorityBoostCondition(int conditionStrength, int effectDuration,int conditionPriority = 0): base(effectDuration, conditionPriority,"Priority Up")
    {
        priority = conditionStrength;
    }
    public override void OnApply(UnitBody appliedUnit)
    {
        unit = appliedUnit;
        TurnOrderManager.Instance.Prioritize(unit, priority);
        unit.EndOfTurn.AddListener(CountDown);
    }
    public override void OnRemove()
    {
        TurnOrderManager.Instance.Prioritize(unit, -priority);
    }

}

[System.Serializable]
public class PriorityDropCondition : Condition
{
    public int priority = 0;

    public PriorityDropCondition(int conditionStrength, int effectDuration,int conditionPriority = 0): base(effectDuration, conditionPriority,"Priority Down")
    {
      
        priority = conditionStrength;
    }
    public override void OnApply(UnitBody appliedUnit)
    {
        unit = appliedUnit;
        TurnOrderManager.Instance.Prioritize(unit, -priority);
        unit.EndOfTurn.AddListener(CountDown);
    }
    public override void OnRemove()
    {
        TurnOrderManager.Instance.Prioritize(unit, priority);
    }

}

[System.Serializable]
public class DamageOverTimeCondition : Condition
{
    public int category;
    public int damageValue;
    public int attackMult = 1;
    
    public DamageOverTimeCondition(int damageCat /*1 = set damage, 2 = attack power, 3 = fixed percent*/, int conditionStrength, int effectDuration,int conditionPriority = 0): base(effectDuration, conditionPriority,"Poison Condition")
    {
        category = damageCat;
        damageValue = conditionStrength;
        if(damageCat == 2)
        {
            attackMult = TurnOrderManager.Instance.turnPlayer.activeStats.Attack;
        }
        
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


public class Tremor: Condition
{
    public int power;
    public int stacks;

    public Tremor(int userPower, int addedStacks):base(-1,0,"Tremor")
    {
        power = userPower;
        stacks = addedStacks;
    }

    public override void ApplyCondition(UnitBody appliedUnit)
    {
        foreach(Condition condition in appliedUnit.conditions)
        {
            if(condition.GetType() == this.GetType())
            {
                (condition as Tremor).stacks += stacks;
                unit.updateConditions.Invoke();

                return;
            }
        }
        appliedUnit.conditions.Add(this);
        OnApply(appliedUnit);
        unit.updateConditions.Invoke();
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
        unit.TakeDamage(power*stacks, DamageType.Destined, Element.None);
    }
}

