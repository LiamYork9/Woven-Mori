using System.Collections.Generic;
using MoriSkills;
using UnityEngine;


[System.Serializable]
public enum TargetingStyle
{
    LowestHealthEnemy,
    HighestHealthEnemy,
    
    LowestHealthAlly,
    HighestHealthAlly,

    EnemyTeam,
    AllyTeam,

    self,
    All
}
[System.Serializable]
public struct SkillUse
{
    public Skill skill;
    public UnitBody user;
    public bool freeSkill;
    public List<UnitBody> targets;

}


[System.Serializable]
public struct FateCode
{
    public string code;
    public SkillId skillId;
    public bool targetingOverride;
    public Target newTargeting;
    public bool isFree;

}
[CreateAssetMenu(fileName = "UnitBrain", menuName = "Scriptable Objects/EnemyBrain/T1")]
public class UnitBrain : ScriptableObject
{
    public List<FateCode> fates;
    public virtual SkillUse Think(UnitBody unit)
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
                Skill tempSkill = SkillMaker.Instance.GetById(tempId);
                temp.skill = tempSkill;
                temp.freeSkill = tempFate.Value.isFree;
            }
        }
            
        if(temp.skill == null)
        {
            temp.skill = SelectSkill(unit);
            temp.freeSkill=false;
        }
        temp.skill.CheckSkillModConditions(unit);
        temp.targets=SkillTarget(temp.skill,unit);
        return temp;
    } 
    public FateCode? CheckFate()
    {
        string temp = TurnOrderManager.Instance.turnOrder[0].fate;
        foreach(FateCode fate in fates)
        {
            if (fate.code==temp)
            {
                return fate;
            }
        }
        
        return null;
    }

    public virtual Skill SelectSkill(UnitBody user)
    {
        List<Skill> tempSkills = new List<Skill>();
        for(int i = 0; i < user.skills.Count; i++)
        {
            if(SkillMaker.Instance.GetById(user.skills[i]).cost <= user.AP)
            {
                tempSkills.Add(SkillMaker.Instance.GetById(user.skills[i]));
            }
        }
        int temp = Random.Range(-1,tempSkills.Count);
        if(temp == -1)
        {
            return SkillMaker.Instance.GetById(SkillId.Attack);
        }
        else
        {
            return tempSkills[temp];
        }
    }

    public virtual List<UnitBody> SkillTarget(Skill skill, UnitBody user)
    {
        List<UnitBody> targets = new List<UnitBody>();
        if(!user.partyMember)
        {
            switch (skill.target)
            {
                case Target.single:
                targets.Add(BattleManager.Instance.playerSlots[Random.Range(0,BattleManager.Instance.playerSlots.Count)].GetComponent<UnitBody>());
                    break;
                case Target.mutipleEnemy:
                for( int i = 0; i < BattleManager.Instance.playerSlots.Count ; i++)
                    {
                        targets.Add(BattleManager.Instance.playerSlots[i].GetComponent<UnitBody>());
                    }
                    break;
                case Target.self:
                    targets.Add(TurnOrderManager.Instance.turnPlayer);
                    break;
                case Target.party:
                    for( int i = 0; i < BattleManager.Instance.enemySlots.Count ; i++)
                    {
                        targets.Add(BattleManager.Instance.enemySlots[i].GetComponent<UnitBody>());
                    }
                    break;
                case Target.ally:
                    bool heals = false;
                    for( int i = 0; i<skill.attrs.Count; i++)
                    {
                        if(skill.attrs[i] is HealAttr && !skill.attrs[i].targetSelf)
                        {
                            heals = true;
                        }
                    }
                    if(heals)
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
                
            }
        }
        else
        {
            
        }
        return targets;
    }
}
