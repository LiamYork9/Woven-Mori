using JetBrains.Annotations;
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
    public Unit unit;
    public string name;
    public int duration;

    //Higher = earlier
    public int priority;

    public Condition(int effectDuration)
    {
        name = "Blank Condition";

        //-1 is a permanent effect, -2 is unclearable effect (maybe)
        duration = effectDuration;
        priority = 0;
    }

    public virtual void OnApply(Unit appliedUnit)
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
        OnRemove();
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
