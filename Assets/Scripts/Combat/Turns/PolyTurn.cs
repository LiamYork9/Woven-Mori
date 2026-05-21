using System.Collections.Generic;
using UnityEngine;

public class PolyTurn : Turn
{
    public List<MiniTurn> innerTurns;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override System.Collections.IEnumerator StartTurnCo()
    {
        exhausted = true;
        BattleManager.Instance.dialogueText.text = "Several Actions Happen!";
        yield return new WaitForSeconds(1f);
        innerTurns[0].StartTurn();
    }

    public void NextTurn()
    {
        Turn temp = new Turn();
        temp.PopulateTurn(innerTurns[0].unit);
        TurnOrderManager.Instance.recentTurns.Insert(0, temp);
        innerTurns.Remove(innerTurns[0]);
        if(innerTurns.Count >= 1)
        {
            innerTurns[0].StartTurn();
        }
        else
        {
            EndTurn();
        }
    }
}
