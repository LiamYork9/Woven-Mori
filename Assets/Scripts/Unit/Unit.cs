using System;
using System.Collections.Generic;
using MoriSkills;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public string unitName;
    public bool partyMember;

    public List<SkillId> skills;

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

    void Start()
    {

    }


    void Update()
    {

    }

    public void CopyStats(Unit target)
    {
        unitName = target.unitName;
        skills = target.skills;
        slotNumber = target.slotNumber;
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

    public virtual void Death()
    {
        if (BattleManager.Instance.enemySlots.Count == 0)
        {
            BattleManager.Instance.WinCondtion();
        }
        if (BattleManager.Instance.playerSlots.Count == 0)
        {
            BattleManager.Instance.LoseCondition();
        }
    }

    public void TakeDamage(int damageValue, Category category = Category.Physical, Element element = Element.None)
    {
        int damageMod = 0;
        if (category == Category.Physical)
        {
            damageMod = damageValue / defense;

        }

        if (category == Category.Magic)
        {
            damageMod = damageValue / mDefense;
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
