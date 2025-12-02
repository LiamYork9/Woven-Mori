using System;
using System.Collections.Generic;
using MoriSkills;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "unit", menuName = "ScriptableObjects/Unit/Generic", order = 1)]
[Serializable]
public class Unit : ScriptableObject
{

    public string unitName;
    public bool partyMember;

    public List<SkillId> skills;

    public List<Element> resistance;

    public List<Element> immunity;

    public List<Element> vulnerability;
    [SerializeReference]
    public List<Condition> conditions;

    public Sprite chSprite;

    public Sprite deathSprite;

    public int level = 1;

    public int attack;

    public int defense = 1;

    public int mDefense = 1;

    public int maxHP;

    public int currentHP;

    public int speed = 1;

    public int initiative;

    public int localTurnCount;

    public int localTurnCountCurrentVal;

    public int AP;

    public int APCap;

    public int APGain = 1;

    public int emergencybutton;


    //Events

    public void CopyStats(UnitBody target)
    {
        unitName = target.name;
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

    public virtual void Death(UnitBody body)
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

    public void Restore()
    {
        currentHP = maxHP;
        conditions.Clear();
    }

}
