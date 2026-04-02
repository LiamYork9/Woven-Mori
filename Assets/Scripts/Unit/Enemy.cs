using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    Rare,
    Uncommon,
    Common
}
[System.Serializable]
    public struct Loot
    {
        public Item item;
        public int amount;
        public int chance;
        public Rarity rarity;
        public Loot(Item loot, int count, int dropchance, Rarity tier)
        {
            item = loot;
            amount = count;
            chance = dropchance;
            rarity = tier;
        }   
    }

[CreateAssetMenu(fileName = "unit", menuName = "ScriptableObjects/Unit/Enemy", order = 1)]
[System.Serializable]
public class Enemy : Unit
{
    
    public int expYield; 

    public int money;

    public List<Loot> lootTable;
    public int dropChance = 20;
    

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
        BattleManager.Instance.cashEarned += money;
        BattleManager.Instance.loot.AddRange(RollLoot());
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

    public List<Loot> RollLoot()
    {
        List<Loot> temp = new List<Loot>();
        Rarity tier = Rarity.Common;
        List<Loot> sameTier = new List<Loot>();
        List<Loot> reward = new List<Loot>();
        temp.AddRange(lootTable);
        int lootcount = 0;
        int tempint = 0;
        for(int i = 0; i<3; i++)
        {
            tempint = Random.Range(0, 100);
            if (tempint < dropChance)
            {
                lootcount+=1;
            }
        }
        
        for(int i = 0; i<lootcount; i++)
        {
            tempint = Random.Range(0, 100);
            if (tempint < 5)
            {
                tier = Rarity.Rare;
            }
            else if (tempint < 30)
            {
                tier = Rarity.Uncommon;
            }
            else
            {
                tier = Rarity.Common;
            }
            sameTier.Clear();
            while(temp.Count>0 && sameTier.Count == 0)
            {
                for(int j = 0; j < temp.Count; j++)
                {
                    if (temp[j].rarity == tier)
                    {
                        if(tier != Rarity.Common)
                        {
                            tempint = Random.Range(0, 100);
                            if(tempint<temp[j].chance)
                            {
                                sameTier.Add(temp[j]);
                            }
                        }
                        else
                        {
                            sameTier.Add(temp[j]);
                        }

                    }
                }
                if(sameTier.Count==0)
                {
                    if(tier == Rarity.Rare)
                    {
                        tier = Rarity.Uncommon;
                    }
                    else if(tier == Rarity.Uncommon)
                    {
                        tier = Rarity.Common;
                    }
                    else
                    {
                        break;
                    }
                }
                if(tier == Rarity.Common)
                {
                    for(int j = sameTier.Count-1; j > -1; j--)
                    {
                        tempint = Random.Range(0, 100);
                        if(tempint>sameTier[j].chance)
                        {
                            sameTier.RemoveAt(j);
                        }
                    }
                }
            }
            if(sameTier.Count>0)
            {
                tempint=Random.Range(0,sameTier.Count);
                reward.Add(sameTier[tempint]);
                temp.Remove(sameTier[tempint]);
            }
        }

        return reward;
    }
}
