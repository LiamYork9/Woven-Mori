using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class Turn
{
    public Unit unit;
    public int initiative;
    public int cycle;

    public void StartTurn()
    {
        Debug.Log("Start " + unit.name + "'s Turn!");
    }

    public void SelectAction()
    {
        Debug.Log("What will " + unit.name + " do?");
    }

    public void EndTurn()
    {
        Debug.Log("End " + unit.name + "'s Turn!");
    }
}
