using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;

public class MiniTurn : Turn
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override IEnumerator EndTurnCo()
    {
        unit.EndOfTurn.Invoke();
        // Removing Timed out Buff and Debuffs, end of turn effects 
        Turn temp = TurnOrderManager.Instance.turnOrder[0];
        temp.turnShift = turnShift;
        if(temp is PolyTurn)
        {
            (temp as PolyTurn).NextTurn();
        }

        BattleManager.Instance.dialogueText.text = " ";
        BattleManager.Instance.attacking = false;
        BattleManager.Instance.useItem = false;
        BattleManager.Instance.playerTurn = false;
        BattleManager.Instance.enemyTurn = false;
        yield return new WaitForSeconds(1f);
        BattleManager.Instance.TurnTransiton();
    }
    
}
