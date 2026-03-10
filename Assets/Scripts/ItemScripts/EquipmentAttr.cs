using UnityEngine;

namespace MoriSkills 
{
    public enum EquipmentAttrTypes
    {
        none,
        skillBoost
    }
     [System.Serializable]

     [CreateAssetMenu(fileName = "EquipmentAttr", menuName = "ScriptableObjects/EqupmentAttr")]
    public class EquipmentAttr : ScriptableObject
    {
        public bool basicAttackAttr = false;
        public string attrName;

        public EquipmentAttr()
        {
            attrName = "Generic Attr";
           

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

   

    

     
}
