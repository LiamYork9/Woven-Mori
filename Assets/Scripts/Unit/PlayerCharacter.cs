using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : Unit
{

    void Start()
    {

    }

    void Update()
    {
        
    }



    public void Death()
    {
        if (currentHP <= 0)
        {
            BattleManager.Instance.playerSlots[slotNumber].GetComponent<Image>().sprite = deathSprite;

            for (int i = 0; i < TurnOrderManager.Instance.turnOrder.Count; i++)
            {
                if (TurnOrderManager.Instance.turnOrder[i] == BattleManager.Instance.enemyTarget.GetComponent<PlayerCharacter>())
                {
                    TurnOrderManager.Instance.turnOrder.Remove(BattleManager.Instance.enemyTarget.GetComponent<PlayerCharacter>());
                }
            }

            for (int i = 0; i < TurnOrderManager.Instance.recentTurns.Count; i++)
            {
                if (TurnOrderManager.Instance.recentTurns[i] == BattleManager.Instance.enemyTarget.GetComponent<PlayerCharacter>())
                {
                    TurnOrderManager.Instance.recentTurns.Remove(BattleManager.Instance.enemyTarget.GetComponent<PlayerCharacter>());
                }
            }

            TurnOrderManager.Instance.downedPlayers.Add(BattleManager.Instance.enemyTarget.GetComponent<PlayerCharacter>());
            
          
                
            TurnOrderManager.Instance.allFighters.Remove(BattleManager.Instance.enemyTarget);
                
            

        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}



