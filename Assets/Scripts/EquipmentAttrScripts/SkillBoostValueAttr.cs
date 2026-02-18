using UnityEngine;
using MoriSkills;

[CreateAssetMenu(fileName = "SkillBoostValueAttr", menuName = "ScriptableObjects/EqupmentAttr/SkillBoostAttr")]


public class SkillBoostValueAttr : EquipmentAttr
{
    
    
        public int boostValue;

        public SkillBoostValueAttr(int boost)
        {
            name = "Skill Boost";
            boostValue = boost;
        }

        public override void ActivateOnSkill(Skill skill)
        {
            if(basicAttackAttr == true)
            {
                
                skill.power += boostValue;
                    
                
            }
            else if(skill.skillId != SkillId.Attack)
            {
                skill.power += boostValue;
            }
            
            
        }
    
}
