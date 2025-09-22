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

    // Enemy Death Function (Removes them from list)
    public override void Death()
    {
        if (currentHP <= 0)
        {
            gameObject.SetActive(false);
            ClearStats();
            for (int i = 0; i < TurnOrderManager.Instance.turnOrder.Count; i++)
            {
                emergencybutton = 0;
                while (i < TurnOrderManager.Instance.turnOrder.Count && emergencybutton < 20)
                {
                    if (TurnOrderManager.Instance.turnOrder[i].unit == this)
                    {
                        TurnOrderManager.Instance.turnOrder.RemoveAt(i);
                    }
                    else
                    {
                        break;
                    }
                    emergencybutton++;
                }
            }

            for (int i = 0; i < TurnOrderManager.Instance.recentTurns.Count; i++)
            {
                emergencybutton = 0;
                while (i < TurnOrderManager.Instance.recentTurns.Count && emergencybutton < 20)
                {
                    if (TurnOrderManager.Instance.recentTurns[i].unit == this)
                    {
                        TurnOrderManager.Instance.recentTurns.RemoveAt(i);
                    }
                    else
                    {
                        break;
                    }
                    emergencybutton++;
                }
            }


            BattleManager.Instance.enemySlots.Remove(this.gameObject);
            TurnOrderManager.Instance.allFighters.Remove(this.gameObject);

        }
        else
        {
            gameObject.SetActive(true);
        }
        
        base.Death();
    }
}
