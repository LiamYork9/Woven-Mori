using UnityEngine;
using MoriSkills;



public class Forge : MonoBehaviour
{
    public Equipment equipment;

    public EquipmentAttrTypes equipmentAttrTypes;

    [SerializeReference]
    public  SkillBoostAttr skillBoostAttr;

    public void Start()
    {
        skillBoostAttr = new SkillBoostAttr(4);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddAttr();
        }
    }
    public void AddAttr()
    {
        switch (equipmentAttrTypes)
        {
            case EquipmentAttrTypes.skillBoost: 
            equipment.AddAttr(skillBoostAttr);
            break;

            default: break;

        }
    }
}
