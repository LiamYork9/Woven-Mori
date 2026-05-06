using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;

public class TurnOrderManager : MonoBehaviour
{
    public static TurnOrderManager Instance;
    public UnitBody turnPlayer;
    public List<GameObject> allFighters;

    public List<UnitBody> downedPlayers = new List<UnitBody> { };

    public List<Turn> turnOrder = new List<Turn> { };

    public List<Turn> recentTurns = new List<Turn> { };

    public int emergencybutton = 0;

    public int cycle = 0;

    public BattleManager BM;




    public void Awake()
    {
        if (TurnOrderManager.Instance != this && TurnOrderManager.Instance != null)
        {
            Destroy(TurnOrderManager.Instance);
            Instance = this;
        }
        else
        {
            Instance = this;
        }

    }

    void Start()
    {
        BM = BattleManager.Instance;
    }


    void Update()
    {

        
    }

    // Creates a list of all the current fighters in a battle
    public void GatherFighters()
    {
        if (BM == null)
        {
            BM = BattleManager.Instance;
        }
        for (int i = 0; i < BM.playerSlots.Count; i++)
        {
            BM.playerSlots[i].name = BM.playerSlots[i].GetComponent<UnitBody>().name;
            allFighters.Add(BM.playerSlots[i]);
        }

        for (int i = 0; i < BM.enemySlots.Count; i++)
        {
            BM.enemySlots[i].name = BM.enemySlots[i].GetComponent<UnitBody>().name;
            allFighters.Add(BM.enemySlots[i]);
        }
        TurnCalulation();



    }

    // How turn order is calculated
    public void TurnCalulation()
    {
        emergencybutton = 0;
        while (turnOrder.Count < 11 && emergencybutton < 100)
        {
            emergencybutton++;
            for (int i = 0; i < allFighters.Count; i++)
            {
                
                UnitBody tempUnit = allFighters[i].GetComponent<UnitBody>();
                tempUnit.initiative += Mathf.Max(1,tempUnit.activeStats.Speed + Random.Range(-5, 6));

                if (tempUnit.initiative >= 100)
                {
                    Turn tempTurn = new Turn();
                    tempUnit.initiative -= 100;
                    tempTurn.PopulateTurn(tempUnit);
                    turnOrder.Add(tempTurn);
                }
            }
            cycle++;
        }
        InitiativeSort();
    }

    public void TurnShift(int shift = 1)
    {
        if (turnOrder.Count <= 10)
        {
            TurnCalulation();
        }

        if (shift >= 0)
        {
            for (int i = 0; i < shift; i++)
            {
                if(turnOrder[0].exhausted != true)
                {
                    recentTurns.Insert(0, turnOrder[0]);
                }
                turnOrder.Remove(turnOrder[0]);
                BM.globalTurn += 1;
            }
            //BM.TurnTransiton();
        }
        else
        {
            if (recentTurns.Count + shift >= 0)
            {
                for (int i = 0; i + shift < 0; i++)
                {

                    turnOrder.Insert(0, recentTurns[0]);
                    recentTurns.Remove(recentTurns[0]);
                    BM.globalTurn -= 1;
                }
                //BM.TurnTransiton();
            }
            else
            {
                TurnShift();
            }
        }
       

        if (turnOrder.Count <= 10)
        {
            TurnCalulation();
        }
        BM.gTurnText.text = "Turn: " + BM.globalTurn;
    }

    public void InitiativeSort()
    {
        CheckInitiatives();
        SortExcludingFirst();
    }

    public void CheckInitiatives()
    {
        foreach(Turn turn in turnOrder)
        {
            while(turn.initiative>=100)
            {
                turn.initiative -= 100;
                turn.cycle -=1;
            }
            while(turn.initiative<0)
            {
                turn.initiative += 100;
                turn.cycle +=1;
            }
        }
    }
    public void SortExcludingFirst()
    {
        Turn first = turnOrder[0];
        List<Turn> temp = new List<Turn>();
        temp.AddRange(turnOrder);
        temp.RemoveAt(0);
        turnOrder.Clear();
        turnOrder.Add(first);
        turnOrder.AddRange(temp.OrderBy(x => x.cycle).ThenByDescending(x=>x.initiative).ToList<Turn>());
    }

    public void EndTurn()
    {
        turnOrder[0].EndTurn();
    }
}
