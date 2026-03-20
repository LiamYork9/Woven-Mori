using UnityEngine;

[CreateAssetMenu(fileName = "unit", menuName = "ScriptableObjects/Unit/Player", order = 1)]
[System.Serializable]
public class PlayerCharacter : Unit
{
    [Header ("Player Specific")]
    public Classes playerClass;
    public int exp;
    public Weapon weapon; 

    public Armor armor;

    public Accessory accessory;


    void Start()
    {
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

    public bool EquipGear(Equipment equipment)
    {
        if((equipment.classList & playerClass) != 0)
        {
            if(equipment is Weapon)
            {
                if(weapon != null)
                {
                    weapon.availableNumber += 1;
                }
                weapon = equipment as Weapon;
                weapon.availableNumber -= 1;
            }
            else if (equipment is Armor)
            {
                if(armor != null)
                {
                    armor.availableNumber += 1;
                }
                armor = equipment as Armor;
                armor.availableNumber -= 1;
            }
            else if(equipment is Accessory)
            {
                if(accessory != null)
                {
                    accessory.availableNumber += 1;
                }
                accessory = equipment as Accessory;
                accessory.availableNumber -= 1;
            }
            return true;
        }
        else 
        {
            return false;
        }

    }

    public void UnEquip(int remove)
    {
        if(remove == 1)
        {
            if(weapon!=null)
            weapon.availableNumber += 1;
            weapon = null;
        }
        if(remove == 2)
        {
            armor.availableNumber += 1;
            armor = null;
        }
        if(remove == 3)
        {
            accessory.availableNumber += 1;
            accessory = null;
        }
       
    }

    public void LevelUp()
    {
        for(int i = 0; i<LevelUpManager.Instance.classGrowths.Count; i++)
        {
            if(LevelUpManager.Instance.classGrowths[i].playerClass == playerClass)
            {
                int milestoneIndex = -1;
                for(int j=0; j<LevelUpManager.Instance.classGrowths[i].milestones.Count;j++)
                {
                    if(LevelUpManager.Instance.classGrowths[i].milestones[j].Level > level)
                    {
                        milestoneIndex = j;
                        break;
                    }
                }

                if(milestoneIndex != -1)
                {
                    ClassGrowth.Milestones goal = LevelUpManager.Instance.classGrowths[i].milestones[milestoneIndex];
                    int levelgap = goal.Level-level;
                    level ++;
                    currentHP += (goal.MaxHP-maxHP)/levelgap;
                    maxHP += (goal.MaxHP-maxHP)/levelgap;
                    attack += (goal.Attack-attack)/levelgap;
                    defense += (goal.Defense-defense)/levelgap;
                    mDefense += (goal.Mdefense-mDefense)/levelgap;
                    speed += (goal.Speed-speed)/levelgap;
                }

                for(int j=0; j<LevelUpManager.Instance.classGrowths[i].skillUnlocks.Count;j++)
                {
                    if(LevelUpManager.Instance.classGrowths[i].skillUnlocks[j].Level == level)
                    {
                        ClassGrowth.SkillUnlocks newSkills = LevelUpManager.Instance.classGrowths[i].skillUnlocks[j];
                        for(int k=0; k<newSkills.SkillIds.Count;k++)
                        {
                            if (!(skills.Contains(newSkills.SkillIds[k])))
                            {
                                skills.Add(newSkills.SkillIds[k]);
                            }
                        }
                    }
                }
                break;
            }
        }
        
    }

    public void ResetLevel()
    {
        level = 0;
        skills.Clear();
        LevelUp();
    }

   
}



