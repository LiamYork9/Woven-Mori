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
                for(int i = 0; i < skill.attrs.Count; i++)
            {
                if(skill.attrs[i] is DamageAttr)
                {
                    (skill.attrs[i] as DamageAttr).element = element;
                }
                else if(skill.attrs[i] is LevelScaleAttr)
                {
                    for(int j = 0; j<(skill.attrs[i] as LevelScaleAttr).scaledAttr.Count; j++)
                    {
                        if((skill.attrs[i] as LevelScaleAttr).scaledAttr[j] is DamageAttr)
                        {
                            ((skill.attrs[i] as LevelScaleAttr).scaledAttr[j] as DamageAttr).element = element;
                        }
                    }
                }
            }
                
               skill.element = element;
                
            }
            
            
            
        }
}
