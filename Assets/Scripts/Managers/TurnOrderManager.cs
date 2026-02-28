using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
        while (turnOrder.Count < 11 && emergencybutton < 100)
        {
            emergencybutton++;
            for (int i = 0; i < allFighters.Count; i++)
            {
                Turn tempTurn = new Turn();
                UnitBody tempUnit = allFighters[i].GetComponent<UnitBody>();
                tempUnit.initiative += Mathf.Max(1,tempUnit.speed + Random.Range(-5, 6));

                if (tempUnit.initiative >= 100)
                {
                    tempUnit.initiative -= 100;
                    tempTurn.PopulateTurn(tempUnit);
                    turnOrder.Add(tempTurn);
                }
            }
            cycle++;
        }
    }


    public void SpeedUp()
    {
        //turnPlayer.speed += 10;
        emergencybutton = 0;
        turnOrder.Clear();
        TurnCalulation();
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
                recentTurns.Insert(0, turnOrder[0]);
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

    public void EndTurn()
    {
        turnOrder[0].EndTurn();
    }
}
