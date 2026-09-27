using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
    public enum DamageAfterEffect
    {
        None,
        Recoil,
        Lifesteal
    };

    [System.Serializable]
    public class SkillAttr
    {
        public string name;
        public int chance;
        public bool targetSelf;

        public SkillAttr(bool doTargetSelf = false, int effChance = 100)
        {
            name = "Generic Attr";
            
            this.targetSelf = doTargetSelf;

        }

        public virtual SkillAttr ShallowCopy()
        {
            return (SkillAttr)this.MemberwiseClone();
        }

        public virtual void ActivateAttr(UnitBody unitUser, List<UnitBody> unitTargets, int power, Element skillElement)
        {

        }
    }

    public class DamageAttr : SkillAttr
    {
        
        public float mutiplier;
        public DamageType type;
        public Element element;

        public bool changeElement;
        public DamageAfterEffect afterEff;
        public float afterEffMult;

        public DamageAttr(float skillMult, DamageType damageType, Element damageElement = Element.None, bool elementOverride = false, DamageAfterEffect afterEffect = DamageAfterEffect.None, float afterEffectMultiplier = 0.5f, bool targetSelf = false, int effChance = 100) : base(targetSelf, effChance)
        {
            name = "DamageAttr";
            mutiplier = skillMult;
            type = damageType;
            element = damageElement;
            changeElement = elementOverride;
            afterEff = afterEffect;
            afterEffMult = afterEffectMultiplier;

        }

        public override void ActivateAttr(UnitBody unitUser, List<UnitBody> unitTargets, int power,Element skillElement)
        {
            int damageDealt = 0;
            if(changeElement == false)
            {
                element = skillElement;
            }
            if(targetSelf)
            {
                damageDealt = unitUser.TakeDamage((int)(power * mutiplier * unitUser.activeStats.Attack), type, element);                
            }
            else
            { 
                for(int i=0; i<unitTargets.Count; i++)
                {
                    damageDealt+=unitTargets[i].TakeDamage((int)(power * mutiplier * unitUser.activeStats.Attack), type, element);
                }
            }

            if(afterEff == DamageAfterEffect.Recoil)
            {
                unitUser.activeStats.CurrentHP -= (int)(damageDealt*afterEffMult);
                PopUpManager.Instance.DamageDone((int)(damageDealt*afterEffMult),unitUser.transform.position,false);
            }
            else if(afterEff == DamageAfterEffect.Lifesteal)
            {
                unitUser.activeStats.CurrentHP += (int)(damageDealt*afterEffMult);
                if(unitUser.activeStats.CurrentHP > unitUser.activeStats.MaxHP)
                {
                    unitUser.activeStats.CurrentHP = unitUser.activeStats.MaxHP;
                }
                PopUpManager.Instance.HealingDone((int)(damageDealt*afterEffMult),unitUser.transform.position,false);
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

        public override void ActivateAttr(UnitBody unitUser, List<UnitBody> unitTargets, int power,Element skillElement)
        {
            for(int i = 0; i < scaledAttr.Count; i++)
            {
                
                scaledAttr[i].ActivateAttr(unitUser, unitTargets,power+scaleValue*unitUser.activeStats.Level, skillElement);
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
        public StatBoostConAttr(Stats boostedStat, int boostNum, int conDuration = 3, bool targetSelf = false,int effChance = 100) : base(targetSelf,effChance)
        {
            name = "StatBoostAttr";
            stat = boostedStat;
            duration = conDuration;
            boost = boostNum;
        }
        public override void ActivateAttr(UnitBody unitUser, List<UnitBody> unitTargets, int power,Element skillElement)
        {
            List<UnitBody> temp = new List<UnitBody>();
            if (targetSelf==true)
            {
                temp.Add(unitUser);
            }
            else
            {
                temp.AddRange(unitTargets);
            }

            for(int i =0; i<temp.Count; i++)
            {
                if((stat & Stats.Attack) == Stats.Attack)
                {
                temp[i].ApplyCondition(new AttackBoostCondition(boost, duration));
                }
                if((stat & Stats.Defense) == Stats.Defense)
                {
                temp[i].ApplyCondition(new DefenseBoostCondition(boost, duration));
                }
                if((stat & Stats.mDefense) == Stats.mDefense)
                {
                temp[i].ApplyCondition(new MagicDefenseBoostCondition(boost, duration));
                }
            }
        }
    }

    public class StatDropConAttr : SkillAttr
    {
        public Stats stat;

        public int boost;

        public int duration;
        public StatDropConAttr(Stats boostedStat, int boostNum, int conDuration = 3, bool targetSelf = false, int effChance = 100) : base(targetSelf, effChance)
        {
            name = "StatDropAttr";
            stat = boostedStat;
            duration = conDuration;
            boost = boostNum;
            
        }
        public override void ActivateAttr(UnitBody unitUser, List<UnitBody> unitTargets, int power,Element skillElement)
        {
            List<UnitBody> temp = new List<UnitBody>();
            if (targetSelf==true)
            {
                temp.Add(unitUser);
            }
            else
            {
                temp.AddRange(unitTargets);
            }

            for(int i =0; i<temp.Count; i++)
            {
                if((stat & Stats.Attack) == Stats.Attack)
                {
                temp[i].ApplyCondition(new AttackDropCondition(boost, duration));
                }
                if((stat & Stats.Defense) == Stats.Defense)
                {
                temp[i].ApplyCondition(new DefenseDropCondition(boost, duration));
                }
                if((stat & Stats.mDefense) == Stats.mDefense)
                {
                temp[i].ApplyCondition(new MagicDefenseDropCondition(boost, duration));
                }
            }
        }
    }

    public class PriorityAttr : SkillAttr
    {
        public int priority;
        public int duration;
        public PriorityAttr(int conPriority, int conDuration = 3, bool targetSelf = false, int effChance = 100) : base(targetSelf, effChance)
        {
            priority = conPriority;
            duration = conDuration;
            if(priority < 0)
            {
                name = "Priority Down";
            }
            else
            {
                name = "priority Up";
            }
        }
        public override void ActivateAttr(UnitBody unitUser, List<UnitBody> unitTargets, int power, Element skillElement)
        {
            List<UnitBody> temp = new List<UnitBody>(); 
            
            if(targetSelf)
            {
                temp.Add(unitUser);
            }
            else
            {
               temp.AddRange(unitTargets);
            }
            for(int i =0; i<temp.Count; i++)
            {
                if(priority>=0)
                {
                    temp[i].ApplyCondition(new PriorityBoostCondition(priority,duration));
                }
                else
                {
                    
                    temp[i].ApplyCondition(new PriorityDropCondition(-priority,duration));
                }
            }
        }
    }

    public class HealAttr : SkillAttr
    {
        public float healMultiplier;
        public float statModifier;
        List<UnitBody> target;

        public HealAttr(float healMult, float statMod = .2f, bool targetSelf = false,int effChance = 100) : base(targetSelf, effChance)

        {
            name = "HealAttr";
            healMultiplier = healMult;
            statModifier = statMod;
        }

        public override void ActivateAttr(UnitBody unitUser, List<UnitBody> unitTargets, int power,Element skillElement)
        {   target = new List<UnitBody>();
            
            if (targetSelf == true)
            {
                target.Add(unitUser);
            }
            else
            {
                target.AddRange(unitTargets);
            }
            int healVal = (int)((healMultiplier + (unitUser.activeStats.Attack*statModifier/10))*power);
            for(int i =0; i<target.Count; i++)
            {
                target[i].activeStats.CurrentHP += healVal;
                if (target[i].activeStats.CurrentHP>target[i].activeStats.MaxHP)
                {
                    target[i].activeStats.CurrentHP = target[i].activeStats.MaxHP;
                }
                
                PopUpManager.Instance.HealingDone(healVal,target[i].transform.position,false);
            }

        }
    }

    public class ApplyConditionAttr : SkillAttr
    {
        public int duration;
        public ApplyConditionAttr(int conditionDuration = 2, bool targetSelf = false, int effChance = 100) : base(targetSelf,effChance)
        {
            name = "ApplyConditionAttr";
            duration = conditionDuration;
        }
        public override void ActivateAttr(UnitBody unitUser, List<UnitBody> unitTargets, int power,Element skillElement)
        {
            if (targetSelf == true)
            {
                unitUser.ApplyCondition(new Condition(duration));
            }
            else
            {
                for(int i =0; i<unitTargets.Count; i++)
                {
                    unitTargets[i].ApplyCondition(new Condition(duration));
                }
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

        public override void ActivateAttr(UnitBody unitUser, List<UnitBody> unitTargets, int power,Element skillElement)
        {
            if (BattleManager.Instance.globalTurn % 2 == 0)
            {
                for (int i = 0; i < evenAttr.Count; i++)
                {
                    evenAttr[i].ActivateAttr(unitUser, unitTargets,power,skillElement);
                }
            }
            else
            {
                for (int i = 0; i < oddAttr.Count; i++)
                {
                    oddAttr[i].ActivateAttr(unitUser, unitTargets,power,skillElement);
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


        public ApplyPoisonAttr(int conditionDuration = 2, int poisonCat = 1, int poisonPower = 5, bool targetSelf = false, int effChance = 100) : base(targetSelf, effChance)
        {
            name = "ApplyConditionAttr";
            duration = conditionDuration;
            category = poisonCat;
            potency = poisonPower;
        }
        public override void ActivateAttr(UnitBody unitUser, List<UnitBody> unitTargets, int power,Element skillElement)
        {
            if (targetSelf == true)
            {
                unitUser.ApplyCondition(new DamageOverTimeCondition(category,potency,duration));
            }
            else
            {
                for(int i =0; i<unitTargets.Count; i++)
                {
                    unitTargets[i].ApplyCondition(new DamageOverTimeCondition(category,potency,duration));
                }
            }
        }
    }

    public class APGainAttr : SkillAttr
    {
        public int amount;


        public APGainAttr(int APchange = 1, bool targetSelf = false, int effChance = 100) : base(targetSelf, effChance)
        {
            name = "APAttr";
            amount = APchange;
        }
        public override void ActivateAttr(UnitBody unitUser, List<UnitBody> unitTargets, int power,Element skillElement)
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
                for(int i =0; i<unitTargets.Count; i++)
                {
                    unitTargets[i].AP+=amount;
                    if (unitTargets[i].AP < 0)
                    {
                        unitTargets[i].AP = 0;
                    }
                }
            }
        }
    }


    public class ApplyTremorLTC : SkillAttr
    {
        public int duration;
        public bool useEnemyLTC;


        public ApplyTremorLTC(bool enemyLTC, bool targetSelf = false, int effChance = 100) : base(targetSelf, effChance)
        {
            name = "TremorLTC";
            useEnemyLTC = enemyLTC;
        }
        public override void ActivateAttr(UnitBody unitUser, List<UnitBody> unitTargets, int power,Element skillElement)
        {
            int temp = 0;
            if (targetSelf == true)
            {
                if (useEnemyLTC)
                {
                    for(int i =0; i<unitTargets.Count; i++)
                    {
                        temp += unitTargets[i].localTurnCount;
                    }
                }
                else
                {
                    temp = unitUser.localTurnCount;
                }
                if(temp<1)
                {
                    temp = 1;
                }
                unitUser.ApplyCondition(new Tremor(unitUser.activeStats.Attack,temp));
            }
            else
            {
                for(int i =0; i<unitTargets.Count; i++)
                {
                    if (useEnemyLTC)
                    {
                        temp = unitTargets[i].localTurnCount;
                    }
                    else
                    {
                        temp = unitUser.localTurnCount;
                    }
                    if(temp<1)
                    {
                        temp = 1;
                    }
                    unitTargets[i].ApplyCondition(new Tremor(unitUser.activeStats.Attack,temp));
                }
            }
        }
    }


    public class ApplyBurn : SkillAttr
    {
        public int duration;


        public ApplyBurn(int burnDuration, bool targetSelf = false, int effChance = 100) : base(targetSelf, effChance)
        {
            name = "Burn";
            duration=burnDuration;
        }
        public override void ActivateAttr(UnitBody unitUser, List<UnitBody> unitTargets, int power,Element skillElement)
        {
            if (targetSelf == true)
            {
                
                unitUser.ApplyCondition(new Tremor(unitUser.activeStats.Attack,duration));
            }
            else
            {
                for(int i =0; i<unitTargets.Count; i++)
                {
                    
                    unitTargets[i].ApplyCondition(new Burn(unitUser.activeStats.Attack,duration));
                }
            }
        }
    }



    public class PassageOfTimeAttr : SkillAttr
    {
        public int duration;
        public PassageOfTimeAttr(int timeDuration = 2, bool targetSelf = false) : base(targetSelf)
        {
            name = "PassageOfTime";
            duration = timeDuration;
        }
        public override void ActivateAttr(UnitBody unitUser, List<UnitBody> unitTargets, int power,Element skillElement)
        {
            if (targetSelf == true)
            {
                for (int i =0; i<duration; i++)
                {
                    unitUser.ConditionEndTurn.Invoke();
                    unitUser.localTurnCount+=1;
                }
            }
            else
            {
                
                for(int i =0; i<unitTargets.Count; i++)
                {
                    for (int k =0; k<duration; k++)
                    {
                        unitTargets[i].ConditionEndTurn.Invoke();
                        unitTargets[i].localTurnCount+=1;
                    }
                }
            }
        }
    }
}