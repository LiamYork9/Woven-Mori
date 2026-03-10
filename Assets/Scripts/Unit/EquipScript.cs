using UnityEngine;

public class EquipScript : MonoBehaviour
{
    public Weapon weapon; 

    public Armor armor;

    public Accessory accessory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PartyEquipWeapon()
    {
        for(int i = 0; i < PartyManager.Instance.party.Count; i++)
        {
            PartyManager.Instance.party[i].EquipGear(weapon);
        }
    }
    public void PartyEquipArmor()
    {
         for(int i = 0; i < PartyManager.Instance.party.Count; i++)
        {
            PartyManager.Instance.party[i].EquipGear(armor);
        }
    }
    public void PartyEquipAccessory()
    {
         for(int i = 0; i < PartyManager.Instance.party.Count; i++)
        {
            PartyManager.Instance.party[i].EquipGear(accessory);
        }
    }
    
}
