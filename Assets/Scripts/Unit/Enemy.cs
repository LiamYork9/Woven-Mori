using UnityEngine;

[CreateAssetMenu(fileName = "unit", menuName = "ScriptableObjects/Unit/Enemy", order = 1)]
[System.Serializable]
public class Enemy : Unit
{
    public int expYield; 

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
    public override void Death(UnitBody body)
    {
        BattleManager.Instance.expEarned += expYield;
        for(int i = 0; i < PartyManager.Instance.party.Count; i++)
        {
            //PartyManager.Instance.party[i].get +=
        }
        if (body.currentHP <= 0)
        {
            Debug.Log(unitName + " Soul Death");
            body.gameObject.SetActive(false);
            //ClearStats();
            for (int i = 0; i < TurnOrderManager.Instance.turnOrder.Count; i++)
            {
                emergencybutton = 0;
                while (i < TurnOrderManager.Instance.turnOrder.Count && emergencybutton < 20)
                {
                    if (TurnOrderManager.Instance.turnOrder[i].unit == body)
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
                    if (TurnOrderManager.Instance.recentTurns[i].unit == body)
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


            BattleManager.Instance.enemySlots.Remove(body.gameObject);
            TurnOrderManager.Instance.allFighters.Remove(body.gameObject);

        }
        else
        {
            body.gameObject.SetActive(true);
        }

        base.Death(body);
    }
}
