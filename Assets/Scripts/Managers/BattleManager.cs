using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MoriSkills;



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

    public bool multiTarget;

    public bool selecting;

    public bool playerSelecting;

    public int targetIndex = 0;

    public bool attacking;

    public TextMeshProUGUI dialogueText;

    public List<GameObject> buttons;

    public bool enemyTurn;

    public bool playerTurn;

    public bool action;

    public SkillButtonScript skillButton;

    public GameObject skillMenu;

    public GameObject actionMenu;

    public bool usingSkill;





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
        //Move later for optemization 
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

        //Put in Take Damage Function 
        for (int i = 0; i < playerSlots.Count; i++)
        {

            if (playerSlots[i].GetComponent<PlayerCharacter>().currentHP <= 0)
            {
                Debug.Log("player died");
                playerSlots.Remove(enemyTarget);
            }
        }

        //For testing will be reomved later
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


        // Will go into a death function later
        if (playerSlots.Count == 0)
        {
            LoseCondition();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (usingSkill == true)
            {
                skillMenu.SetActive(false);
                usingSkill = false;
                actionMenu.SetActive(true);
            }
            if (selecting == true)
            {
                targetArrow.SetActive(false);
                ButtonsOn();
                selecting = false;
                attacking = false;
                dialogueText.text = "";
            }
        }



    }

    // The funcation that will be called at the start of every fight 
    void BattleStart()
    {
        globalTurn = 1;

        dialogueText.text = " ";
        enemySlots.Clear();
        //playerSlots.Clear();
        TurnOrderManager.Instance.allFighters.Clear();
        TurnOrderManager.Instance.recentTurns.Clear();
        TurnOrderManager.Instance.cycle = 0;
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
        TurnOrderManager.Instance.turnPlayer = TurnOrderManager.Instance.turnOrder[0].unit;
        TurnOrderManager.Instance.turnOrder[0].StartTurn();

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

    public void SkillMenu()
    {
        usingSkill = true;
        actionMenu.SetActive(false);
        ButtonsOff();
        skillMenu.SetActive(true);

    }



    IEnumerator PlayerAttack()
    {

        targetArrow.SetActive(false);
        target.GetComponent<Enemy>().currentHP -= TurnOrderManager.Instance.turnPlayer.attack;
        if (target.GetComponent<Enemy>().currentHP <= 0)
        {
            target.GetComponent<Enemy>().Death();
        }
        dialogueText.text = "Player has Attacked " + target.GetComponent<Enemy>().unitName;
        yield return new WaitForSeconds(2f);
        if (enemySlots.Count == 0)
        {
            WinCondtion();
        }
        else
        {
            TurnOrderManager.Instance.EndTurn();

        }

    }

    IEnumerator EnemyAttackCo()
    {
        action = true;
        enemyTarget = playerSlots[0];
        yield return new WaitForSeconds(2f);
        DamagePlayer();
        dialogueText.text = "Enemy has Attacked " + enemyTarget.GetComponent<PlayerCharacter>().unitName;
        yield return new WaitForSeconds(1f);
        dialogueText.text = " ";
        enemyTurn = false;
        action = false;
        TurnOrderManager.Instance.EndTurn();



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


        if (TurnOrderManager.Instance.turnPlayer != null && TurnOrderManager.Instance.turnPlayer.partyMember == true)
        {
            playerTurn = true;

        }

        if (playerTurn == true && attacking == false && usingSkill == false)
        {
            //ButtonsOn();
        }

        skillButton.SetSkillButtons();
    }

    public void DamagePlayer()
    {
        enemyTarget.GetComponent<PlayerCharacter>().currentHP -= TurnOrderManager.Instance.turnPlayer.attack;

        if (enemyTarget.GetComponent<PlayerCharacter>().currentHP <= 0)
        {
            enemyTarget.GetComponent<PlayerCharacter>().Death();
        }
    }

    public void StartStartTurnCo(Turn turn)
    {
        StartCoroutine(turn.StartTurnCo());
    }

    public void StartSelectActionCo(Turn turn)
    {
        if (turn.unit.partyMember == true)
        {
            StartCoroutine(turn.SelectActionCo());
        }
        else
        {
            StartCoroutine(turn.EnemyActionCo());
        }

    }

    public void StartEndTurnCo(Turn turn)
    {
        StartCoroutine(turn.EndTurnCo());
    }

    public void EnemyAttack()
    {
        StartCoroutine(EnemyAttackCo());
    }



}
