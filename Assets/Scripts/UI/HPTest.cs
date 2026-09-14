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
        hpText.text = unitBody.name + "  HP:" + CurrentHealth + "/" + MaxHealth;

        if(TurnOrderManager.Instance.turnPlayer == unitBody)
        {
            hpText.color = Color.yellow;
        }
        else
        {
            hpText.color = Color.white;
        }
    }

    public void SetHpBar()
    {
        MaxHealth = unitBody.activeStats.MaxHP;
        CurrentHealth = unitBody.activeStats.CurrentHP;
    }
}
