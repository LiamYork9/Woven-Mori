using System;
using System.Collections.Generic;
using MoriSkills;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public string unitName;
    public bool partyMember;

    public List<SkillId> skills;

    public Sprite chSprite;

    public Sprite deathSprite;

    public int attack;

    public int maxHP;

    public int currentHP;

    public int speed;

    public int initiative;

    public int slotNumber;
   
    void Start()
    {

    }


    void Update()
    {
        
    }

    public void CopyStats(Unit target)
    {
        unitName = target.unitName;
        slotNumber = target.slotNumber;
        partyMember = target.partyMember;
        chSprite = target.chSprite;
        deathSprite = target.deathSprite;
        attack = target.attack;
        maxHP = target.maxHP;
        currentHP = target.currentHP;
        speed = target.speed;
    }
}
