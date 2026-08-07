using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoriSkills
{
    public enum ConditionModifiers
    {
        None,
        Skill,
        Attack,
        Buff,
        Healing,
    }
    public class SkillCondition
    {
        public string name;
        public Skill skill;
        public ConditionModifiers conditionMod;

        public SkillCondition(ConditionModifiers conMod)
        {
            conditionMod = conMod;
            name = "Generic condition";
        }

        public virtual Skill CheckConditionSkillMod(Skill checkedSkill, UnitBody user)
        {
            skill = checkedSkill;
            return skill;
        }
        public virtual SkillCondition ShallowCopy()
        {
            return (SkillCondition)this.MemberwiseClone();
        }
    }

    public class EvenOddCondition : SkillCondition
    {
        [SerializeReference]
        public List<SkillAttr> evenAttrs = new List<SkillAttr>();
        [SerializeReference]
        public List<SkillAttr> oddAttrs = new List<SkillAttr>();

        public int evenPower;
        public int oddPower;
        public Element evenElement;
        public Element oddElement;
        public Target evenTarget;
        public Target oddTarget;
        public Category evenCategory;
        public Category oddCategory;
        public int evenCost;
        public int oddCost;
        public int evenTurnShift;
        public int oddTurnShift;

        public EvenOddCondition(
            int ePower,Element eElem, Target eTarget, Category eCat, int eCost, int eShift,
            int oPower,Element oElem, Target oTarget, Category oCat, int oCost, int oShift
            ):base(ConditionModifiers.Skill)
        {
            name = "Even Odd Condition";

            evenPower = ePower;
            oddPower = oPower;
            
            evenElement = eElem;
            oddElement = oElem;
            
            evenTarget = eTarget;
            oddTarget = oTarget;
            
            evenCategory = eCat;
            oddCategory = oCat;
            
            evenCost = eCost;
            oddCost = oCost;
            
            evenTurnShift = eShift;
            oddTurnShift = oShift;
            
        }

        public EvenOddCondition Even(SkillAttr addedAttr)
        {
            if(evenAttrs == null)
            {
                evenAttrs = new List<SkillAttr> { };
            }
            evenAttrs.Add(addedAttr);
            return this;
        }

        
        public EvenOddCondition Odd(SkillAttr addedAttr)
        {
            if(oddAttrs == null)
            {
                oddAttrs = new List<SkillAttr> { };
            }
            oddAttrs.Add(addedAttr);
            return this;
        }

        public override Skill CheckConditionSkillMod(Skill CheckedSkill, UnitBody user)
        {
            skill = CheckedSkill;
            if(BattleManager.Instance.globalTurn%2 == 0)
            {
                skill.power = evenPower;
                skill.element = evenElement;
                skill.target = evenTarget;
                skill.category = evenCategory;
                skill.cost = evenCost;
                skill.turnShift = evenTurnShift;
                skill.attrs = evenAttrs;
            }
            else
            {
                skill.power = oddPower;
                skill.element = oddElement;
                skill.target = oddTarget;
                skill.category = oddCategory;
                skill.cost = oddCost;
                skill.turnShift = oddTurnShift;
                skill.attrs = oddAttrs;
            }


            return skill;
        }

    }


    public class HighLowCondition : SkillCondition
    {
        [SerializeReference]
        public List<SkillAttr> highAttrs = new List<SkillAttr>();
        [SerializeReference]
        public List<SkillAttr> lowAttrs = new List<SkillAttr>();

        public int highPower;
        public int lowPower;
        public Element highElement;
        public Element lowElement;
        public Target highTarget;
        public Target lowTarget;
        public Category highCategory;
        public Category lowCategory;
        public int highCost;
        public int lowCost;
        public int highTurnShift;
        public int lowTurnShift;

        public int threshold;
        public bool local;

        public HighLowCondition(
            int hPower,Element hElem, Target hTarget, Category hCat, int hCost, int hShift,
            int lPower,Element lElem, Target lTarget, Category lCat, int lCost, int lShift,
            int highThreshold, bool useLocal = false
            ):base(ConditionModifiers.Skill)
        {
            name = "Even Odd Condition";

            highPower = hPower;
            lowPower = lPower;
            
            highElement = hElem;
            lowElement = lElem;
            
            highTarget = hTarget;
            lowTarget = lTarget;
            
            highCategory = hCat;
            lowCategory = lCat;
            
            highCost = hCost;
            lowCost = lCost;
            
            highTurnShift = hShift;
            lowTurnShift = lShift;

            threshold = highThreshold;
            local = useLocal;
            
        }

        public HighLowCondition High(SkillAttr addedAttr)
        {
            if(highAttrs == null)
            {
                highAttrs = new List<SkillAttr> { };
            }
            highAttrs.Add(addedAttr);
            return this;
        }

        
        public HighLowCondition Odd(SkillAttr addedAttr)
        {
            if(lowAttrs == null)
            {
                lowAttrs = new List<SkillAttr> { };
            }
            lowAttrs.Add(addedAttr);
            return this;
        }

        public override Skill CheckConditionSkillMod(Skill CheckedSkill, UnitBody user)
        {

            skill = CheckedSkill;
            int temp = 0;
            if(local)
            {
                temp = user.localTurnCount;
            }
            else
            {
                temp = BattleManager.Instance.globalTurn;
            }
            if(temp >= threshold)
            {
                skill.power = highPower;
                skill.element = highElement;
                skill.target = highTarget;
                skill.category = highCategory;
                skill.cost = highCost;
                skill.turnShift = highTurnShift;
                skill.attrs = highAttrs;
            }
            else
            {
                skill.power = lowPower;
                skill.element = lowElement;
                skill.target = lowTarget;
                skill.category = lowCategory;
                skill.cost = lowCost;
                skill.turnShift = lowTurnShift;
                skill.attrs = lowAttrs;
            }


            return skill;
        }

    }
}