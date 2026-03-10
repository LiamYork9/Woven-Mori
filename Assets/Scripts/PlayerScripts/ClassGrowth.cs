using System.Collections.Generic;
using MoriSkills;
using Unity.Mathematics;
using UnityEngine;



[CreateAssetMenu(fileName = "ClassGrowth", menuName = "ScriptableObjects/ClassGrowth")]
public class ClassGrowth : ScriptableObject
{
    [System.Serializable]
    public struct Milestones
    {
        public int Level ;
        
        public int MaxHP;
        public int Attack ;

        public int Defense ;
        public int Mdefense;
        public int Speed; 
        public Milestones(int lvl, int att, int def, int mdef, int spd, int HP)
        {
            Level = lvl;
            Attack = att;
            Defense = def;
            Mdefense = mdef;
            Speed = spd;
            MaxHP = HP;
        }   
    }

    [System.Serializable]
    public struct SkillUnlocks
    {
        public int Level ;
        public List<SkillId> SkillIds;
        public SkillUnlocks(int lvl, List<SkillId> skills)
        {
            Level = lvl;
            SkillIds = new List<SkillId>();
            SkillIds.AddRange(skills);
        }   
    }

    
    public Classes playerClass;
    public List <Milestones> milestones;
    public List <SkillUnlocks> skillUnlocks;
}
