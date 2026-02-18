using UnityEngine;
using MoriSkills;

[CreateAssetMenu(fileName = "ElementBoostAttr", menuName = "ScriptableObjects/EqupmentAttr/ElementBoostAttr")]
public class ElementBoostAttr : EquipmentAttr
{
    

    
    
        public int boostValue;
        public Element element;
        public ElementBoostAttr(int boost)
        {
            name = "Element Boost";
            boostValue = boost;
        }

        public override void ActivateOnSkill(Skill skill)
        {     

             if(basicAttackAttr == true)
            {
                
               if(skill.element == element)
                {
                    skill.power += boostValue;
                }
                
            }
            else if(skill.skillId != SkillId.Attack)
            {
               if(skill.element == element)
                {
                    skill.power += boostValue;
                }
            }
            
        }
    
}
