using System.Collections.Generic;
using MoriSkills;
using UnityEngine;

public enum TargetGroup
{
    LowHP,
    HighHP,
    MissingHPPercent,   
}

[CreateAssetMenu(fileName = "UnitBrain2", menuName = "Scriptable Objects/EnemyBrain/T2")]
public class Tier2Brain: UnitBrain
{
    public List<Category> preferredCategory;
    public int weightCategory;
    public List<Target> preferredTargetMethod;
    public int weightMethod;
    public List<TargetGroup> preferredTargetGroup;
    public int weightGroup;


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


    public override Skill SelectSkill(UnitBody user)
    {
        List<Skill> tempSkills = new List<Skill>();
        for(int i = 0; i < user.skills.Count; i++)
        {
            Skill tempSkill = SkillMaker.Instance.GetById(user.skills[i]).CheckSkillModConditions(user);
            if(tempSkill.cost <= user.AP)
            {
                
                tempSkills.Add(tempSkill);
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

    public virtual Category DecideIntent(int attack, int heal, int status, int random = 0)
    {
        int temp = Random.Range(0,attack+heal+status+random);

        if(temp<attack)
        {
            return Category.Attack;
        }
        else
        {
            temp-=attack;
        }
        if(temp<heal)
        {
            return Category.Heal;
        }
        else
        {
            temp -= heal;
        }
        if(temp<status)
        {
            return Category.Status;
        }
        else
        {
            temp -= status;
        }
        if(temp<random)
        {
            return (Category)Random.Range(0,3);
        }

        return Category.Attack;
    }
}
