using System;
using System.Collections.Generic;
using MoriSkills;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnitBody : MonoBehaviour
{

    public Unit unit;
    public bool partyMember;

    public List<SkillId> skills;
    [SerializeReference]
    public List<Condition> conditions;

    public Sprite chSprite;

    public Sprite deathSprite;

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
                BattleManager.Instance.playerSlots[slotNumber].GetComponent<Image>().sprite = deathSprite;
            }
            else
            {
                BattleManager.Instance.enemySlots[slotNumber].GetComponent<Image>().sprite = deathSprite;
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
        attack = target.attack;
        defense = target.defense;
        mDefense = target.mDefense;
        maxHP = target.maxHP;
        currentHP = target.currentHP;
        speed = target.speed;
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
    
    public void TakeDamage(int damageValue, Category category = Category.Physical, Element element = Element.None)
    {
        int damageMod = 0;
        if (category == Category.Physical)
        {
            damageMod = damageValue-defense;

        }

        if (category == Category.Magic)
        {
            damageMod = damageValue-mDefense;
        }

        //resistance and immunity checks

        if (damageMod < 1)
        {
            damageMod = 1;
        }

        currentHP -= damageMod;

        if (currentHP <= 0)
        {
            Death();
        }
    }

}
