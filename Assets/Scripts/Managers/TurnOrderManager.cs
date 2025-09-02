using System.Collections.Generic;
using UnityEngine;

public class TurnOrderManager : MonoBehaviour
{
    public static TurnOrderManager Instance;
    public Unit turnPlayer;
    public List<GameObject> allFighters;

    public List<Unit> downedPlayers = new List<Unit> { };

    public List<Unit> turnOrder = new List<Unit> { };

    public List<Unit> recentTurns = new List<Unit> { };

    public int emergencybutton = 0;




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

    }


    void Update()
    {
        turnPlayer = turnOrder[0];

        if (turnOrder.Count <= 6)
        {
            TurnCalulation();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            SpeedUp();
        }
    }

    // Creates a list of all the current fighters in a battle
    public void GatherFighters()
    {
        for (int i = 0; i < BattleManager.Instance.playerSlots.Count; i++)
        {
            BattleManager.Instance.playerSlots[i].name = BattleManager.Instance.playerSlots[i].GetComponent<Unit>().unitName;
            allFighters.Add(BattleManager.Instance.playerSlots[i]);
        }

        for (int i = 0; i < BattleManager.Instance.enemySlots.Count; i++)
        {
            BattleManager.Instance.enemySlots[i].name = BattleManager.Instance.enemySlots[i].GetComponent<Unit>().unitName;
            allFighters.Add(BattleManager.Instance.enemySlots[i]);
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
                Unit temp = allFighters[i].GetComponent<Unit>();
                temp.initiative += temp.speed;

                if (temp.initiative >= 100)
                {
                    temp.initiative -= 100;
                    turnOrder.Add(temp);
                }
            }
        }
    }


    public void SpeedUp()
    {
        //turnPlayer.speed += 10;
        emergencybutton = 0;
        turnOrder.Clear();
        TurnCalulation();
    }
}
