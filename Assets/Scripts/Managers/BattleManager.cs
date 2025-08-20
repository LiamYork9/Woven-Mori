using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
 using UnityEngine.UI;



public class BattleManager : MonoBehaviour
{
    public List<GameObject> playerSlots;

    public List<GameObject> enemySlots;

    public List<GameObject> defaultSlots;

    public List<GameObject> defaultPlayerSlots;
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

    public GameObject enemyTarget;

    public bool selecting;

    public bool playerSelecting;

    public int targetIndex = 0;

    public bool attacking;

    public TextMeshProUGUI dialogueText;

    public List<GameObject> buttons;

    public bool enemyTurn;

    public bool playerTurn;

    public bool action;


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
        ButtonsOff();
    }

    public void Update()
    {
         gTurnText.text = "Turn: " + globalTurn;
        for (int i = 0; i < enemySlots.Count; i++)
        {
            enemySlots[i].GetComponent<Unit>().slotNumber = i;
        }

        for (int i = 0; i < playerSlots.Count; i++)
        {
            playerSlots[i].GetComponent<Unit>().slotNumber = i;
        }



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

        if (playerSelecting == true)
        {
            target = playerSlots[targetIndex];
            targetArrow.transform.position = target.transform.position;
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (targetIndex + 1 >= playerSlots.Count)
                {
                    targetIndex = 0;
                }
                else
                {
                    targetIndex++;
                }

                target = playerSlots[targetIndex];
                targetArrow.transform.position = target.transform.position;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (targetIndex - 1 < 0)
                {
                    targetIndex = playerSlots.Count - 1;
                }
                else
                {
                    targetIndex--;
                }
                target = playerSlots[targetIndex];
                targetArrow.transform.position = target.transform.position;

            }
        }

        for (int i = 0; i < playerSlots.Count; i++)
        {

            if (playerSlots[i].GetComponent<PlayerCharacter>().currentHP <= 0)
            {
                Debug.Log("player died");
                playerSlots.Remove(enemyTarget);
            }
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            enemySlots.Clear();
            playerSlots.Clear();
            for (int i = 0; i < defaultSlots.Count; i++)
            {
                enemySlots.Add(defaultSlots[i]);
            }
            for (int i = 0; i < defaultPlayerSlots.Count; i++)
            {
                playerSlots.Add(defaultPlayerSlots[i]);
            }
            BattleStart();
            TurnOrderManager.Instance.downedPlayers.Clear();

        }

        if (action == false)
        {
            TurnTransiton();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            WinCondtion();
        }

        if (playerSlots.Count == 0)
        {
            LoseCondition();
        }



    }

    // The funcation that will be called at the start of every fight 
    void BattleStart()
    {

        dialogueText.text = " ";
        enemySlots.Clear();
        //playerSlots.Clear();
        TurnOrderManager.Instance.allFighters.Clear();
        TurnOrderManager.Instance.recentTurns.Clear();
        for (int i = 0; i < defaultSlots.Count; i++)
        {
            enemySlots.Add(defaultSlots[i]);
        }

        for (int i = 0; i < PartyManager.Instance.party.Count; i++)
        {

            playerSlots[i].SetActive(true);
            PlayerCharacter temp = playerSlots[i].GetComponent<PlayerCharacter>();
            temp.CopyStats(PartyManager.Instance.party[i]);
            playerSlots[i].GetComponent<Image>().sprite = temp.chSprite;
        }

        for (int i = 0; i < enemySlots.Count; i++)
        {

            enemySlots[i].SetActive(true);
            Enemy temp = enemySlots[i].GetComponent<Enemy>();
            temp.CopyStats(RollEnemy());
            enemySlots[i].GetComponent<Image>().sprite = temp.chSprite;

        }
        TurnOrderManager.Instance.GatherFighters();


    }

    //Where we roll our Enimies 
    public Enemy RollEnemy()
    {
        return EncounterManager.Instance.encounterPool[Random.Range(0, EncounterManager.Instance.encounterPool.Count)];

    }

    //Player Attacking function
    public void Attack()
    {
        targetIndex = 0;
        targetArrow.SetActive(true);
        attacking = true;
        selecting = true;
        action = false;
        ButtonsOff();
        dialogueText.text = "Select Target";

    }

    IEnumerator PlayerAttack()
    {

        targetArrow.SetActive(false);
        target.GetComponent<Enemy>().currentHP -= TurnOrderManager.Instance.turnPlayer.attack;

           globalTurn += 1;
        dialogueText.text = "Player has Attacked " + target.GetComponent<Enemy>().unitName;

        yield return new WaitForSeconds(2f);
        if (enemySlots.Count == 0)
        {
            WinCondtion();
        }
        else
        {
            yield return new WaitForSeconds(2f);
            TurnOrderManager.Instance.recentTurns.Insert(0, TurnOrderManager.Instance.turnPlayer);
            TurnOrderManager.Instance.turnOrder.Remove(TurnOrderManager.Instance.turnPlayer);
            yield return new WaitForSeconds(1f);
            dialogueText.text = " It is now " + TurnOrderManager.Instance.turnPlayer.unitName + "Turn";
            yield return new WaitForSeconds(1f);
            dialogueText.text = " ";
            attacking = false;
            playerTurn = false;
            TurnTransiton();

        }

    }

    IEnumerator EnemyAttack()
    {
        action = true;
        enemyTarget = playerSlots[0];
        globalTurn += 1;
        yield return new WaitForSeconds(2f);
        DamagePlayer();
        TurnOrderManager.Instance.recentTurns.Insert(0, TurnOrderManager.Instance.turnPlayer);
        TurnOrderManager.Instance.turnOrder.Remove(TurnOrderManager.Instance.turnPlayer);
        dialogueText.text = "Enemy has Attacked " + enemyTarget.GetComponent<PlayerCharacter>().unitName;
        yield return new WaitForSeconds(1f);
        dialogueText.text = " It is now " + TurnOrderManager.Instance.turnPlayer.unitName + "Turn";
        yield return new WaitForSeconds(1f);
        dialogueText.text = " ";
        enemyTurn = false;
        action = false;
        TurnTransiton();



    }

    public void EnemyTurn()
    {
        playerTurn = false;
        ButtonsOff();
    }



    // What shows up when you Win
    public void WinCondtion()
    {
        ButtonsOff();
        for (int i = 0; i < PartyManager.Instance.party.Count; i++)
        {
            PlayerCharacter temp = PartyManager.Instance.party[i];
            
            temp.CopyStats(defaultPlayerSlots[i].GetComponent<PlayerCharacter>());
        }
        dialogueText.text = "You Win!";
    }

    // What happens when you lose

    public void LoseCondition()
    {
        ButtonsOff();
        dialogueText.text = "You Lose";
    }

    public void ButtonsOn()
    {
        foreach (var obj in buttons)
            obj.SetActive(true);
    }

    public void ButtonsOff()
    {
        foreach (var obj in buttons)
            obj.SetActive(false);
    }

    public void TurnTransiton()
    {
        if (TurnOrderManager.Instance.turnPlayer != null && TurnOrderManager.Instance.turnPlayer.partyMember == false)
        {
            enemyTurn = true;
            EnemyTurn();
        }

        if (enemyTurn == true && action == false)
        {
            StartCoroutine(EnemyAttack());
            ButtonsOff();
        }

        if (TurnOrderManager.Instance.turnPlayer != null && TurnOrderManager.Instance.turnPlayer.partyMember == true)
        {
            playerTurn = true;
        }

        if (playerTurn == true && attacking == false)
        {
            ButtonsOn();
        }
    }

    public void DamagePlayer()
    {
        enemyTarget.GetComponent<PlayerCharacter>().currentHP -= TurnOrderManager.Instance.turnPlayer.attack;

        if (enemyTarget.GetComponent<PlayerCharacter>().currentHP <= 0)
        {
            enemyTarget.GetComponent<PlayerCharacter>().Death();
        }
    }









}
