using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PolyTurn : Turn
{
    public int point = -1;
    public List<MiniTurn> innerTurns = new List<MiniTurn>();
    
    public override void EndTurn()
    {
        if(innerTurns.Count >= 1)
        {
            innerTurns[0].EndTurn();
        }
        else
        {
            if (BattleManager.Instance.fightState == FightState.Active)
            {
                BattleManager.Instance.StartEndTurnCo(this);
            }
        }
    }

    public override System.Collections.IEnumerator StartTurnCo()
    {
        if(!exhausted)
        {
            exhausted = true;
            BattleManager.Instance.dialogueText.text = "Several Actions Happen!";
        }
        else
        {
            BattleManager.Instance.dialogueText.text = "Then...";
        }
        yield return new WaitForSeconds(1f);
        innerTurns[0].StartTurn();
    }

    public void NextTurn()
    {
        Turn temp = new Turn();
        temp.PopulateTurn(innerTurns[0].unit, innerTurns[0].fated, innerTurns[0].fate);
        temp.visited ++;
        TurnOrderManager.Instance.recentTurns.Insert(0, temp);
        innerTurns.Remove(innerTurns[0]);
        if(innerTurns.Count >= 1)
        {
            PopulateTurn(innerTurns[0].unit, innerTurns[0].fated, innerTurns[0].fate);
        }
        else
        {
            EndTurn();
        }
    }

    public void AddMiniTurn(FatedPoint point)
    {
        if(innerTurns.Count==0)
        {
            PopulateTurn(point.unit, point.fate != "", point.fate);
        }
        MiniTurn temp = new MiniTurn();
        temp.PopulateTurn(point.unit, point.fate != "", point.fate);
        innerTurns.Add(temp);

    }

    public Turn GetPolyturn()
    {
        if (innerTurns.Count >1)
        {
            return this;
        }
        else if (innerTurns.Count == 1)
        {
            Turn temp = new Turn();
            Turn temp1 = innerTurns[0];
            temp.PopulateTurn(temp1.unit, temp1.fated, temp1.fate);
            return temp;
        }
        return null; 
    }
}
