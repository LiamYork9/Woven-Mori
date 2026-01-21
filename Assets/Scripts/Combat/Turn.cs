using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using MoriSkills;
using System.Reflection;

[System.Serializable]
public class Turn
{
    public string name;
    public UnitBody unit;
    public int initiative;
    public int cycle;

    public int turnShift = 1;

    

    public void StartTurn()
    {
        // Checking Buff and Debuff timers, start of turn effects 
        if (unit.partyMember == true)
        {
            BattleManager.Instance.playerTurn = true;
        }
        BattleManager.Instance.StartStartTurnCo(this);
        unit.localTurnCount += 1;
        unit.AP += unit.APGain;

    }

    public void SelectAction()
    {
        BattleManager.Instance.StartSelectActionCo(this);
    }

    public void EndTurn()
    {
        if (BattleManager.Instance.fightState == FightState.Active)
        {
            BattleManager.Instance.StartEndTurnCo(this);
        }

        
        
       
    }

    public void PopulateTurn(UnitBody tempUnit)
    {
        unit = tempUnit;
        name = unit.name;
        initiative = unit.initiative;
        cycle = TurnOrderManager.Instance.cycle;
    }

    // Its just shootout dumbass 
    public IEnumerator StartTurnCo()
    {
        BattleManager.Instance.ButtonsOff();
        BattleManager.Instance.dialogueText.text = "Start " + unit.name + "'s Turn!";
        yield return new WaitForSeconds(1f);
        unit.StartOfTurn.Invoke();

        SelectAction();
      
    
       
    }

    public IEnumerator SelectActionCo()
    {
        BattleManager.Instance.dialogueText.text = "What will " + unit.name + " do?";
        yield return new WaitForSeconds(1f);
        BattleManager.Instance.dialogueText.text = "";
        if (BattleManager.Instance.playerTurn == true && BattleManager.Instance.attacking == false && BattleManager.Instance.usingSkill == false)
        {
            BattleManager.Instance.ButtonsOn();
        }
        else
        {
            BattleManager.Instance.ButtonsOff();
        }
    }

    public IEnumerator EnemyActionCo()
    {
        //Enemy AI Here !!!!!
        yield return new WaitForSeconds(0.5f);
        Skill Temp = EnemySelectSkill();
        List<UnitBody> tempTargets = EnemySkillTarget(Temp);
        BattleManager.Instance.EnemyAttack(Temp,tempTargets);
    }

    public IEnumerator EndTurnCo()
    {
        unit.EndOfTurn.Invoke();
        // Removing Timed out Buff and Debuffs, end of turn effects 
        TurnOrderManager.Instance.TurnShift(turnShift);
        BattleManager.Instance.dialogueText.text = " ";
        BattleManager.Instance.attacking = false;
        BattleManager.Instance.playerTurn = false;
        BattleManager.Instance.enemyTurn = false;
        yield return new WaitForSeconds(1f);
        BattleManager.Instance.TurnTransiton();
     
    }

    public Skill EnemySelectSkill()
    {
        int AP = TurnOrderManager.Instance.turnPlayer.AP;
        List<Skill> enemySkills = new List<Skill>();
        for(int i = 0; i < TurnOrderManager.Instance.turnPlayer.skills.Count; i++)
        {
            if(SkillMaker.Instance.GetById(TurnOrderManager.Instance.turnPlayer.skills[i]).cost <= AP)
            {
                enemySkills.Add(SkillMaker.Instance.GetById(TurnOrderManager.Instance.turnPlayer.skills[i]));
            }
        }
       int temp = Random.Range(-1,enemySkills.Count);
        if(temp == -1)
        {
            return SkillMaker.Instance.GetById(SkillId.Attack);
        }
        else
        {
            return enemySkills[temp];
        }
    }
    
    public List<UnitBody> EnemySkillTarget(Skill skill)
    {
        List<UnitBody> targets = new List<UnitBody>();
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
                        if(temp.maxHP-temp.currentHP>missingHP)
                        {
                            missingHP=temp.maxHP-temp.currentHP;
                            temptarget = i;
                        }
                    }
                    targets.Add(BattleManager.Instance.enemySlots[temptarget].GetComponent<UnitBody>());
                }
                else
                {
                   targets.Add(BattleManager.Instance.enemySlots[Random.Range(0,BattleManager.Instance.enemySlots.Count)].GetComponent<UnitBody>()); 
                }
                 break;
            
        }
        return targets;

    }
}
