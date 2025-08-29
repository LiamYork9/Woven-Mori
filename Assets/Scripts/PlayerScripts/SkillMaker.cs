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

    public List<Skills> madeSkills;
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
        Skills EndBringer = new Skills(SkillId.EndBringer, "End Bringer", Element.Fate, Target.single, Category.Magic, 100, 100, 5, "This shit kills people", 0, 2);
        madeSkills.Add(EndBringer);
        Skills SmallPunch = new Skills(SkillId.SmallPunch, "Small Punch", Element.None, Target.single, Category.Physical, 10, 100, 1, "You punch them", 100, 1);
        madeSkills.Add(SmallPunch);
        Skills StaticShock = new Skills(SkillId.StaticShock, "Static Shock", Element.Air, Target.mutipleEnemy, Category.Magic, 40, 70, 4, "Hit mutiple guys with lighting", 0, 1);
        madeSkills.Add(StaticShock);
    }

    public Skills GetById(SkillId Id)
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
