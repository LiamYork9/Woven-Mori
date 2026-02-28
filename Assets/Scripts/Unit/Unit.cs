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

    [Header("Stats")]

    public int level = 1;

    
    public int maxHP;

    public int currentHP;

    public int attack;

    public int defense = 1;

    public int mDefense = 1;

     public int speed = 1;



    


    public int APCap;

    public int APGain = 1;


    public List<int> equipmentStats = new List<int> {0,0,0,0,0,0};

    protected int initiative;
    protected int emergencybutton;
    //Events

    public void CopyStats(UnitBody target)
    {
        for (int j = 0; j < target.conditions.Count; j++)
            {
                target.conditions[j].RemoveCondition();
                j--;
            }
        unitName = target.name;
        skills = target.skills;
        partyMember = target.partyMember;
        chSprite = target.chSprite;
        deathSprite = target.deathSprite;
        level = target.level;
        maxHP = target.maxHP - target.equipmentStats[4];
        currentHP = target.currentHP;
        attack = target.attack - target.equipmentStats[0];
        defense = target.defense - target.equipmentStats[1];
        mDefense = target.mDefense - target.equipmentStats[2];
        speed = target.speed - target.equipmentStats[3];
        
        APCap = target.APCap;
        APGain = target.APGain - target.equipmentStats[5];
        // resistance = target.resistance;
        // immunity = target.immunity;
        // vulnerability = target.vulnerability;
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
        int equipmentBouns  = 0;
        if(this is PlayerCharacter)
        {
            if((this as PlayerCharacter).weapon != null)
            {
                equipmentBouns += (this as PlayerCharacter).weapon.stats[4];
            }
            if((this as PlayerCharacter).armor != null)
            {
                equipmentBouns += (this as PlayerCharacter).armor.stats[4];
            }
            if((this as PlayerCharacter).accessory != null)
            {
                equipmentBouns += (this as PlayerCharacter).accessory.stats[4];
            }

        }
        currentHP = maxHP + equipmentBouns;
        conditions.Clear();
    }

}
