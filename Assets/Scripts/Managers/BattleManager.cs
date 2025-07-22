using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
 using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public List<GameObject> playerSlots;

    public List<GameObject> enemySlots;
    public static BattleManager Instance;

    public static BattleManager GetInstance()
    {
        return Instance;
    }

    public int globalTurn;
    public Transform pfCharacterBattle;

    public SpriteRenderer teamColor;

    public Sprite playerSprite;

    public Sprite enemySprite;
    private SpriteRenderer setSprite;

    public TMP_Text gTurnText;


    public void Awake()
    {
        if (BattleManager.Instance != this && BattleManager.Instance != null)
        {
            Destroy(BattleManager.Instance);
            Instance = this;
        }
        else
        {
            Instance = this;
        }

    }


    void Start()
    {
        BattleStart();
    }

    public void Update()
    {

    }

    void BattleStart()
    {
        for (int i = 0; i < playerSlots.Count; i++)
        {
            playerSlots[i].GetComponent<Image>().sprite = PartyManager.Instance.party[i].chSprite;
        }

        for (int i = 0; i < enemySlots.Count; i++)
        {
            //enemySlots[i].AddComponent<Enemy>().
            enemySlots[i].GetComponent<Image>().sprite = RollEnemy().chSprite;
        }

    }

    public Enemy RollEnemy()
    {
        return EncounterManager.Instance.encounterPool[Random.Range(0, EncounterManager.Instance.encounterPool.Count)];
        
    }

   

    

    
}
