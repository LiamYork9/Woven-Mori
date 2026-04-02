using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MoriSkills;
using UnityEngine.SceneManagement;


public class WinScreenUI : MonoBehaviour
{
    public TMP_Text moneyText;

    public TMP_Text expText;

    public TMP_Text lootText;

    public string sceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "Money Earned: " + BattleManager.Instance.cashEarned;

        expText.text = "EXP Earned: " + BattleManager.Instance.expEarned;

         lootText.text = "Loot Droped: ";

        for (int i = 0; i < BattleManager.Instance.loot.Count; i++)
        {

            lootText.text += BattleManager.Instance.loot[i].item.itemName + ": X " + BattleManager.Instance.loot[i].amount;
            if (i + 1 != BattleManager.Instance.loot.Count)
            {
                lootText.text+= ", ";
            }

        }
    }

    public void SetText()
    {
        moneyText.text = "Money Earned: " + BattleManager.Instance.cashEarned;

        expText.text = "EXP Earned: " + BattleManager.Instance.expEarned;

        lootText.text = "Loot Droped" + BattleManager.Instance.loot;
    }

    public void returnScene()
    {
        BattleManager.Instance.cashEarned = 0;
        BattleManager.Instance.expEarned = 0;
         SceneManager.LoadScene(sceneName);
    }
}
