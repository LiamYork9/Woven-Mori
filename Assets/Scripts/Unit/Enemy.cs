using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Unit
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Death();
    }

    // Clears the enemy stats on death 
    public void ClearStats()
    {
        unitName = "";
        chSprite = null;
        attack = 0;
        currentHP = maxHP;
        speed = 0;
    }

// Enemy Death Function (Reomves them from list)
    public void Death()
    {
        if (currentHP <= 0)
        {
            gameObject.SetActive(false);
            ClearStats();
            for (int i = 0; i < TurnOrderManager.Instance.turnOrder.Count; i++)
            {
                if (TurnOrderManager.Instance.turnOrder[i] == BattleManager.Instance.target.GetComponent<Enemy>())
                {
                    TurnOrderManager.Instance.turnOrder.Remove(BattleManager.Instance.target.GetComponent<Enemy>());
                }
            }

            for (int i = 0; i < TurnOrderManager.Instance.recentTurns.Count; i++)
            {
                if (TurnOrderManager.Instance.recentTurns[i] == BattleManager.Instance.target.GetComponent<Enemy>())
                {
                    TurnOrderManager.Instance.recentTurns.Remove(BattleManager.Instance.target.GetComponent<Enemy>());
                }
            }

        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
