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
        EvenOdd,
        Fortifiy,
        PowerUp,
        BurnAway,
        TriSlash,
        FiredUp,
        Berserk,
        Rampage,
        CutLine,
        HolyAbsorption



        ,//Enemy Skills
        PoisonBite
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


    public class Condtion
    {

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

        public int accurcy;

        public int cost;

        public string toolTip;

        public int chance;

        public int turnShift;

        [SerializeReference]
        public List<SkillAttr> attrs;

        public List<Condtion> condtions;


        public Skill(SkillId SskillId, string SskillName, Element Selement, Target defualtTarget, Category Scategory, int Saccuracy, int Scost, string StoolTip, int Schance, int SturnShift, List<SkillAttr> Sattr = null, List<Condtion> Scondtions = null)
        {
            skillId = SskillId;
            name = SskillName;
            element = Selement;
            target = defualtTarget;
            category = Scategory;
            accurcy = Saccuracy;
            cost = Scost;
            toolTip = StoolTip;
            chance = Schance;
            turnShift = SturnShift;
            if (Sattr == null)
            {
                attrs = new List<SkillAttr> { };
            }
            else
            {
                attrs = Sattr;
            }
            if (Scondtions == null)
            {
                condtions = new List<Condtion> { };
            }
            else
            {
                condtions = Scondtions;
            }
        }

        // Attempt at adding Attrs
        public Skill Attr(SkillAttr addedAttr)
        {
            attrs.Add(addedAttr);
            return this;
        }







        void Start()
        {

        }


        void Update()
        {

        }

        public void ApplyEffects(UnitBody unitUser, UnitBody unitTarget)
        {
            TurnOrderManager.Instance.turnOrder[0].turnShift = turnShift;
            for (int i = 0; i < attrs.Count; i++)
            {
                attrs[i].ActivateAttr(unitUser,unitTarget);
            }
        }


    }
}

