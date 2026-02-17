using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

        public virtual SkillAttr ShallowCopy()
        {
            return (SkillAttr)this.MemberwiseClone();
        }

        public virtual void ActivateAttr(UnitBody unitUser, UnitBody unitTarget, int power)
        {

        }
    }

    public class DamageAttr : SkillAttr
    {
        public float mutiplier;
        DamageType type;
        Element element;

        public DamageAttr(float skillMult, DamageType damageType, Element damageElement = Element.None, bool targetSelf = false) : base(targetSelf)
        {
            name = "DamageAttr";
            mutiplier = skillMult;
            type = damageType;
            element = damageElement;
        }

        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget, int power)
        {
            if(targetSelf)
            {
                unitUser.TakeDamage((int)(power * mutiplier * unitUser.attack), type, element);                
            }
            else
            {
                unitTarget.TakeDamage((int)(power * mutiplier * unitUser.attack), type, element);
            }
           
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

        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget, int power)
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

        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget, int power)
        {
            for(int i = 0; i < scaledAttr.Count; i++)
            {
                
                scaledAttr[i].ActivateAttr(unitUser, unitTarget,power*scaleValue*unitUser.level);
            }
        }

        public override SkillAttr ShallowCopy()
        {
            LevelScaleAttr temp = (LevelScaleAttr)this.MemberwiseClone();
            temp.scaledAttr = new List<SkillAttr>();
            for(int i=0; i<scaledAttr.Count; i++)
            {
                temp.scaledAttr.Add(scaledAttr[i].ShallowCopy());
            }
            return temp;
        }
    }

    public class StatBoostConAttr : SkillAttr
    {
        public Stats stat;

        public int boost;

        public int duration;
        public StatBoostConAttr(Stats boostedStat, int boostNum, int conDuration = 3, bool targetSelf = false) : base(targetSelf)
        {
            name = "StatBoostAttr";
            stat = boostedStat;
            duration = conDuration;
            boost = boostNum;
            
        }
        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget, int power)
        {
            if (targetSelf==true)
            {
                unitUser.ApplyCondition(new StatBoostCondition(stat, boost, duration));
            }
            else
            {
                unitTarget.ApplyCondition(new StatBoostCondition(stat, boost, duration));
            }
        }
    }

    public class HealAttr : SkillAttr
    {
        public float healMultiplier;
        public float statModifier;
        UnitBody target = null;

        public HealAttr(float healMult, float statMod = .2f, bool targetSelf = false) : base(targetSelf)

        {
            name = "HealAttr";
            healMultiplier = healMult;
            statModifier = statMod;

        }

        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget, int power)
        {
            
            if (targetSelf == true)
            {
                target = unitUser;
            }
            else
            {
                target = unitTarget;
            }
            target.currentHP += (int)((healMultiplier + (unitUser.attack*statModifier/10))*power);
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
        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget, int power)
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

        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget, int power)
        {
            if (BattleManager.Instance.globalTurn % 2 == 0)
            {
                for (int i = 0; i < evenAttr.Count; i++)
                {
                    evenAttr[i].ActivateAttr(unitUser, unitTarget,power);
                }
            }
            else
            {
                for (int i = 0; i < oddAttr.Count; i++)
                {
                    oddAttr[i].ActivateAttr(unitUser, unitTarget,power);
                }
            }
        }

        public override SkillAttr ShallowCopy()
        {
            EvenOddAttr temp = (EvenOddAttr)this.MemberwiseClone();
            temp.oddAttr = new List<SkillAttr>();
            temp.evenAttr = new List<SkillAttr>();
            for(int i=0; i<oddAttr.Count; i++)
            {
                temp.oddAttr.Add(oddAttr[i].ShallowCopy());
            }
            for(int i=0; i<evenAttr.Count; i++)
            {
                temp.evenAttr.Add(evenAttr[i].ShallowCopy());
            }
            return temp;
        }
    }

    public class ApplyPoisonAttr : SkillAttr
    {
        public int duration;
        public int category;
        public int potency;


        public ApplyPoisonAttr(int conditionDuration = 2, int poisonCat = 1, int poisonPower = 5, bool targetSelf = false) : base(targetSelf)
        {
            name = "ApplyConditionAttr";
            duration = conditionDuration;
            category = poisonCat;
            potency = poisonPower;
        }
        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget, int power)
        {
            if (targetSelf == true)
            {
                unitUser.ApplyCondition(new DamageOverTimeCondition(category,potency,duration));
            }
            else
            {
                unitTarget.ApplyCondition(new DamageOverTimeCondition(category,potency,duration));
            }
        }
    }
}