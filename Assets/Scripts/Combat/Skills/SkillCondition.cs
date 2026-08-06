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

        public virtual Skill CheckConditionSkillMod(Skill checkedSkill)
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

        public override Skill CheckConditionSkillMod(Skill CheckedSkill)
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
}