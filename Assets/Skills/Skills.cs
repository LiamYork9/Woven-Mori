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
            StaticShock

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
            mutipleEnemy
        }

           public enum Category
        {
            Physical,
            Magic,
            Buff,
            Debuff
        }
    public class SkillAttr
    {

    }

    public class Condtion
    {

    }
    [System.Serializable]
    public class Skills
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

        public List<SkillAttr> attrs;

        public List<Condtion> condtions;

        public Skills(SkillId SskillId, string SskillName, Element Selement, Target defualtTarget, Category Scategory, int Spower, int Saccuracy, int Scost, string StoolTip, int Schance, int SturnShift, List<SkillAttr> Sattr = null, List<Condtion> Scondtions = null)
        {
            skillId = SskillId;
            name = SskillName;
            element = Selement;
            target = defualtTarget;
            category = Scategory;
            power = Spower;
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








        void Start()
        {

        }


        void Update()
        {

        }


    }
}

