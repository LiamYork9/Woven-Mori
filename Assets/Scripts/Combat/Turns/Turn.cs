using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using MoriSkills;

[System.Serializable]
public class Turn
{
    public string name;
    public UnitBody unit;
    public int visited;
    public int exhaustValue = 2;
    public bool exhausted;
    public bool fated;
    public string fate;

    public int turnShift = 1;

    

    public virtual void StartTurn()
    {
        // Checking Buff and Debuff timers, start of turn effects 
        if (unit.partyMember == true)
        {
            BattleManager.Instance.playerTurn = true;
        }
        visited ++;
        if(visited>= exhaustValue)
        {
            exhausted = true;
        }
        BattleManager.Instance.StartStartTurnCo(this);
        unit.localTurnCount += 1;
        unit.AP += unit.activeStats.APGain;

    }

    public virtual void SelectAction()
    {
        BattleManager.Instance.StartSelectActionCo(this);
    }

    public virtual void EndTurn()
    {
        if (BattleManager.Instance.fightState == FightState.Active)
        {
            BattleManager.Instance.StartEndTurnCo(this);
        }

        
        
       
    }

    public void PopulateTurn(UnitBody tempUnit, bool isFated = false, string fateCode = "")
    {
        unit = tempUnit;
        name = unit.name;
        fated = isFated;
        fate = fateCode;
    }

    // Its just shootout dumbass 
    public virtual IEnumerator StartTurnCo()
    {
        Vector3 temp = new Vector3(100.0f,0,0);
        if(unit.partyMember == false)
        {
            temp*=-1;
            
        }
        unit.gameObject.transform.position += temp;
        
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
        yield return new WaitForSeconds(0.5f);
        //Enemy AI Here !!!!!
        SkillUse temp = unit.unit.brain.Think(unit);
        BattleManager.Instance.EnemyAttack(temp.skill,temp.targets);
    }

    public virtual IEnumerator EndTurnCo()
    {
        Vector3 temp = new Vector3(100.0f,0,0);
        if(unit.partyMember == false)
        {
            temp*=-1;
            
        }
        unit.gameObject.transform.position -= temp;

        unit.EndOfTurn.Invoke();
        fated = false;
        // Removing Timed out Buff and Debuffs, end of turn effects 
        TurnOrderManager.Instance.TurnShift(turnShift);
        BattleManager.Instance.dialogueText.text = " ";
        BattleManager.Instance.attacking = false;
        BattleManager.Instance.useItem = false;
        BattleManager.Instance.playerTurn = false;
        BattleManager.Instance.enemyTurn = false;
        yield return new WaitForSeconds(1f);
        BattleManager.Instance.TurnTransiton();
     
    }

    
    
    
}
