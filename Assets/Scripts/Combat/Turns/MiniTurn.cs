using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MiniTurn : Turn
{
    
    public override IEnumerator StartTurnCo()
    {
        BattleManager.Instance.ButtonsOff();
        BattleManager.Instance.dialogueText.text = "Start " + unit.name + "'s Turn!";
        yield return new WaitForSeconds(1f);
        unit.StartOfTurn.Invoke();

        SelectAction();
      
    
       
    }
    public override IEnumerator EndTurnCo()
    {
        fated = false;

        unit.EndOfTurn.Invoke();
        // Removing Timed out Buff and Debuffs, end of turn effects 
        Turn temp = TurnOrderManager.Instance.turnOrder[0];
        temp.turnShift = turnShift;
        

        BattleManager.Instance.dialogueText.text = "";
        BattleManager.Instance.attacking = false;
        BattleManager.Instance.useItem = false;
        BattleManager.Instance.playerTurn = false;
        BattleManager.Instance.enemyTurn = false;
        yield return new WaitForSeconds(1f);
        if(TurnOrderManager.Instance.turnOrder[0] is PolyTurn)
        {
            (temp as PolyTurn).NextTurn();
        }
        else
        {
            Debug.Log("NotPoly");
            BattleManager.Instance.TurnTransiton();
        }
    }
    
}
