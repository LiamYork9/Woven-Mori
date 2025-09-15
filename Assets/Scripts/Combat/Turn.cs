using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class Turn
{
    public string name;
    public Unit unit;
    public int initiative;
    public int cycle;

    public void StartTurn()
    {
        Debug.Log("Start " + unit.name + "'s Turn!");
        unit.localTurnCount += 1;
        unit.AP += unit.APGain;
        SelectAction();
    }

    public void SelectAction()
    {
        Debug.Log("What will " + unit.name + " do?");
    }

    public void EndTurn()
    {
        Debug.Log("End " + unit.name + "'s Turn!");
        
        BattleManager.Instance.TurnTransiton();
    }

    public void PopulateTurn(Unit tempUnit)
    {
        unit = tempUnit;
        name = unit.name;
        initiative = tempUnit.initiative;
        cycle = TurnOrderManager.Instance.cycle;
    }
}
