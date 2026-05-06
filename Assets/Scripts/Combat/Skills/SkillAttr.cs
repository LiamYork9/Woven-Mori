using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

namespace MoriSkills
{
    [Flags]
    public enum Stats
    {
        Attack = 1 << 0,
        Defense = 1 << 1,
        mDefense = 1 << 2,
        Speed = 1 << 3
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

        public virtual void ActivateAttr(UnitBody unitUser, UnitBody unitTarget, int power, Element skillElement)
        {

        }
    }

    public class DamageAttr : SkillAttr
    {
        public float mutiplier;
        public DamageType type;
        public Element element;

        public bool changeElement;

        public DamageAttr(float skillMult, DamageType damageType, Element damageElement = Element.None,bool elementOverride = false, bool targetSelf = false) : base(targetSelf)
        {
            name = "DamageAttr";
            mutiplier = skillMult;
            type = damageType;
            element = damageElement;
            changeElement = elementOverride;
        }

        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget, int power,Element skillElement)
        {
            if(changeElement == false)
            {
                element = skillElement;
            }
            if(targetSelf)
            {
                unitUser.TakeDamage((int)(power * mutiplier * unitUser.activeStats.Attack), type, element);                
            }
            else
            {
                unitTarget.TakeDamage((int)(power * mutiplier * unitUser.activeStats.Attack), type, element);
            }
           
        }

    }




    public class LevelScaleAttr : SkillAttr
    {
        int scaleValue;
        [SerializeReference]
        public List<SkillAttr> scaledAttr = null;

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

        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget, int power,Element skillElement)
        {
            for(int i = 0; i < scaledAttr.Count; i++)
            {
                
                scaledAttr[i].ActivateAttr(unitUser, unitTarget,power+scaleValue*unitUser.activeStats.Level, skillElement);
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
        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget, int power,Element skillElement)
        {
            UnitBody temp;
            if (targetSelf==true)
            {
                temp = unitUser;
            }
            else
            {
                temp = unitTarget;
            }

            if((stat & Stats.Attack) == Stats.Attack)
            {
             temp.ApplyCondition(new AttackBoostCondition(boost, duration));
            }
            if((stat & Stats.Defense) == Stats.Defense)
            {
             temp.ApplyCondition(new DefenseBoostCondition(boost, duration));
            }
            if((stat & Stats.mDefense) == Stats.mDefense)
            {
             temp.ApplyCondition(new MagicDefenseBoostCondition(boost, duration));
            }
            if((stat & Stats.Speed) == Stats.Speed)
            {
             temp.ApplyCondition(new SpeedBoostCondition(boost, duration));
            }
        }
    }

    public class StatDropConAttr : SkillAttr
    {
        public Stats stat;

        public int boost;

        public int duration;
        public StatDropConAttr(Stats boostedStat, int boostNum, int conDuration = 3, bool targetSelf = false) : base(targetSelf)
        {
            name = "StatDropAttr";
            stat = boostedStat;
            duration = conDuration;
            boost = boostNum;
            
        }
        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget, int power,Element skillElement)
        {
            UnitBody temp;
            if (targetSelf==true)
            {
                temp = unitUser;
            }
            else
            {
                temp = unitTarget;
            }

            if((stat & Stats.Attack) == Stats.Attack)
            {
             temp.ApplyCondition(new AttackDropCondition(boost, duration));
            }
            if((stat & Stats.Defense) == Stats.Defense)
            {
             temp.ApplyCondition(new DefenseDropCondition(boost, duration));
            }
            if((stat & Stats.mDefense) == Stats.mDefense)
            {
             temp.ApplyCondition(new MagicDefenseDropCondition(boost, duration));
            }
             if((stat & Stats.Speed) == Stats.Speed)
            {
             temp.ApplyCondition(new SpeedDropCondition(boost, duration));
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

        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget, int power,Element skillElement)
        {
            
            if (targetSelf == true)
            {
                target = unitUser;
            }
            else
            {
                target = unitTarget;
            }
            target.activeStats.CurrentHP += (int)((healMultiplier + (unitUser.activeStats.Attack*statModifier/10))*power);
            if (target.activeStats.CurrentHP>target.activeStats.MaxHP)
            {
                target.activeStats.CurrentHP = target.activeStats.MaxHP;
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
        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget, int power,Element skillElement)
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

        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget, int power,Element skillElement)
        {
            if (BattleManager.Instance.globalTurn % 2 == 0)
            {
                for (int i = 0; i < evenAttr.Count; i++)
                {
                    evenAttr[i].ActivateAttr(unitUser, unitTarget,power,skillElement);
                }
            }
            else
            {
                for (int i = 0; i < oddAttr.Count; i++)
                {
                    oddAttr[i].ActivateAttr(unitUser, unitTarget,power,skillElement);
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
        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget, int power,Element skillElement)
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

    public class APGainAttr : SkillAttr
    {
        public int amount;


        public APGainAttr(int APchange = 1, bool targetSelf = false) : base(targetSelf)
        {
            name = "APAttr";
            amount = APchange;
        }
        public override void ActivateAttr(UnitBody unitUser, UnitBody unitTarget, int power,Element skillElement)
        {
            if (targetSelf == true)
            {
                unitUser.AP+=amount;
                if (unitUser.AP < 0)
                {
                    unitUser.AP = 0;
                }
            }
            else
            {
                unitTarget.AP+=amount;
                if (unitTarget.AP < 0)
                {
                    unitTarget.AP = 0;
                }
            }
        }
    }
}