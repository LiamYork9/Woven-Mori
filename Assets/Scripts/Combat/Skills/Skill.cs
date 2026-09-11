using System;
using System.Collections.Generic;
using UnityEngine;


namespace MoriSkills
{

    public enum SkillId
    {
        None,
        EndBringer,
        SmallPunch,
        StaticShock,
        Flex,
        Heal,
        Attack,
        SwordShield,
        HopeDespair,
        FlameFlood,
        Fortifiy,
        PowerUp,
        BurnAway,
        TriSlash,
        FiredUp,
        Berserk,
        Rampage,
        CutLine,
        HolyAbsorption,
        HighLow



        ,//Enemy Skills
        PoisonBite,
        
        MassHeal,
        PoisonBlade,
        WindBlade,
        Focus,
        SpeedUp,
        Quake,
        Erosion
    }

    public enum Element
    {
        None,
        Fire,
        Water,
        Earth,
        Air,
        Dark,
        Light,
        Fate
    }

    public enum Target
    {
        single,
        self,
        party,
        ally,
        mutipleEnemy
    }

    public enum Category
    {
        Attack,
        Buff,
        Debuff,
        Support
    }

    [System.Serializable]
    public class Skill
    {
        public String name;
        public SkillId skillId;
        public Element element;
        public Target target;



        public Category category;

        public int power;

        public int cost;

        public string toolTip;

        public int turnShift;

        [SerializeReference]
        public List<SkillAttr> attrs;
        
        [SerializeReference]
        public List<SkillCondition> conditions;


        public Skill(SkillId SskillId, string SskillName, int Spower, Element Selement, Target defualtTarget, Category Scategory, int Scost, string StoolTip, int SturnShift, List<SkillAttr> Sattr = null, List<SkillCondition> Sconditions = null)
        {
            skillId = SskillId;
            name = SskillName;
            power = Spower;
            element = Selement;
            target = defualtTarget;
            category = Scategory;
            cost = Scost;
            toolTip = StoolTip;
            turnShift = SturnShift;
            if (Sattr == null)
            {
                attrs = new List<SkillAttr> { };
            }
            else
            {   attrs = new List<SkillAttr> { };
                for(int i = 0; i < Sattr.Count; i++)
                {
                    Attr(Sattr[i].ShallowCopy());
                }
            }
            if (Sconditions == null)
            {
                conditions = new List<SkillCondition> { };
            }
            else
            {
                conditions = new List<SkillCondition> { };
                conditions.AddRange(Sconditions);
            }
        }

        // Attempt at adding Attrs
        public Skill Attr(SkillAttr addedAttr)
        {
            attrs.Add(addedAttr);
            return this;
        }

        public Skill Condition(SkillCondition condition)
        {
            conditions.Add(condition);
            return this;
        }


        public void CheckSkillModConditions(UnitBody user)
        {
            foreach (SkillCondition condition in conditions)
            {
                if (condition.conditionMod == ConditionModifiers.Skill)
                {
                    condition.CheckConditionSkillMod(this,user);
                }
            }
        }




        void Start()
        {

        }


        void Update()
        {

        }

        public void ApplyEffects(UnitBody unitUser, List<UnitBody> unitTargets)
        {
            TurnOrderManager.Instance.turnOrder[0].turnShift = turnShift;
            for (int i = 0; i < attrs.Count; i++)
            {
                attrs[i].ActivateAttr(unitUser,unitTargets,power,element);
            }
        }


    }
}

