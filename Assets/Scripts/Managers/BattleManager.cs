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
    public Sprite playerSprite;

    public Sprite enemySprite;
    private SpriteRenderer setSprite;

    public TMP_Text gTurnText;

    public GameObject targetArrow;

    public GameObject target;

    public bool selecting;

    public int targetIndex = 0;

    public bool attacking;

   


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
        
        if (selecting == true)
        {

            target = enemySlots[targetIndex];
            targetArrow.transform.position = target.transform.position;
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (targetIndex + 1 >= enemySlots.Count)
                {
                    targetIndex = 0;
                }
                else
                {
                    targetIndex++;
                }
                
                target = enemySlots[targetIndex];
                targetArrow.transform.position = target.transform.position;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (targetIndex - 1 < 0)
                {
                    targetIndex = enemySlots.Count - 1;
                }
                else
                {
                    targetIndex--;
                }
                    target = enemySlots[targetIndex];
                    targetArrow.transform.position = target.transform.position;
                
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (attacking == true)
                {
                    target.GetComponent<Enemy>().currentHP -= 5;
                    attacking = false;
                }
                selecting = false;
                Debug.Log("Player has Attacked " + target);
            }
        }
        
    
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
            Enemy temp = enemySlots[i].GetComponent<Enemy>();
            temp.CopyStats(RollEnemy());
            enemySlots[i].GetComponent<Image>().sprite = temp.chSprite;
        }

    }

    public Enemy RollEnemy()
    {
        return EncounterManager.Instance.encounterPool[Random.Range(0, EncounterManager.Instance.encounterPool.Count)];

    }

    public void Attack()
    {
        targetArrow.SetActive(true);
        attacking = true;
        selecting = true;
        ChooseTarget();
        
        
    }

    public void ChooseTarget()
    {
        // int targetIndex = 0;
        // target = enemySlots[targetIndex];
        // targetArrow.transform.position = target.transform.position;
        // while (selecting == true)
        // {

        //     target = enemySlots[targetIndex];
        //     targetArrow.transform.position = target.transform.position;
        //     if (Input.GetKeyDown(KeyCode.A))
        //     {
        //         targetIndex++;
        //         target = enemySlots[targetIndex];
        //         targetArrow.transform.position = target.transform.position;
        //     }
        //     if (Input.GetKeyDown(KeyCode.D))
        //     {
        //         targetIndex--;
        //         target = enemySlots[targetIndex];
        //         targetArrow.transform.position = target.transform.position;
        //     }

        //     if (Input.GetKeyDown(KeyCode.Space))
        //     {
        //         selecting = false;
        //         Debug.Log("Player has Attacked " + target);
        //     }
        // }

    }

   

    

    
}
