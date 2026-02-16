using UnityEngine;

[CreateAssetMenu(fileName = "unit", menuName = "ScriptableObjects/Unit/Player", order = 1)]
[System.Serializable]
public class PlayerCharacter : Unit
{
    public Classes theirClass;
    public Weapon weapon; 

    public Armor armor;

    public Accessory accessory;

    void Start()
    {
        AP = 1;
        APCap = 10;
    }

   



    public override void Death(UnitBody body)
    {

        if (body.currentHP <= 0)
        {
            body.gameObject.SetActive(false);
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

            TurnOrderManager.Instance.downedPlayers.Add(body);



            TurnOrderManager.Instance.allFighters.Remove(body.gameObject);



        }
        else
        {
            body.gameObject.SetActive(true);
        }

        base.Death(body);
    }

    public void EquipGear(Equipment equipment)
    {
        if(equipment is Weapon)
        {
            weapon = equipment as Weapon;
        }
        else if (equipment is Armor)
        {
            armor = equipment as Armor;
        }
        else if(equipment is Accessory)
        {
            accessory = equipment as Accessory;
        }
    }
}



