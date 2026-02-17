using System;
using System.Collections.Generic;
using System.Linq;
using MoriSkills;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

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

    public int level;

    public int attack;

    public int defense = 1;

    public int mDefense = 1;

    public int speed = 1;

    public int maxHP;

    public int currentHP;


    public int initiative;

    public int slotNumber;

    public int localTurnCount;

    public int localTurnCountCurrentVal;

    public int AP;

    public int APCap;

    public int APGain = 1;

    public int emergencybutton;

    
    public UnityEvent StartOfTurn;
    public UnityEvent StartOfAction;

    public UnityEvent EndOfAction;
    public UnityEvent EndOfTurn;

     public HPTest hPTest;

    public List<int> equipmentStats = new List<int> {0,0,0,0,0,0};

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (unit != null)
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
        if (currentHP <= 0)
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
        level = target.level;
        attack = target.attack + equipmentStats[0];
        defense = target.defense + equipmentStats[1];
        mDefense = target.mDefense + equipmentStats[2];
        speed = target.speed + equipmentStats[3];
        maxHP = target.maxHP + equipmentStats[4];
        currentHP = target.currentHP;
        APCap = target.APCap;
        APGain = target.APGain + equipmentStats[5];
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
        attack = 0;
        defense = 0;
        mDefense = 0;
        maxHP = 0;
        currentHP = 0;
        speed = 0;
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

    public void TakeDamage(int damageValue, DamageType damageType = DamageType.Physical, Element element = Element.None)
    {
        int damageMod = 0;
        if (damageType == DamageType.Physical)
        {
            damageMod = damageValue / defense;

        }

        if (damageType == DamageType.Magic)
        {
            damageMod = damageValue / mDefense;
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

        currentHP -= damageMod;
        PopUpManager.Instance.DamageDone(damageMod,this.transform.position,false);

        

        if (currentHP <= 0)
        {
            Death();
        }
        
    }
    
    public UnitBody ApplyCondition(Condition addedCondition)
    {
        conditions.Add(addedCondition);
        addedCondition.OnApply(this);
        return this;
    }

    public void DamageNumber(int damage)
    {
        
    }

}
