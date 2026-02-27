using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HPTest : MonoBehaviour
{
    public UnitBody unitBody;
    public TMP_Text hpText;

    public int MaxHealth;

    public int CurrentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = "HP:" + CurrentHealth + "/" + MaxHealth;
    }

    public void SetHpBar()
    {
        MaxHealth = unitBody.maxHP;
        CurrentHealth = unitBody.currentHP;
    }
}
