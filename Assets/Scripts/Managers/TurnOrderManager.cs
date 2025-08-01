using System.Collections.Generic;
using UnityEngine;

public class TurnOrderManager : MonoBehaviour
{
    public static TurnOrderManager Instance;
    public GameObject turnPlayer;
    public List<GameObject> allFighters;

    public List<Unit> turnOrder = new List<Unit>{};

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
        
    }

    public void GatherFighters()
    {
        for (int i = 0; i < BattleManager.Instance.playerSlots.Count; i++)
        {
            allFighters.Add(BattleManager.Instance.playerSlots[i]);
        }

        for (int i = 0; i < BattleManager.Instance.enemySlots.Count; i++)
        {
            allFighters.Add(BattleManager.Instance.enemySlots[i]);
        }
        TurnCalulation();


    }

    public void TurnCalulation()
    {
        while (turnOrder.Count < 11 && emergencybutton < 100)
        {
            emergencybutton++;
            for (int i = 0; i < allFighters.Count; i++)
            {
                Unit temp = allFighters[i].GetComponent<Unit>();
                Debug.Log(temp.initiative);
                temp.initiative += temp.speed;

                if (temp.initiative >= 100)
                {
                    temp.initiative -= 100;
                    turnOrder.Add(temp);
                }
            }
        }
    }
}
