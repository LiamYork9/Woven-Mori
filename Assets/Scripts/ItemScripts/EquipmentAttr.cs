using UnityEngine;

namespace MoriSkills
{
    public enum EquipmentAttrTypes
    {
        none,
        skillBoost
    }
     [System.Serializable]
    public class EquipmentAttr 
    {
        public string name;

        public EquipmentAttr()
        {
            name = "Generic Attr";
           

        }

        public virtual EquipmentAttr ShallowCopy()
        {
            return (EquipmentAttr)this.MemberwiseClone();
        }

        public virtual void ActivateAttr(UnitBody user)
        {

        }

        public virtual void ActivateOnSkill(Skill skill)
        {
            
        }
    
        
    }

    public class SkillBoostAttr: EquipmentAttr
    {
        public int boostValue;

        public SkillBoostAttr(int boost)
        {
            name = "Skill Boost";
            boostValue = boost;
        }

        public override void ActivateOnSkill(Skill skill)
        {
            skill.power += boostValue;
        }
    }
}
