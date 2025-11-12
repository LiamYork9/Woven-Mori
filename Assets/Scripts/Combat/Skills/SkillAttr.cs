using UnityEngine;

namespace MoriSkills
{
    public enum Stats
    {
        Attack,
        Defence,
        mDefense,
        Speed
    }

    [System.Serializable]
    public class SkillAttr
    {
        public string name;
        public bool targetSelf;

        public SkillAttr(bool doTargetSelf = false)
        {
            name = "Generic Attr";
            this.targetSelf = doTargetSelf;

        }

        public virtual void ActivateAttr(UnitBody unitUser, UnitBody unitTarget)
        {

        }
    }


    public class StatBoostAttr : SkillAttr
    {
        public Stats stat;

        public int boost;
        public StatBoostAttr(Stats boostedStat, int boostNum, bool targetSelf = false) : base(targetSelf)
        {
            name = "StatBoostAttr";
            stat = boostedStat;

            boost = boostNum;
        }

        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget)
        {
            if (targetSelf == true)
            {
                if (stat == Stats.Attack)
                {
                    unitUser.attack += boost;
                }
                if (stat == Stats.Defence)
                {
                    unitUser.defense += boost;
                }
                if (stat == Stats.mDefense)
                {
                    unitUser.mDefense += boost;
                }
                if (stat == Stats.Speed)
                {
                    unitUser.speed += boost;
                }

            }
            else
            {
                if (stat == Stats.Attack)
                {
                    unitTarget.attack += boost;
                }
                if (stat == Stats.Defence)
                {
                    unitTarget.defense += boost;
                }
                if (stat == Stats.mDefense)
                {
                    unitTarget.mDefense += boost;
                }
                if (stat == Stats.Speed)
                {
                    unitTarget.speed += boost;
                }
            }
        }


    }

    public class StatBoostConAttr : SkillAttr
    {
        public Stats stat;

        public int boost;
        public StatBoostConAttr(Stats boostedStat, int boostNum, bool targetSelf = false) : base(targetSelf)
        {
            name = "StatBoostAttr";
            stat = boostedStat;

            boost = boostNum;
        }
        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget)
        {
            if (targetSelf==true)
            {
                unitUser.ApplyCondition(new StatBoostCondition(stat, boost, 3));
            }
            else
            {
                unitTarget.ApplyCondition(new StatBoostCondition(stat, boost, 3));
            }
        }
    }

    public class HealAttr : SkillAttr
    {
        public int healAmount;

        public HealAttr(int healNum, bool targetSelf = false) : base(targetSelf)

        {
            name = "HealAttr";
            healAmount = healNum;

        }

        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget)
        {
            if (targetSelf == true)
            {
                unitUser.currentHP += healAmount;
            }
            else
            {
                unitTarget.currentHP += healAmount;
            }

        }
    }

    public class ApplyConditionAttr : SkillAttr
    {
        public int duration;
        public ApplyConditionAttr(int conditionDuration = 2, bool targetSelf = false) : base(targetSelf)
        {
            name = "ApplyConditionAttr";
            duration = conditionDuration;
        }
        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget)
        {
            if (targetSelf==true)
            {
                unitUser.ApplyCondition(new Condition(duration));
            }
            else
            {
                unitTarget.ApplyCondition(new Condition(duration));
            }
        }
    }
}