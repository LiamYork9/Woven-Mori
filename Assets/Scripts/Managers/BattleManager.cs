using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
 using UnityEngine.UI;
using UnityEditor.Rendering;

public class BattleManager : MonoBehaviour
{
    public List<GameObject> playerSlots;

    public List<GameObject> enemySlots;

    public List<GameObject> defaultSlots;
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

    public TextMeshProUGUI dialogueText;

    public GameObject attackButton;




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
                    StartCoroutine(PlayerAttack());
                }
                selecting = false;
                Debug.Log("Player has Attacked " + target);
            }

            for (int i = 0; i < enemySlots.Count; i++)
            {

                if (enemySlots[i].GetComponent<Enemy>().currentHP <= 0)
                {
                    enemySlots.Remove(target);
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            enemySlots.Clear();
            for (int i = 0; i < defaultSlots.Count; i++)
            {
                enemySlots.Add(defaultSlots[i]);
            }
            BattleStart();
        }


    }

    void BattleStart()
    {
         enemySlots.Clear();
            for (int i = 0; i < defaultSlots.Count; i++)
            {
                enemySlots.Add(defaultSlots[i]);
            }
        
        for (int i = 0; i < playerSlots.Count; i++)
        {
            playerSlots[i].GetComponent<Image>().sprite = PartyManager.Instance.party[i].chSprite;
        }

        for (int i = 0; i < enemySlots.Count; i++)
        {
             
            enemySlots[i].SetActive(true);
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
        targetIndex = 0;
        targetArrow.SetActive(true);
        attacking = true;
        selecting = true;
        attackButton.SetActive(false);
        dialogueText.text = "Select Target" ;
        
    }

    IEnumerator PlayerAttack()
    {

        targetArrow.SetActive(false);
        target.GetComponent<Enemy>().currentHP -= 5;
        attacking = false;

        dialogueText.text = "Player has Attacked " + target;

        yield return new WaitForSeconds(2f);
        dialogueText.text = " ";
        attackButton.SetActive(true);
    }



   

    

    
}
