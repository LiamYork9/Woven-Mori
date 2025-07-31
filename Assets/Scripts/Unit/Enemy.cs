using UnityEngine;

public class Enemy : Unit
{

    public BattleManager battleManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        battleManager = FindAnyObjectByType<BattleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP <= 0)
        {
            gameObject.SetActive(false);
            ClearStats();
           

        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public void ClearStats()
    {
        unitName = "";
        chSprite = null;
        attack = 0;
        
        currentHP = maxHP;
        speed = 0;
    }
}
