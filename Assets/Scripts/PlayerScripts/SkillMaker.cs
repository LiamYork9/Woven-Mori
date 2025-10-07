using UnityEngine;
using System.Collections.Generic;
using System;
using MoriSkills;

public class SkillMaker : MonoBehaviour
{
     public static SkillMaker Instance;

    public static SkillMaker GetInstance()
    {
        return Instance;
    }



    public void Awake()
    {
        if (SkillMaker.Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public List<Skill> madeSkills;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SkillCreation();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SkillCreation()
    {
        Skill EndBringer = new Skill(SkillId.EndBringer, "End Bringer", Element.Fate, Target.single, Category.Magic, 100, 100, 5, "This shit kills people", 0, 2);
        madeSkills.Add(EndBringer);
        Skill SmallPunch = new Skill(SkillId.SmallPunch, "Small Punch", Element.None, Target.single, Category.Physical, 10, 100, 1, "You punch them", 100, 1);
        madeSkills.Add(SmallPunch);
        Skill StaticShock = new Skill(SkillId.StaticShock, "Static Shock", Element.Air, Target.mutipleEnemy, Category.Magic, 40, 70, 1, "Hit mutiple guys with lighting", 0, 1);
        madeSkills.Add(StaticShock);
        Skill Flex = new Skill(SkillId.Flex, "Flex", Element.None, Target.self, Category.Buff, 0, 0, 1, "Get Ripped", 0, 1).Attr(new StatBoostAttr(Stats.Attack, 10, true)).Attr(new StatBoostAttr(Stats.Defence, 2));
        madeSkills.Add(Flex);
        Skill Heal = new Skill(SkillId.Heal, "Heal", Element.None, Target.single, Category.Magic, 0, 0, 1, "Restore some HP", 0, 1).Attr(new HealAttr(10,true));
        madeSkills.Add(Heal);
    }

    public Skill GetById(SkillId Id)
    {
        for (int i = 0; i < madeSkills.Count; i++)
        {
            if (madeSkills[i].skillId == Id)
            {
                return madeSkills[i];
            }
        }
        return null;
    }
}
