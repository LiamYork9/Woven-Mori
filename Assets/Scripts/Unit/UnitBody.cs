using System;
using System.Collections.Generic;
using MoriSkills;
using UnityEngine;
using UnityEngine.Events;


public class UnitBody : MonoBehaviour
{

    public Unit unit;
    public bool partyMember;

    public List<SkillId> skills;

    public List<Element> resistance;

    public List<Element> immunity;

    public List<Element> vulnerability;
    [SerializeReference]
    public List<Condition> conditions;

    [SerializeReference]
    public List<EquipmentAttr> equipmentAttrs;

    public Sprite chSprite;

    public Sprite deathSprite;

    public BaseStats baseStats;

    public BaseStats activeStats;

    public List<FatedTurn> fatedTurns;
    public List<FatedPoint> fatedPoints;

    public int turnsAdded = 0;

    public int initiative;

    public int slotNumber;

    public int localTurnCount;

    public int localTurnCountCurrentVal;

    public int AP;

    public int emergencybutton;

    public Classes playerClass;

    
    public UnityEvent StartOfTurn;
    public UnityEvent StartOfAction;

    public UnityEvent EndOfAction;
    public UnityEvent EndOfTurn;

    public HPTest hPTest;

    public List<int> equipmentStats = new List<int> {0,0,0,0,0,0};

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (unit != null && unit.partyMember == false)
        {
           SetUnit(unit);
        }
        hPTest.SetHpBar();
        if(equipmentAttrs == null){
        equipmentAttrs = new List<EquipmentAttr>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        hPTest.SetHpBar();
    }

    public void SetUnit(Unit newUnit)
    {
        unit = newUnit;
        CopyStats(newUnit);
        //Debug.Log("Copied Stats" + newUnit.name);
    }

    public void Death()
    {
        Debug.Log(name + " Body Death");
        if (activeStats.CurrentHP <= 0)
        {
            if (partyMember)
            {
                BattleManager.Instance.playerSlots.Remove(this.gameObject);
            }
            else
            {
                BattleManager.Instance.enemySlots.Remove(this.gameObject);
            }
            unit.Death(this);
        }
    }

    public void CopyStats(Unit target)
    {
        if(target is PlayerCharacter)
        {
            CheckEquipment(target as PlayerCharacter);
        }
        name = target.unitName;
        skills = target.skills;
        partyMember = target.partyMember;
        chSprite = target.chSprite;
        deathSprite = target.deathSprite;
        baseStats.CopyStatsWithEquipment(target.stats, equipmentStats);
        activeStats.CopyStats(baseStats);

        fatedTurns.AddRange(target.fatedTurns);
        fatedPoints.AddRange(target.fatedPoints);
        for(int i = 0; i < fatedPoints.Count;i++)
        {
            FatedPoint temp = fatedPoints[i];
            temp.unit = this;
            fatedPoints[i]= temp;
        }
        
        resistance = new List<Element>();
        for(int i = 0; i < target.resistance.Count; i++)
        {
             resistance.Add(target.resistance[i]);
        }
        vulnerability = new List<Element>();
        for(int i = 0; i < target.vulnerability.Count; i++)
        {
             vulnerability.Add(target.vulnerability[i]);
        }
        immunity = new List<Element>();
        for(int i = 0; i < target.immunity.Count; i++)
        {
             immunity.Add(target.immunity[i]);
        }
        
    }

    public void ClearStats()
    {
        name = "";
        skills = null;
        partyMember = false;
        chSprite = null;
        deathSprite = null;
        activeStats.Attack = 0;
        activeStats.Defense = 0;
        activeStats.Mdefense = 0;
        activeStats.MaxHP = 0;
        activeStats.CurrentHP = 0;
        activeStats.Speed = 0;
    }

    public void CheckEquipment(PlayerCharacter target)
    {
        equipmentStats = new List<int> {0,0,0,0,0,0};

        for(int i = 0; i < equipmentStats.Count; i++)
        {
            if(target.armor != null)
            {
                equipmentStats[i] += target.armor.stats[i];
            }
            if(target.weapon != null)
            {
                equipmentStats[i] += target.weapon.stats[i];
            }
            if(target.accessory != null)
            {
                equipmentStats[i] += target.accessory.stats[i];
            }
           
        }
    }

    public int TakeDamage(int damageValue, DamageType damageType = DamageType.Physical, Element element = Element.None)
    {
        int damageMod = 0;
        if (damageType == DamageType.Physical)
        {
            damageMod = damageValue / activeStats.Defense;

        }

        if (damageType == DamageType.Magic)
        {
            damageMod = damageValue / activeStats.Mdefense;
        }

        if(damageType == DamageType.Destined)
        {
            damageMod = damageValue;
        }

        if (resistance.Contains(element))
        {
            damageMod /= 2;
        }

        if (vulnerability.Contains(element))
        {
            damageMod *= 2;
        }

        if (damageMod < 1)
        {
            damageMod = 1;
        }

        if (immunity.Contains(element))
        {
            damageMod = 0;
        }
        
        int returnDamage = 0;
        if(damageMod>activeStats.CurrentHP)
        {
            returnDamage = activeStats.CurrentHP;
        }
        else
        {
            returnDamage=damageMod;
        }


        activeStats.CurrentHP -= damageMod;
        PopUpManager.Instance.DamageDone(damageMod,this.transform.position,false);

        

        if (activeStats.CurrentHP <= 0)
        {
            Death();
        }

        return returnDamage;
    }
    
    public UnitBody ApplyCondition(Condition addedCondition)
    {
        addedCondition.ApplyCondition(this);
        return this;
    }

    public void DamageNumber(int damage)
    {
        
    }


    //Fated turn things
    public bool CheckFate()
    {
        foreach(FatedTurn fate in fatedTurns)
        {
            if((turnsAdded==fate.startTurn||(fate.repeating&&(turnsAdded-fate.startTurn)%fate.frequency==0)) && (turnsAdded<=fate.endTurn||fate.endTurn==-1))
            {
                return true;
            }
        }



        return false;
    }

    public string GetFate()
    {
        string temp = "";
        foreach(FatedTurn fate in fatedTurns)
        {
            if((turnsAdded==fate.startTurn||(fate.repeating&&(turnsAdded-fate.startTurn)%fate.frequency==0))  && (turnsAdded<=fate.endTurn||fate.endTurn==-1))
            {
                temp = fate.fate;
            }
        }



        return temp;
    }

}
