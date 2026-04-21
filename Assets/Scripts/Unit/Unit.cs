using System;
using System.Collections.Generic;
using MoriSkills;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
    public struct BaseStats
    {
        public int Level;
        public int MaxHP;
        public int CurrentHP;
        public int Attack ;

        public int Defense ;
        public int Mdefense;
        public int Speed; 
        public int APCap;
        public int APGain;
        public BaseStats(int lvl, int att, int def, int mdef, int spd, int HPMax, int HP, int capAP, int gainAP)
        {
            Level = lvl;
            Attack = att;
            Defense = def;
            Mdefense = mdef;
            Speed = spd;
            MaxHP = HPMax;
            CurrentHP = HP;
            APCap = capAP;
            APGain = gainAP;
        }

        public void CopyStats(BaseStats target)
        {
            Level = target.Level;
            MaxHP = target.MaxHP;
            CurrentHP = target.CurrentHP;
            Attack = target.Attack;
            Defense = target.Defense;
            Mdefense = target.Mdefense;
            Speed = target.Speed;
            APCap = target.APCap;
            APGain = target.APGain;
        }
        public void CopyStats(BaseStats target, List<int> equipmentStats)
        {
            Level = target.Level;
            MaxHP = target.MaxHP - equipmentStats[4];
            CurrentHP = target.CurrentHP;
            Attack = target.Attack - equipmentStats[0];
            Defense = target.Defense - equipmentStats[1];
            Mdefense = target.Mdefense - equipmentStats[2];
            Speed = target.Speed - equipmentStats[3];
            APCap = target.APCap;
            APGain = target.APGain - equipmentStats[5];
        }
        public void CopyStatsWithEquipment(BaseStats target, List<int> equipmentStats)
        {
            Level = target.Level;
            MaxHP = target.MaxHP + equipmentStats[4];
            CurrentHP = target.CurrentHP;
            Attack = target.Attack + equipmentStats[0];
            Defense = target.Defense + equipmentStats[1];
            Mdefense = target.Mdefense + equipmentStats[2];
            Speed = target.Speed + equipmentStats[3];
            APCap = target.APCap;
            APGain = target.APGain + equipmentStats[5];
            if(CurrentHP > MaxHP)
            {
                CurrentHP = MaxHP;
            }
        }
    }

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

    public BaseStats stats;


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
        stats.CopyStats(target.baseStats, target.equipmentStats);
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
        stats.CurrentHP = stats.MaxHP + equipmentBouns;
        conditions.Clear();
    }

}
