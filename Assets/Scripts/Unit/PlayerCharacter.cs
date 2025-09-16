using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : Unit
{

    void Start()
    {
        AP = 1;
        APCap = 10;
    }

    void Update()
    {
        if (AP > APCap)
        {
            AP = APCap;
        }
    }



    public void Death()
    {
        if (currentHP <= 0)
        {
            BattleManager.Instance.playerSlots[slotNumber].GetComponent<Image>().sprite = deathSprite;

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

            TurnOrderManager.Instance.downedPlayers.Add(this);
            
          
                
            TurnOrderManager.Instance.allFighters.Remove(this.gameObject);
                
            

        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}



