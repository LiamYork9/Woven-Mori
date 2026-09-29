using System.Collections.Generic;
using MoriSkills;
using UnityEditor;
using UnityEngine;

[System.Serializable]


public struct TargetPreference
{
    public TargetingStyle target;

    public int weight;


}


[System.Serializable]
public struct CategoryPreference
{
    public Category category;

    public int weight;
}

[System.Serializable]
public struct ChosenCategory
{
    public Category chosenCategory;

    public List<TargetPreference> preferences;

   


}

[CreateAssetMenu(fileName = "UnitBrain2", menuName = "Scriptable Objects/EnemyBrain/T2")]
public class Tier2Brain: UnitBrain
{
    public List<CategoryPreference> categoryPref;

    public List<ChosenCategory> chosenCategory;
    
    public int useRandom;

    public int basicAttackChance;

    public TargetingStyle basicAttackStyle = TargetingStyle.Default;

    public override SkillUse Think(UnitBody unit)
    {
        SkillUse temp = new SkillUse
        {
            user = unit
        };
        if (TurnOrderManager.Instance.turnOrder[0].fated)
        {
            FateCode? tempFate = CheckFate();
            if(tempFate!=null && tempFate.Value.skillId!=SkillId.None)
            {
                SkillId tempId = tempFate.Value.skillId;
                Skill tempSkill = SkillMaker.Instance.GetById(tempId).CheckSkillModConditions(unit);
                tempSkill.style = tempFate.Value.newTargeting;
                temp.skill = tempSkill;
                temp.freeSkill = tempFate.Value.isFree;
            }
        }
            
        if(temp.skill == null)
        {
            temp.skill = SelectSkill(unit);
            temp.freeSkill=false;
        }
        temp.targets=SkillTarget(temp.skill,unit);
        return temp;
    } 


    public override Skill SelectSkill(UnitBody user)
    {
        Category cat = DecideIntent(categoryPref,useRandom);

        TargetingStyle style = ChosenStyle(cat);

        Target tar = Target.single;

        switch (style)
        {
            case TargetingStyle.LowestHealthEnemy:
            tar = Target.single;
            break;
            case TargetingStyle.HighestHealthEnemy:
            tar = Target.single;
            break;
            case TargetingStyle.LowestHealthAlly:
            tar = Target.ally;
            break;
            case TargetingStyle.HighestHealthAlly:
            tar = Target.ally;
            break;
            case TargetingStyle.EnemyTeam:
            tar = Target.mutipleEnemy;
            break;
            case TargetingStyle.AllyTeam:
            tar = Target.party;
            break;
            case TargetingStyle.Self:
            tar = Target.self;
            break;
            case TargetingStyle.Default:
            tar = Target.single;
            break;
            case TargetingStyle.DefaultAlly:
            tar = Target.ally;
            break;
            case TargetingStyle.Basic:
            Debug.Log("Basic Targeting");
            Skill attack =SkillMaker.Instance.GetById(SkillId.Attack);
            attack.style = TargetingStyle.Default;
            return attack;

            case TargetingStyle.All:
            tar = Target.mutipleEnemy;
            break;

            default:
            tar = Target.single;
            break;
        }





        List<Skill> tempSkills = new List<Skill>();
        for(int i = 0; i < user.skills.Count; i++)
        {
            Skill tempSkill = SkillMaker.Instance.GetById(user.skills[i]).CheckSkillModConditions(user);
            if(tempSkill.cost <= user.AP && tempSkill.category == cat&& tempSkill.target == tar)
            {
                tempSkills.Add(tempSkill);
            }
        }
        if(tempSkills.Count == 0)
        {
            Skill attack =SkillMaker.Instance.GetById(SkillId.Attack);
            attack.style = basicAttackStyle;
            Debug.Log("No Valid Skill");
            return attack;
        }
        else if(basicAttackChance > Random.Range(0,100))
        {
            Skill attack =SkillMaker.Instance.GetById(SkillId.Attack);
            attack.style = basicAttackStyle;
            Debug.Log("Rolled Basic Attack Chance");
            return attack;
        }

        int temp = Random.Range(0,tempSkills.Count);
        tempSkills[temp].style = style;
        Debug.Log("Using Skill: "+tempSkills[temp].name);
        return tempSkills[temp];
    }

    public virtual Category DecideIntent(List<CategoryPreference> categories , int random = 0)
    {
        List<CategoryPreference> tempList = new List<CategoryPreference>();
        tempList.AddRange(categories);
        int temp = 0 ;
        foreach(CategoryPreference  i in categories)
        {
            temp += i.weight;

        }
        temp =  Random.Range(0,temp+random);
        
        while(tempList.Count > 0 && temp > tempList[0].weight)
        {
            temp -= tempList[0].weight;
            tempList.RemoveAt(0);
        }

        if(tempList.Count > 0)
        {
            Debug.Log( tempList[0].category);
            return tempList[0].category;
        }

        Debug.Log("Random");
        return (Category)Random.Range(0,3);

        

        
    }

    public TargetingStyle ChosenStyle(Category category)
    {
        ChosenCategory? tempCat = null;
        foreach(ChosenCategory i in chosenCategory)
        {
            if(i.chosenCategory == category)
            {
                tempCat = i;
                break;
            }
        }

        if(tempCat == null)
        {
            Debug.Log("No Category");
            return TargetingStyle.Basic;
        }
        ChosenCategory tempCat2 = tempCat.Value;
        List<TargetPreference> tempList = new List<TargetPreference>();
        tempList.AddRange(tempCat2.preferences);
        int temp = 0 ;
        foreach(TargetPreference  i in tempList)
        {
            temp += i.weight;

        }
        temp =  Random.Range(0,temp);
        
        while(tempList.Count > 0 && temp > tempList[0].weight)
        {
            temp -= tempList[0].weight;
            tempList.RemoveAt(0);
        }

        if(tempList.Count > 0)
        {
            Debug.Log("Target Style: " + tempList[0].target);
            return tempList[0].target;
        }
        
        return TargetingStyle.Basic;


    }

    public override List<UnitBody> SkillTarget(Skill skill, UnitBody user)
    {
        List<UnitBody> targets = new List<UnitBody>();
        if(!user.partyMember)
        {
            switch (skill.style)
            {
                case TargetingStyle.Default:
                targets.Add(BattleManager.Instance.playerSlots[Random.Range(0,BattleManager.Instance.playerSlots.Count)].GetComponent<UnitBody>());
                break;
                case TargetingStyle.DefaultAlly:
                    if(skill.category== Category.Heal)
                    {
                        int temptarget = 0;
                        int missingHP = 0;
                        for( int i = 0; i<BattleManager.Instance.enemySlots.Count; i++)
                        {   
                            UnitBody temp = BattleManager.Instance.enemySlots[i].GetComponent<UnitBody>();
                            if(temp.activeStats.MaxHP-temp.activeStats.CurrentHP>missingHP)
                            {
                                missingHP=temp.activeStats.MaxHP-temp.activeStats.CurrentHP;
                                temptarget = i;
                            }
                        }
                        if (missingHP!=0)
                        {
                            targets.Add(BattleManager.Instance.enemySlots[temptarget].GetComponent<UnitBody>());
                        }
                        else
                        {
                            targets.Add(BattleManager.Instance.enemySlots[Random.Range(0,BattleManager.Instance.enemySlots.Count)].GetComponent<UnitBody>()); 
                        }
                    }
                    else
                    {
                    targets.Add(BattleManager.Instance.enemySlots[Random.Range(0,BattleManager.Instance.enemySlots.Count)].GetComponent<UnitBody>()); 
                    }
                    break;
                case TargetingStyle.Self:
                    targets.Add(TurnOrderManager.Instance.turnPlayer);
                    break;
                case TargetingStyle.AllyTeam:
                    for( int i = 0; i < BattleManager.Instance.enemySlots.Count ; i++)
                    {
                        targets.Add(BattleManager.Instance.enemySlots[i].GetComponent<UnitBody>());
                    }
                    break;
                case TargetingStyle.EnemyTeam:
                    for( int i = 0; i < BattleManager.Instance.playerSlots.Count ; i++)
                    {
                        targets.Add(BattleManager.Instance.playerSlots[i].GetComponent<UnitBody>());
                    }
                    break;

                case TargetingStyle.LowestHealthAlly:
                    UnitBody templowA = BattleManager.Instance.enemySlots[0].GetComponent<UnitBody>();
                    foreach(GameObject i in BattleManager.Instance.enemySlots)
                    {
                        if(i.GetComponent<UnitBody>().activeStats.CurrentHP < templowA.activeStats.CurrentHP)
                        {
                            templowA = i.GetComponent<UnitBody>();
                        }
                        else if(i.GetComponent<UnitBody>().activeStats.CurrentHP == templowA.activeStats.CurrentHP)
                        {
                            int flip = Random.Range(0,2);
                            if (flip == 0)
                            {
                                templowA = i.GetComponent<UnitBody>();
                            }
                        }
                    }
                    targets.Add(templowA);
                  
                    break;

                case TargetingStyle.HighestHealthAlly:
                    UnitBody temphighA = BattleManager.Instance.enemySlots[0].GetComponent<UnitBody>();
                    foreach(GameObject i in BattleManager.Instance.enemySlots)
                    {
                        if(i.GetComponent<UnitBody>().activeStats.CurrentHP > temphighA.activeStats.CurrentHP)
                        {
                            temphighA = i.GetComponent<UnitBody>();
                        }
                        else if(i.GetComponent<UnitBody>().activeStats.CurrentHP == temphighA.activeStats.CurrentHP)
                        {
                            int flip = Random.Range(0,2);
                            if (flip == 0)
                            {
                               temphighA = i.GetComponent<UnitBody>();
                            }
                        }
                    }
                    targets.Add(temphighA);

                    break;

                     case TargetingStyle.LowestHealthEnemy:
                    UnitBody templowE = BattleManager.Instance.playerSlots[0].GetComponent<UnitBody>();
                    foreach(GameObject i in BattleManager.Instance.playerSlots)
                    {
                        if(i.GetComponent<UnitBody>().activeStats.CurrentHP < templowE.activeStats.CurrentHP)
                        {
                            templowE = i.GetComponent<UnitBody>();
                        }
                        else if(i.GetComponent<UnitBody>().activeStats.CurrentHP == templowE.activeStats.CurrentHP)
                        {
                            int flip = Random.Range(0,2);
                            if (flip == 0)
                            {
                                templowE = i.GetComponent<UnitBody>();
                            }
                        }
                    }
                    targets.Add(templowE);
                  
                    break;

                case TargetingStyle.HighestHealthEnemy:
                    UnitBody temphighE = BattleManager.Instance.playerSlots[0].GetComponent<UnitBody>();
                    foreach(GameObject i in BattleManager.Instance.playerSlots)
                    {
                        if(i.GetComponent<UnitBody>().activeStats.CurrentHP > temphighE.activeStats.CurrentHP)
                        {
                            temphighE = i.GetComponent<UnitBody>();
                        }
                        else if(i.GetComponent<UnitBody>().activeStats.CurrentHP == temphighE.activeStats.CurrentHP)
                        {
                            int flip = Random.Range(0,2);
                            if (flip == 0)
                            {
                               temphighE = i.GetComponent<UnitBody>();
                            }
                        }
                    }
                    targets.Add(temphighE);
                    

                    break;

                case TargetingStyle.All:
                    for( int i = 0; i < BattleManager.Instance.enemySlots.Count ; i++)
                    {
                        targets.Add(BattleManager.Instance.enemySlots[i].GetComponent<UnitBody>());
                    }
                    for( int i = 0; i < BattleManager.Instance.playerSlots.Count ; i++)
                    {
                        targets.Add(BattleManager.Instance.playerSlots[i].GetComponent<UnitBody>());
                    }

                    break;
                
                case TargetingStyle.Basic:
                    Debug.LogError("Basic");
                   break; 

                default:
                Debug.LogError("FUCK");
                    targets.Add(BattleManager.Instance.playerSlots[Random.Range(0,BattleManager.Instance.playerSlots.Count)].GetComponent<UnitBody>());
                    break;


                
                
            }

        }
        else
        {
            
        }
        return targets;
    }
}
