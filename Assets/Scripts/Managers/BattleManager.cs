using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MoriSkills;
using UnityEngine.SceneManagement;

public enum FightState
{
    Active,
    Lost,
    Won,
    Scripted
}

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

    public TurnOrderManager TOM;

    public SkillButtonScript SBS;

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

    public bool targetParty;

    public bool targetSelf;

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

    public FightState fightState = FightState.Active;

    public bool win;
    public string sceneName = "EncounterTest";





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
        TOM = TurnOrderManager.Instance;
        if (EncounterManager.Instance.encounteredEnemies.Count > 0)
        {
            BattleStartFromScene();
        }
        else
        {
            BattleStart();
        }
        ButtonsOff();
    }

    public void Update()
    {
        if(win && Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene(sceneName);
        }
        //Move later for optemization 
        for (int i = 0; i < enemySlots.Count; i++)
        {
            enemySlots[i].GetComponent<UnitBody>().slotNumber = i;
        }

        for (int i = 0; i < playerSlots.Count; i++)
        {
            playerSlots[i].GetComponent<UnitBody>().slotNumber = i;
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
                    selecting = false;
                    StartCoroutine(PlayerAttack());
                }
               
              

            }

            for (int i = 0; i < enemySlots.Count; i++)
            {

                if (enemySlots[i].GetComponent<UnitBody>().currentHP <= 0)
                {
                    enemySlots.Remove(enemySlots[i]);
                }

            }





        }

        if (targetSelf == true)
        {
            target = null;
        }

        if (playerSelecting == true)
        {
            targetArrow.SetActive(true);
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

            if (playerSlots[i].GetComponent<UnitBody>().currentHP <= 0)
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
            TOM.downedPlayers.Clear();

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
                multiTarget = false;
                targetSelf = false;
                actionMenu.SetActive(true);
                ButtonsOn();
                targetArrow.SetActive(false);
            }
            if (selecting == true)
            {
                targetArrow.SetActive(false);
                if (usingSkill == true)
                {
                    skillMenu.SetActive(true);
                }
                else
                {
                    ButtonsOn();
                }
              
                selecting = false;
                attacking = false;
                dialogueText.text = "";
            }
        }



    }

    // The funcation that will be called at the start of every fight 
    void BattleStart()
    {
        win = false;
        Debug.Log("Ol Reliable");
        globalTurn = 1;
        gTurnText.text = "Turn: " + globalTurn;
        //fightState = FightState.Active;
        dialogueText.text = " ";
        enemySlots.Clear();
        //playerSlots.Clear();
        TOM.allFighters.Clear();
        TOM.recentTurns.Clear();
        TOM.cycle = 0;
        for (int i = 0; i < defaultSlots.Count; i++)
        {
            enemySlots.Add(defaultSlots[i]);
        }

        for (int i = 0; i < PartyManager.Instance.party.Count; i++)
        {

            playerSlots[i].SetActive(true);
            UnitBody temp = playerSlots[i].GetComponent<UnitBody>();
            temp.SetUnit(PartyManager.Instance.party[i]);
            playerSlots[i].GetComponent<Image>().sprite = temp.chSprite;
        }

        for (int i = 0; i < enemySlots.Count; i++)
        {

            enemySlots[i].SetActive(true);
            UnitBody temp = enemySlots[i].GetComponent<UnitBody>();
            temp.SetUnit(RollEnemy());
            enemySlots[i].GetComponent<Image>().sprite = temp.chSprite;

        }
        TOM.GatherFighters();
        TOM.turnPlayer = TOM.turnOrder[0].unit;
        TOM.turnOrder[0].StartTurn();

    }


    public void BattleStartFromScene()
    {
        win =false;
        Debug.Log("FromScene");
        globalTurn = 1;
        gTurnText.text = "Turn: " + globalTurn;
        //fightState = FightState.Active;
        dialogueText.text = " ";
        enemySlots.Clear();
        //playerSlots.Clear();
        TOM.allFighters.Clear();
        TOM.recentTurns.Clear();
        TOM.cycle = 0;
        for (int i = 0; i < defaultSlots.Count; i++)
        {
            if (i < EncounterManager.Instance.encounteredEnemies.Count)
            {
                enemySlots.Add(defaultSlots[i]);
            }
            else
            {
                defaultSlots[i].SetActive(false);
            }
        }

        for (int i = 0; i < PartyManager.Instance.party.Count; i++)
        {

            playerSlots[i].SetActive(true);
            UnitBody temp = playerSlots[i].GetComponent<UnitBody>();
            temp.SetUnit(PartyManager.Instance.party[i]);
            playerSlots[i].GetComponent<Image>().sprite = temp.chSprite;
        }

        for (int i = 0; i < EncounterManager.Instance.encounteredEnemies.Count; i++)
        {

            enemySlots[i].SetActive(true);
            UnitBody temp = enemySlots[i].GetComponent<UnitBody>();
            temp.SetUnit(EncounterManager.Instance.encounteredEnemies[i]);
            enemySlots[i].GetComponent<Image>().sprite = temp.chSprite;

        }
        TOM.GatherFighters();
        TOM.turnPlayer = TOM.turnOrder[0].unit;
        TOM.turnOrder[0].StartTurn();

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
        targetIndex = 0;
        usingSkill = true;
        actionMenu.SetActive(false);
        //SkillButtonScript.ButtonsOn();
        ButtonsOff();
        skillMenu.SetActive(true);
        SBS.ButtonsOn();
        SBS.dialogueText.text = " ";

    }



    IEnumerator PlayerAttack()
    {

        targetArrow.SetActive(false);
        dialogueText.text = TOM.turnPlayer.name + " has Attacked " + target.GetComponent<UnitBody>().name;
        yield return new WaitForSeconds(2f);
        SkillMaker.Instance.GetById(SkillId.Attack).ApplyEffects(TurnOrderManager.Instance.turnPlayer,target.GetComponent<UnitBody>());
        yield return new WaitForSeconds(2f);
        TOM.EndTurn();


    }
    

    IEnumerator EnemyAttackCo()
    {
        action = true;
        enemyTarget = playerSlots[Random.Range(0,playerSlots.Count)];
        yield return new WaitForSeconds(2f);
        SkillMaker.Instance.GetById(SkillId.Attack).ApplyEffects(TurnOrderManager.Instance.turnPlayer,enemyTarget.GetComponent<UnitBody>());
        dialogueText.text =  TOM.turnPlayer.name + " has attacked " + enemyTarget.GetComponent<UnitBody>().name;
        yield return new WaitForSeconds(1f);
        dialogueText.text = " ";
        enemyTurn = false;
        action = false;
        TOM.EndTurn();
    }

    public void EnemyTurn()
    {
        playerTurn = false;
        ButtonsOff();
    }



    // What shows up when you Win
    public void WinCondtion()
    {
        fightState = FightState.Won;
        ButtonsOff();
        for (int i = 0; i < PartyManager.Instance.party.Count; i++)
        {
            PlayerCharacter temp = PartyManager.Instance.party[i];

            for (int j = 0; j < temp.conditions.Count; j++)
            {
                temp.conditions[j].RemoveCondition();
            }

            temp.CopyStats(defaultPlayerSlots[i].GetComponent<UnitBody>());
        }
        dialogueText.text = "You Win!";

        win = true;
        BattleEnd();
    }

    // What happens when you lose

    public void LoseCondition()
    {
        fightState = FightState.Lost;
        ButtonsOff();
        for (int i = 0; i < PartyManager.Instance.party.Count; i++)
        {
            PlayerCharacter temp = PartyManager.Instance.party[i];

            for (int j = 0; j < temp.conditions.Count; j++)
            {
                temp.conditions[j].RemoveCondition();
            }

            temp.CopyStats(defaultPlayerSlots[i].GetComponent<UnitBody>());
        }
        dialogueText.text = "You Lose";
        BattleEnd();
    }

    public void BattleEnd()
    {
        StartCoroutine(EndBattle());
    }
    
     IEnumerator EndBattle()
    {
        yield return new WaitForSeconds(1.5f);
        dialogueText.text = "The Battle is Over";
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
         
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
        TOM.turnPlayer = TOM.turnOrder[0].unit;
        if (TOM.turnPlayer != null && TOM.turnPlayer.partyMember == false)
        {
            enemyTurn = true;
            //EnemyTurn();
            playerTurn = false;

        }


        if (TOM.turnPlayer != null && TOM.turnPlayer.partyMember == true)
        {
            playerTurn = true;
            enemyTurn = false;

        }

        skillButton.SetSkillButtons();
        TOM.turnOrder[0].StartTurn();
        
    }

    public void DamagePlayer()
    {
        enemyTarget.GetComponent<UnitBody>().currentHP -= TOM.turnPlayer.attack;

        if (enemyTarget.GetComponent<UnitBody>().currentHP <= 0)
        {
            enemyTarget.GetComponent<UnitBody>().Death();
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
