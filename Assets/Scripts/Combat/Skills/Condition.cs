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
public class StatBoostCondition : Condition
{
    public Stats boostStat;
    public int boostValue;
    
    public StatBoostCondition(Stats boostedStat, int boost, int effectDuration,int conditionPriority = 0): base(effectDuration, conditionPriority)
    {
        boostStat = boostedStat;
        boostValue = boost;
        name = boostStat + " Boost Condition";
    }

    public override void OnApply(UnitBody appliedUnit)
    {
        unit = appliedUnit;
        if (boostStat == Stats.Attack)
        {
            unit.attack += boostValue;
        }
        if (boostStat == Stats.Defence)
        {
            unit.defense += boostValue;
        }
        if (boostStat == Stats.mDefense)
        {
            unit.mDefense += boostValue;
        }
        if (boostStat == Stats.Speed)
        {
            unit.speed += boostValue;
        }
        unit.EndOfTurn.AddListener(CountDown);
    }

    public override void OnRemove()
    {
        if (boostStat == Stats.Attack)
        {
            unit.attack -= boostValue;
        }
        if (boostStat == Stats.Defence)
        {
            unit.defense -= boostValue;
        }
        if (boostStat == Stats.mDefense)
        {
            unit.mDefense -= boostValue;
        }
        if (boostStat == Stats.Speed)
        {
            unit.speed -= boostValue;
        }
    }

    public override void Activate()
    {

    }
}
