using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.UI;

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
        BattleManager.Instance.EnemyAttack();
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
}
