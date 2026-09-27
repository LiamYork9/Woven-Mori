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
        if(unitBody.unit != null && unitBody.downed == false)
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
        else
        {
            hpText.text = "";
        }
       
       

        if(unitBody.downed == true && unitBody.unit != null)
        {
            hpText.color = Color.red;
            hpText.text = unitBody.name + "  HP:" + CurrentHealth + "/" + MaxHealth;
        }
    }

    public void SetHpBar()
    {
        MaxHealth = unitBody.activeStats.MaxHP;
        CurrentHealth = unitBody.activeStats.CurrentHP;
    }
}
