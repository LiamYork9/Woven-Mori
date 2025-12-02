using System.Collections.Generic;
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

    public enum DamageType
    {
        Physical,
        Magic,
        Destined
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

    public class DamageAttr : SkillAttr
    {
        public int power;
        DamageType type;
        Element element;

        public DamageAttr(int skillPower, DamageType damageType, Element damageElement = Element.None, bool targetSelf = false) : base(targetSelf)
        {
            name = "DamageAttr";
            power = skillPower;
            type = damageType;
            element = damageElement;
        }

        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget)
        {
            unitTarget.TakeDamage(power * unitUser.attack, type, element);
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

    public class LevelScaleAttr : SkillAttr
    {
        int scaleValue;
        [SerializeReference]
        List<SkillAttr> scaledAttr = null;

        public LevelScaleAttr Attr(SkillAttr addedAttr)
        {
            if(scaledAttr == null)
            {
                scaledAttr = new List<SkillAttr> { };
            }
            scaledAttr.Add(addedAttr);
            return this;
        }


        public LevelScaleAttr(int scaleRate, bool targetSelf = false) : base(targetSelf)
        {
            name = "LevelScaleAttr";
            scaleValue = scaleRate;
        }

        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget)
        {
            for(int i = 0; i < scaledAttr.Count; i++)
            {
                if (scaledAttr[i] is DamageAttr)
                {
                    ((DamageAttr)scaledAttr[i]).power = scaleValue * unitUser.level;
                }
                scaledAttr[i].ActivateAttr(unitUser, unitTarget);
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
        UnitBody target = null;

        public HealAttr(int healNum, bool targetSelf = false) : base(targetSelf)

        {
            name = "HealAttr";
            healAmount = healNum;

        }

        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget)
        {
            
            if (targetSelf == true)
            {
                target = unitUser;
            }
            else
            {
                target = unitTarget;
            }
            target.currentHP += healAmount;
            if (target.currentHP>target.maxHP)
            {
                target.currentHP = target.maxHP;
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
            if (targetSelf == true)
            {
                unitUser.ApplyCondition(new Condition(duration));
            }
            else
            {
                unitTarget.ApplyCondition(new Condition(duration));
            }
        }
    }
    
    public class  EvenOddAttr : SkillAttr
    {
        [SerializeReference]
        List<SkillAttr> evenAttr = null;
        [SerializeReference]
        List<SkillAttr> oddAttr = null;

        public EvenOddAttr Even(SkillAttr addedAttr)
        {
            if(evenAttr == null)
            {
                evenAttr = new List<SkillAttr> { };
            }
            evenAttr.Add(addedAttr);
            return this;
        }

        
        public EvenOddAttr Odd(SkillAttr addedAttr)
        {
            if(oddAttr == null)
            {
                oddAttr = new List<SkillAttr> { };
            }
            oddAttr.Add(addedAttr);
            return this;
        }


        public EvenOddAttr(bool targetSelf = false) : base(targetSelf)
        {
            name = "EvenOddAttr";
        }

        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget)
        {
            if (BattleManager.Instance.globalTurn % 2 == 0)
            {
                for (int i = 0; i < evenAttr.Count; i++)
                {
                    evenAttr[i].ActivateAttr(unitUser, unitTarget);
                }
            }
            else
            {
                for (int i = 0; i < oddAttr.Count; i++)
                {
                    oddAttr[i].ActivateAttr(unitUser, unitTarget);
                }
            }
        }
    }
}