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

    public GameObject damageNumber;

    public bool noRunning = false;






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
                playerSelecting = false;
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
            playerSlots.Add(defaultPlayerSlots[i]);
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
            playerSlots.Add(defaultPlayerSlots[i]);
            playerSlots[i].SetActive(true);
            UnitBody temp = playerSlots[i].GetComponent<UnitBody>();
            if(PartyManager.Instance.party[i].currentHP <= 0)
            {
                PartyManager.Instance.party[i].currentHP = 1;
            }
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
    

    IEnumerator EnemyAttackCo(Skill skill, List<UnitBody> targets)
    {
        action = true;
        yield return new WaitForSeconds(2f);
        dialogueText.text = TurnOrderManager.Instance.turnPlayer.name + " used " + skill.name + " On" ;
        TurnOrderManager.Instance.turnPlayer.AP -= skill.cost;
        for (int i = 0; i < targets.Count; i++)
        {
            dialogueText.text += " " + targets[i].name;
           
            // Remeber to cross this bridge (self buff multiple times)
            skill.ApplyEffects(TurnOrderManager.Instance.turnPlayer,targets[i]);
        }
        yield return new WaitForSeconds(2f);
        dialogueText.text = " ";
        enemyTurn = false;
        action = false;
        TOM.EndTurn();
    }

    IEnumerator RunAwayCo()
    {
        ButtonsOff();
         int fleeNum = Random.Range(-5,11)+TurnOrderManager.Instance.turnPlayer.speed+TurnOrderManager.Instance.turnPlayer.level;
       int fastEnemy = 0;
        dialogueText.text = "You try to run away";
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < enemySlots.Count; i++)
            {
                if(enemySlots[i].GetComponent<UnitBody>().speed + enemySlots[i].GetComponent<UnitBody>().level > fastEnemy)
                {
                    fastEnemy = enemySlots[i].GetComponent<UnitBody>().speed + enemySlots[i].GetComponent<UnitBody>().level;
                }
            }
            if(fleeNum > fastEnemy)
            {
                for (int i = 0; i < PartyManager.Instance.party.Count; i++)
            {
                PlayerCharacter temp = PartyManager.Instance.party[i];

                for (int j = 0; j < defaultPlayerSlots[i].GetComponent<UnitBody>().conditions.Count; j++)
                {
                    defaultPlayerSlots[i].GetComponent<UnitBody>().conditions[j].RemoveCondition();
                }

                temp.CopyStats(defaultPlayerSlots[i].GetComponent<UnitBody>());
            }
                dialogueText.text = "You Escape";
                yield return new WaitForSeconds(1f);
                BattleEnd();
            }
            else
            {
                dialogueText.text = "Escape Failed";
                yield return new WaitForSeconds(1f);
                TOM.EndTurn();
            }



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

            for (int j = 0; j < defaultPlayerSlots[i].GetComponent<UnitBody>().conditions.Count; j++)
            {
                defaultPlayerSlots[i].GetComponent<UnitBody>().conditions[j].RemoveCondition();
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

            for (int j = 0; j < defaultPlayerSlots[i].GetComponent<UnitBody>().conditions.Count; j++)
            {
                defaultPlayerSlots[i].GetComponent<UnitBody>().conditions[j].RemoveCondition();
            }
          
            temp.CopyStats(defaultPlayerSlots[i].GetComponent<UnitBody>());
            temp.currentHP = temp.maxHP;
        }
        dialogueText.text = "You Lose";
        PlayerPrefs.DeleteAll();
        EncounterManager.Instance.fightArea = false;
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

    public void EnemyAttack(Skill skill, List<UnitBody> targets)
    {
        StartCoroutine(EnemyAttackCo(skill,targets));
    }

    public void RunAway()
    {
        StartCoroutine(RunAwayCo());
    }




}
