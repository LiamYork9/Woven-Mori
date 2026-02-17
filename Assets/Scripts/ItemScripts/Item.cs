using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject
{
    public Sprite itemSprite;
    public string itemName;

    public string itemDescription;

    public int value;

    public int sellValue;

    public bool stackable;

    public int maxStack;
}
