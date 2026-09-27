using UnityEngine;



[CreateAssetMenu(fileName = "ItemAttr", menuName = "ScriptableObjects/ItemAttr")]
public class ItemAttr : ScriptableObject
{
    public string attrName;

    public ItemAttr()
    {
        attrName = "Generic Attr";
    }

     public virtual void ActivateItemAttrOFB(PlayerCharacter target)
    {

    }
    public virtual void ActivateItemAttrWIB(UnitBody target)
    {

    }

}
