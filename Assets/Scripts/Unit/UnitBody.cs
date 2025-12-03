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

    public Sprite chSprite;

    public Sprite deathSprite;

    public int level;

    public int attack;

    public int defense = 1;

    public int mDefense = 1;

    public int maxHP;

    public int currentHP;

    public int speed = 1;

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

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (unit != null)
        {
           SetUnit(unit);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetUnit(Unit newUnit)
    {
        unit = newUnit;
        CopyStats(newUnit);
    }

    public void Death()
    {
        Debug.Log(name + " Body Death");
        if (currentHP <= 0)
        {
            if (partyMember)
            {
                //BattleManager.Instance.playerSlots[slotNumber].GetComponent<Image>().sprite = deathSprite;
            }
            else
            {
                //BattleManager.Instance.enemySlots[slotNumber].GetComponent<Image>().sprite = deathSprite;
            }
            unit.Death(this);
        }
    }

    public void CopyStats(Unit target)
    {
        name = target.unitName;
        skills = target.skills;
        partyMember = target.partyMember;
        chSprite = target.chSprite;
        deathSprite = target.deathSprite;
        level = target.level;
        attack = target.attack;
        defense = target.defense;
        mDefense = target.mDefense;
        maxHP = target.maxHP;
        currentHP = target.currentHP;
        speed = target.speed;
        APCap = target.APCap;
        APGain = target.APGain;
          resistance = target.resistance;
        immunity = target.immunity;
        vulnerability = target.vulnerability;
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

        DamageNumber(damageMod);

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
