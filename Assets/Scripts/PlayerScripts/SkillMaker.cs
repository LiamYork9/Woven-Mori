using UnityEngine;
using System.Collections.Generic;
using System;
using MoriSkills;
using System.Data.Common;
using UnityEngine.Rendering;

public class SkillMaker : MonoBehaviour
{
    string Name;
    int Power;
    int Cost;
    int Shift;
    string Description;

     public static SkillMaker Instance;
     public int standardBoost = 20;

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
        
        
        Skill Attack = new Skill(
            /*Stats*/
                SkillId.Attack,
                Name = "Attack",
                Power = 1, 
                Element.None, 
                Target.single, 
                Category.Attack,
                Cost = 0,   
                Description = "Your basic attack", 
                Shift = 1
            )
            /*Attrs*/
                .Attr(new LevelScaleAttr(1)
                .Attr(new DamageAttr(1,DamageType.Physical)));
        madeSkills.Add(Attack);
        
        
        Skill EndBringer = new Skill(SkillId.EndBringer, "End Bringer",100 , Element.Fate, Target.single, Category.Attack, 4, "This shit kills people", 2)
        .Attr(new DamageAttr(1,DamageType.Magic,Element.Fate,false,DamageAfterEffect.Recoil, 0.1f));
        madeSkills.Add(EndBringer);
        
        
        Skill SmallPunch = new Skill(SkillId.SmallPunch, "Small Punch",5 , Element.None, Target.single, Category.Attack, 1, "You punch them", 2)
        .Attr(new DamageAttr(1,DamageType.Magic));
        madeSkills.Add(SmallPunch);
       
       
        Skill StaticShock = new Skill(SkillId.StaticShock, "Static Shock",20, Element.Air, Target.mutipleEnemy, Category.Attack, 2, "Power: 20 \nTargets: All Enemies \nElement: Air" , 1)
        .Attr(new DamageAttr(1,DamageType.Magic,Element.Air));
        madeSkills.Add(StaticShock);
        
        
        Skill WindBlade = new Skill(SkillId.WindBlade, "Wind Blade",20, Element.Air, Target.single, Category.Attack, 1, "Power: 20 \nTargets: One Enemy \nElement: Air" , 1)
        .Attr(new DamageAttr(1,DamageType.Magic,Element.Air));
        madeSkills.Add(WindBlade);
        
        
        Skill Flex = new Skill(SkillId.Flex, "Flex",0 , Element.None, Target.self, Category.Buff,  1, "Get Ripped",    1)
        .Attr(new StatBoostConAttr(Stats.Attack|Stats.Defense, standardBoost, 3, true));
        madeSkills.Add(Flex);
        
        
        Skill Heal = new Skill(SkillId.Heal, "Heal",10, Element.None, Target.ally, Category.Support,  2, "Restore some HP to an Ally",  -1)
        .Attr(new HealAttr(1, .5f, false));
        madeSkills.Add(Heal);
        
        
        Skill MassHeal = new Skill(SkillId.MassHeal, "Mass Heal",10, Element.None, Target.party, Category.Support,  3, "Restore some HP to all Allies",  -2)
        .Attr(new HealAttr(1, .5f, false));
        madeSkills.Add(MassHeal);
        
        
        Skill Fortifiy = new Skill(SkillId.Fortifiy, "Fortifiy",0 , Element.None, Target.party, Category.Buff,  3,"Boosts the parties Defenses",1)
        .Attr(new StatBoostConAttr(Stats.Defense|Stats.mDefense, standardBoost));
        madeSkills.Add(Fortifiy);
        
        
        Skill PowerUp = new Skill(SkillId.PowerUp, "PowerUp",0 ,Element.None,Target.ally, Category.Buff,2,"Raise one allies Attack",2)
        .Attr(new StatBoostConAttr(Stats.Attack, standardBoost));
        madeSkills.Add(PowerUp);
        
        
        Skill BurnAway = new Skill(SkillId.BurnAway, "Burn Away",15 ,Element.Fire, Target.single,Category.Attack,2, "Power: 15 \nTargets: One Enemies \nElement: Fire \nLower targets Defense",1)
        .Attr(new StatDropConAttr(Stats.Defense, standardBoost)).Attr(new DamageAttr(1,DamageType.Physical,Element.Fire));
        madeSkills.Add(BurnAway);
        
        
        Skill TriSlash = new Skill(SkillId.TriSlash, "Tri-Slash",10 ,Element.Fate,Target.single,Category.Attack,3, "Power: 10 \nTargets: One Enemie \nElement: Fire,Water,Earth \nThis attack hits 3 times each with a uniqe element",3)
        .Attr(new DamageAttr(1,DamageType.Physical,Element.Fire,true))
        .Attr(new DamageAttr(1,DamageType.Physical,Element.Water,true))
        .Attr(new DamageAttr(1.5f,DamageType.Physical,Element.Earth,true));
        madeSkills.Add(TriSlash);
        
        
        Skill FiredUp = new Skill(SkillId.FiredUp, "Fired Up",0 , Element.Fire, Target.self, Category.Buff,3,"Targets: Self \nElement: Fire \nGreatly boost attack For 1 strike",0)
        .Attr(new StatBoostConAttr(Stats.Attack, standardBoost,2));
        madeSkills.Add(FiredUp);
        
        
        Skill PoisonBlade = new Skill(SkillId.PoisonBlade,"Poison Blade",10,Element.Dark,Target.single,Category.Attack,1, "You Coat your blade in poison and stab the target",1)
        .Attr(new DamageAttr(1,DamageType.Physical,Element.Dark))
        .Attr(new ApplyPoisonAttr(3,2,10));
        madeSkills.Add(PoisonBlade);
        
        
        Skill Berserk = new Skill(SkillId.Berserk, "Berserk",20 , Element.None, Target.single, Category.Attack,2,"Go wild and strike twice",2)
        .Attr(new DamageAttr(1,DamageType.Physical))
        .Attr(new DamageAttr(1,DamageType.Physical))
        .Attr(new StatDropConAttr(Stats.Defense, standardBoost,3,true));
        madeSkills.Add(Berserk);
        
        
        Skill Rampage = new Skill(SkillId.Rampage, "Rampage",10 , Element.None, Target.mutipleEnemy,Category.Attack,2,"Strike Mutiple Foes",2)
        .Attr(new DamageAttr(1,DamageType.Physical));
        madeSkills.Add(Rampage);
        
        
        Skill CutLine = new Skill(SkillId.CutLine, "Cut Line",0 , Element.Fate, Target.mutipleEnemy,Category.Debuff,3,"Targets: All Enemies \nElement: Fate \nLower all enimes Attack",-2)
        .Attr(new StatDropConAttr(Stats.Attack, standardBoost));
        madeSkills.Add(CutLine);
        
        
        Skill HolyAbsorption = new Skill(SkillId.HolyAbsorption,"Holy Absorption",25 ,Element.Light,Target.single,Category.Attack,2,"Power: 25 \nTargets: One Enemie \nElement: Light \nAbsorb HP from the enemy",-1)
        .Attr(new DamageAttr(1,DamageType.Magic,Element.Light,false,DamageAfterEffect.Lifesteal,0.25f));
        madeSkills.Add(HolyAbsorption);
        
        
        Skill SwordShield = new Skill(SkillId.SwordShield, "Sword&Shield",10 , Element.None, Target.single, Category.Attack, 1, "On Even turns Heal and buff yourself, on Odd turns hit the enemy and debuff them", -2)
        .Condition(new EvenOddCondition(
            /*Even Stats*/ 
                20 , Element.None, Target.self, Category.Buff, 1, 1, 
                
            /*Odd Stats*/
                10 , Element.None, Target.single, Category.Attack, 1, 1)
            
            /*Even Attrs*/
                .Even(new HealAttr(1, .2f))
                .Even(new StatBoostConAttr(Stats.Defense, standardBoost,3))
            /*Odd Attrs*/
                .Odd(new DamageAttr(1,DamageType.Magic))
                .Odd(new StatDropConAttr(Stats.Defense, standardBoost))
                .Odd(new StatDropConAttr(Stats.Attack, standardBoost)));
        madeSkills.Add(SwordShield);

        Skill HopeDespair = new Skill(SkillId.HopeDespair, "Hope&Despair",10 , Element.None, Target.single, Category.Buff, 1, "On Even turns lower all enemies Atk, Def, MDef for 3 turns, on Odd turns Raise all parties Atk, Def, MDef for 3 turns", -2)
        .Condition(new EvenOddCondition(
            /*Even Stats*/ 
                20 , Element.None, Target.party, Category.Buff, 3, 2, 
                
            /*Odd Stats*/
                10 , Element.None, Target.mutipleEnemy, Category.Buff, 3, -2)
            
            /*Even Attrs*/
                .Even(new StatDropConAttr(Stats.Attack, standardBoost,3))
                .Even(new StatDropConAttr(Stats.Defense, standardBoost,3))
                .Even(new StatDropConAttr(Stats.mDefense, standardBoost,3))
            /*Odd Attrs*/
                .Odd(new StatBoostConAttr(Stats.Attack, standardBoost,3))
                .Odd(new StatBoostConAttr(Stats.Defense, standardBoost,3))
                .Odd(new StatBoostConAttr(Stats.mDefense, standardBoost,3)));
        madeSkills.Add(HopeDespair);

        Skill FlameFlood = new Skill(SkillId.FlameFlood, "Flame&Flood",10 , Element.None, Target.single, Category.Buff, 1, "On Even turns Deal  Water Damage to all enemies, on Odd turns Deal Fire Damage to one enemy and burn them", -2)
        .Condition(new EvenOddCondition(
            /*Even Stats*/ 
                10 , Element.Water, Target.mutipleEnemy, Category.Attack, 2, 2, 
                
            /*Odd Stats*/
                20 , Element.Fire, Target.single, Category.Attack, 2, 1)
            
            /*Even Attrs*/
            .Even(new DamageAttr(1,DamageType.Magic))
               
            /*Odd Attrs*/
            .Odd(new DamageAttr(1,DamageType.Magic))
            .Odd(new ApplyBurn(3)));
               
        madeSkills.Add(FlameFlood);

        Skill HighLow = new Skill(SkillId.HighLow, "HighLow",10,Element.None,Target.single,Category.Buff,1,"",2)
        .Condition(new HighLowCondition(
            /*High Stats */
            10 , Element.Air, Target.mutipleEnemy, Category.Attack, 1, 2, 

            /*Low Stats */
            20 , Element.Earth, Target.single, Category.Attack, 1, 1, 5)

            /*High Attrs*/
            .High(new DamageAttr(1,DamageType.Magic))

            /*Low Attrs*/
            .Low(new DamageAttr(1,DamageType.Magic)));

         madeSkills.Add(HighLow);
        
        Skill Focus = new Skill(SkillId.Focus, "Focus",10 , Element.None, Target.self, Category.Attack, 0,  "Focus and gain an additional AP",  1)
        .Attr(new APGainAttr(1,true));
        madeSkills.Add(Focus);
        
        
        Skill SpeedUp = new Skill(SkillId.SpeedUp,"Speed Up",0,Element.None,Target.self,Category.Buff,1,"Boost your Priority",1)
        .Attr(new PriorityAttr(2,3));
        madeSkills.Add(SpeedUp);

        //Enemy Skills
        Skill PoisonBite = new Skill(SkillId.PoisonBite,"Poison Bite",10,Element.Dark,Target.single,Category.Attack,2, "A weak Bite that poisons the target",1)
        .Attr(new DamageAttr(1,DamageType.Physical,Element.Dark))
        .Attr(new ApplyPoisonAttr(3,3,10));
        madeSkills.Add(PoisonBite);

        Skill Quake = new Skill(SkillId.Quake,"Quake",10,Element.Earth,Target.single,Category.Attack,1, "An attack that locks the target in a tremor",1)
        .Attr(new DamageAttr(1,DamageType.Physical,Element.Dark))
        .Attr(new ApplyTremorLTC(false));
        madeSkills.Add(Quake);

        Skill Erosion = new Skill(SkillId.Erosion,"Erosion",10,Element.Earth,Target.single,Category.Attack,1, "A spell that accelerates time on the target triggering any end turn effects, counting down all conditions, and dropping their Defense",1)
        .Attr(new PassageOfTimeAttr(3))
        .Attr(new StatDropConAttr(Stats.Defense, standardBoost, 3));
        madeSkills.Add(Erosion);
    }

    public Skill GetById(SkillId Id)
    {
        for (int i = 0; i < madeSkills.Count; i++)
        {
            if (madeSkills[i].skillId == Id)
            {
                return new Skill( madeSkills[i].skillId, madeSkills[i].name, madeSkills[i].power, madeSkills[i].element, madeSkills[i].target, madeSkills[i].category, madeSkills[i].cost, madeSkills[i].toolTip, madeSkills[i].turnShift, madeSkills[i].attrs, madeSkills[i].conditions);
            }
        }
        return null;
    }

    public void SetSkillD()
    {
        //SD4 = "Power: 20 \n Targets: All Enimes \n Element Air";
    }
}
