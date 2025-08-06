using UnityEngine;

public class Unit : MonoBehaviour
{

    public string unitName;
    public bool partyMember;

    public Sprite chSprite;

    public int attack;

    public int maxHP;

    public int currentHP;

    public int speed;

    public int initiative;
   
    void Start()
    {

    }


    void Update()
    {
        
    }

    public void CopyStats(Unit target)
    {
        unitName = target.unitName;
        partyMember = target.partyMember;
        chSprite = target.chSprite;
        attack = target.attack;
        maxHP = target.maxHP;
        currentHP = target.currentHP;
        speed = target.speed;
    }
}
