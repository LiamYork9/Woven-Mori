using UnityEngine;

[CreateAssetMenu(fileName = "unit", menuName = "ScriptableObjects/Unit/Player", order = 1)]
[System.Serializable]
public class PlayerCharacter : Unit
{
    [Header ("Player Specific")]
    public Classes theirClass;
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

    public void LevelUp()
    {
        for(int i = 0; i<LevelUpManager.Instance.classGrowths.Count; i++)
        {
            if(LevelUpManager.Instance.classGrowths[i].playerClass == theirClass)
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

   
}



