using UnityEngine;
using System.Collections.Generic;
using System;
using MoriSkills;
using UnityEngine.Rendering;

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
        Skill Attack = new Skill(SkillId.Attack, "Attack", Element.None, Target.single, Category.Attack, 100, 0, "Your basic attack", 0, 1).Attr(new LevelScaleAttr(1).Attr(new DamageAttr(0,DamageType.Physical)));
        madeSkills.Add(Attack);
        Skill EndBringer = new Skill(SkillId.EndBringer, "End Bringer", Element.Fate, Target.single, Category.Attack, 100, 5, "This shit kills people", 0, 2).Attr(new DamageAttr(100,DamageType.Magic,Element.Fate));
        madeSkills.Add(EndBringer);
        Skill SmallPunch = new Skill(SkillId.SmallPunch, "Small Punch", Element.None, Target.single, Category.Attack, 100, 1, "You punch them", 100, 1).Attr(new DamageAttr(10,DamageType.Magic));
        madeSkills.Add(SmallPunch);
        Skill StaticShock = new Skill(SkillId.StaticShock, "Static Shock", Element.Air, Target.mutipleEnemy, Category.Attack, 70, 1, "Hit mutiple guys with lighting", 0, 1).Attr(new DamageAttr(20,DamageType.Magic,Element.Air));
        madeSkills.Add(StaticShock);
        Skill Flex = new Skill(SkillId.Flex, "Flex", Element.None, Target.self, Category.Buff, 0, 1, "Get Ripped", 0, 1).Attr(new StatBoostConAttr(Stats.Attack, 10, true)).Attr(new StatBoostConAttr(Stats.Defence, 2));
        madeSkills.Add(Flex);
        Skill Heal = new Skill(SkillId.Heal, "Heal", Element.None, Target.ally, Category.Support, 0, 1, "Restore some HP", 0, 1).Attr(new HealAttr(10, false));
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
