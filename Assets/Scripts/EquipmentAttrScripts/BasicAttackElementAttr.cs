using MoriSkills;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicAttackElementAttr", menuName = "ScriptableObjects/EqupmentAttr/BasicAttackElementAttr")]
public class BasicAttackElementAttr : EquipmentAttr
{
    public Element element;
     public override void ActivateOnSkill(Skill skill)
        {
            if( skill.skillId == SkillId.Attack)
            {
                
                
               skill.element = element;
                
            }
            
            
            
        }
}
