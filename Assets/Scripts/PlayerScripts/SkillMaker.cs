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


    //public string SD4,SD9,SD10,SD11,SD14,SD15;
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
        Skill EndBringer = new Skill(SkillId.EndBringer, "End Bringer", Element.Fate, Target.single, Category.Attack, 100, 4, "This shit kills people", 0, 2).Attr(new DamageAttr(100,DamageType.Magic,Element.Fate)).Attr(new DamageAttr(10,DamageType.Magic,Element.Fate,true));
        madeSkills.Add(EndBringer);
        Skill SmallPunch = new Skill(SkillId.SmallPunch, "Small Punch", Element.None, Target.single, Category.Attack, 100, 1, "You punch them", 100, 1).Attr(new DamageAttr(10,DamageType.Magic));
        madeSkills.Add(SmallPunch);
        Skill StaticShock = new Skill(SkillId.StaticShock, "Static Shock", Element.Air, Target.mutipleEnemy, Category.Attack, 70, 2, "Power: 20 \nTargets: All Enemies \nElement: Air" , 0, 1).Attr(new DamageAttr(20,DamageType.Magic,Element.Air));
        madeSkills.Add(StaticShock);
        Skill Flex = new Skill(SkillId.Flex, "Flex", Element.None, Target.self, Category.Buff, 0, 1, "Get Ripped", 0, 1).Attr(new StatBoostConAttr(Stats.Attack, 10, 3, true)).Attr(new StatBoostConAttr(Stats.Defence, 2));
        madeSkills.Add(Flex);
        Skill Heal = new Skill(SkillId.Heal, "Heal", Element.None, Target.ally, Category.Support, 0, 1, "Restore some HP to an Ally", 0, 1).Attr(new HealAttr(10, .5f, false));
        madeSkills.Add(Heal);
        Skill Fortifiy = new Skill(SkillId.Fortifiy, "Fortifiy", Element.None, Target.party, Category.Buff, 0, 3,"Boosts the parties Defense",0,1).Attr(new StatBoostConAttr(Stats.Defence, 5));
        madeSkills.Add(Fortifiy);
        Skill PowerUp = new Skill(SkillId.PowerUp, "PowerUp",Element.None,Target.ally, Category.Buff,0,2,"Raise one allies Attack",0,2).Attr(new StatBoostConAttr(Stats.Attack, 5));
        madeSkills.Add(PowerUp);
        Skill BurnAway = new Skill(SkillId.BurnAway, "Burn Away",Element.Fire, Target.single,Category.Attack,0,2, "Power: 15 \nTargets: One Enemies \nElement: Fire \nLower targets Defense",0,1).Attr(new StatBoostConAttr(Stats.Defence, -5)).Attr(new DamageAttr(15,DamageType.Physical,Element.Fire));
        madeSkills.Add(BurnAway);
        Skill TriSlash = new Skill(SkillId.TriSlash, "Tri-Slash",Element.Fate,Target.single,Category.Attack,0,3, "Power: 10 \nTargets: One Enemie \nElement: Fire,Water,Earth \nThis attack hits 3 times each with a uniqe element",0,3).Attr(new DamageAttr(10,DamageType.Physical,Element.Fire)).Attr(new DamageAttr(10,DamageType.Physical,Element.Water)).Attr(new DamageAttr(10,DamageType.Physical,Element.Earth));
        madeSkills.Add(TriSlash);
        Skill FiredUp = new Skill(SkillId.FiredUp, "Fired Up", Element.Fire, Target.self, Category.Buff,0,3,"Targets: Self \nElement: Fire \nGreatly boost attack For 1 strike",0,0).Attr(new StatBoostConAttr(Stats.Attack, 20,2));
        madeSkills.Add(FiredUp);
        Skill Berserk = new Skill(SkillId.Berserk, "Berserk", Element.None, Target.single, Category.Attack,0,2,"Go wild and strike twice",0,2).Attr(new DamageAttr(20,DamageType.Physical)).Attr(new DamageAttr(20,DamageType.Physical)).Attr(new StatBoostConAttr(Stats.Defence, -5,3,true));
        madeSkills.Add(Berserk);
        Skill Rampage = new Skill(SkillId.Rampage, "Rampage", Element.None, Target.mutipleEnemy,Category.Attack,0,2,"Strike Mutiple Foes",0,1).Attr(new DamageAttr(10,DamageType.Physical));
        madeSkills.Add(Rampage);
        Skill CutLine = new Skill(SkillId.CutLine, "Cut Line", Element.Fate, Target.mutipleEnemy,Category.Debuff,0,3,"Targets: All Enemies \nElement: Fate \nLower all enimes Attack",0,-2).Attr(new StatBoostConAttr(Stats.Attack, -5));
        madeSkills.Add(CutLine);
        Skill HolyAbsorption = new Skill(SkillId.HolyAbsorption,"Holy Absorption",Element.Light,Target.single,Category.Attack,0,2,"Power: 25 \nTargets: One Enemie \nElement: Light \nAbsorb HP from the enemy",0,-1).Attr(new DamageAttr(25,DamageType.Magic,Element.Light)).Attr( new HealAttr(10,.2f,true));
        madeSkills.Add(HolyAbsorption);
        Skill EvenOdd = new Skill(SkillId.EvenOdd, "EvenOdd", Element.None, Target.single, Category.Attack, 100, 1, "On Even turns Heal and buff yourself, on Odd turns hit the enemy and debuff them", 100, -2).Attr(new EvenOddAttr().Odd(new DamageAttr(10,DamageType.Magic)).Odd(new StatBoostConAttr(Stats.Attack, -5)).Even(new HealAttr(10, .2f, true)).Even(new StatBoostConAttr(Stats.Attack, 5,3,true)).Even(new StatBoostConAttr(Stats.Defence,5,3,true)));
        madeSkills.Add(EvenOdd);

        //Enemy Skills
        Skill PoisonBite = new Skill(SkillId.PoisonBite,"Poison Bite",Element.Dark,Target.single,Category.Attack,100,2, "A weak Bite that poisons the target",100,1).Attr(new DamageAttr(10,DamageType.Physical,Element.Dark)).Attr(new ApplyPoisonAttr(3,3,10));
        madeSkills.Add(PoisonBite);
    }

    public Skill GetById(SkillId Id)
    {
        for (int i = 0; i < madeSkills.Count; i++)
        {
            if (madeSkills[i].skillId == Id)
            {
                return new Skill( madeSkills[i].skillId, madeSkills[i].name, madeSkills[i].element, madeSkills[i].target, madeSkills[i].category, madeSkills[i].accurcy, madeSkills[i].cost, madeSkills[i].toolTip, madeSkills[i].chance, madeSkills[i].turnShift, madeSkills[i].attrs, madeSkills[i].condtions);
            }
        }
        return null;
    }

    public void SetSkillD()
    {
        //SD4 = "Power: 20 \n Targets: All Enimes \n Element Air";
    }
}
