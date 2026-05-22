using System.Collections.Generic;
using UnityEngine;

public class TurnOrderManager : MonoBehaviour
{
    public static TurnOrderManager Instance;
    public UnitBody turnPlayer;
    public List<GameObject> allFighters;

    public List<UnitBody> downedPlayers = new List<UnitBody> { };

    [SerializeReference]
    public List<Turn> turnOrder = new List<Turn> { };

    public List<Turn> recentTurns = new List<Turn> { };

    public int emergencybutton = 0;

    public int cycle = 0;

    public BattleManager BM;

    public List<FatedPoint> fatedPoints;




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
            fatedPoints.AddRange(BM.playerSlots[i].GetComponent<UnitBody>().fatedPoints);
        }

        for (int i = 0; i < BM.enemySlots.Count; i++)
        {
            BM.enemySlots[i].name = BM.enemySlots[i].GetComponent<UnitBody>().name;
            allFighters.Add(BM.enemySlots[i]);
            fatedPoints.AddRange(BM.enemySlots[i].GetComponent<UnitBody>().fatedPoints);
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

    public void Prioritize(UnitBody unit, int priority)
    {
        int buffer = 0;
        if(priority > 0)
        {
            for(int i = 1; i < turnOrder.Count; i++)
            {
                if(turnOrder[i].unit == unit)
                {
                    Turn temp = turnOrder[i];
                    for(int j = 1; j <= priority; j++)
                    {
                        if(i-j > buffer)
                        {
                            Debug.Log("i=" + i + "   j=" + j + "   " + (i-j) + "th Slot: Buffer " + buffer );
                            turnOrder[i-(j-1)] = turnOrder[i-j];
                            turnOrder[i-j] = temp;
                        }
                    }
                    if(i-priority <= buffer)
                    {
                        buffer++;
                    }
                }
            }
        }
        else if(priority < 0)
        {
            buffer = 1;
            for(int i = turnOrder.Count-1; i > 0; i--)
            {
                if(turnOrder[i].unit==unit)
                {
                    Turn temp = turnOrder[i];
                    for(int j = 1; j <= priority; j++)
                    {
                        if(i+j <= turnOrder.Count - buffer)
                        {
                            turnOrder[i+(j-1)] = turnOrder[i+j];
                            turnOrder[i+j] = temp;
                        }
                    }
                    if(i+priority >= turnOrder.Count-buffer)
                    {
                        buffer++;
                    }
                }
            }
        }
    }

    public void EndTurn()
    {
        turnOrder[0].EndTurn();
    }
}
