using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using MoriSkills;
using Unity.VisualScripting;


public class SkillButtonScript : MonoBehaviour
{
    public List<GameObject> skillButtons;

    public TMP_Text toolTip;

    public int page = 0;

    public TextMeshProUGUI dialogueText;

    public Skill selectedSkill;



    void Start()
    {
        
        for (int i = 0; i < skillButtons.Count; i++)
        {
            if (skillButtons[i].GetComponent<ToolTipSkill>() != null)
            {
                skillButtons[i].GetComponent<ToolTipSkill>().hoverEvent.AddListener(ToolTipAdder);
                skillButtons[i].GetComponent<ToolTipSkill>().unHoverEvent.AddListener(ToolTipRemover);
            }
        }

        SetSkillButtons();
    }


    void Update()
    {
        if (BattleManager.Instance.multiTarget == true)
        {
            dialogueText.text = "Skill will hit all enemies";
        }
        if (BattleManager.Instance.targetSelf == true)
        {
            dialogueText.text = "Skill will target user";
        }
         if (BattleManager.Instance.playerSelecting == true)
        {
            dialogueText.text = "Choose target";
        }
         if (BattleManager.Instance.targetParty == true)
        {
            dialogueText.text = "Skill will target whole party";
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {


            if (BattleManager.Instance.usingSkill == true && BattleManager.Instance.multiTarget == false && BattleManager.Instance.targetSelf == false && BattleManager.Instance.playerSelecting == false && BattleManager.Instance.targetParty == false && BattleManager.Instance.selecting == true )
            {
                BattleManager.Instance.targetArrow.SetActive(false);
                BattleManager.Instance.selecting = false;
                ActivateSkill(selectedSkill);
               
            }

             BattleManager.Instance.selecting = false;

            if (BattleManager.Instance.usingSkill == true && BattleManager.Instance.multiTarget == true)
            {
                List<Unit> temp = new List<Unit> { };
                for (int i = 0; i < BattleManager.Instance.enemySlots.Count; i++)
                {
                    temp.Add(BattleManager.Instance.enemySlots[i].GetComponent<Unit>());
                }
                ActivateMultiSkill(selectedSkill, temp);
                BattleManager.Instance.multiTarget = false;
            }

            if (BattleManager.Instance.usingSkill == true && BattleManager.Instance.targetParty == true)
            {
                List<Unit> temp = new List<Unit> { };
                for (int i = 0; i < BattleManager.Instance.playerSlots.Count; i++)
                {
                    temp.Add(BattleManager.Instance.playerSlots[i].GetComponent<Unit>());
                }
                ActivateMultiSkill(selectedSkill, temp);
                BattleManager.Instance.targetParty = false;
            }

            if (BattleManager.Instance.usingSkill == true && BattleManager.Instance.targetSelf == true)
            {
                ActivateSkillSelf(selectedSkill);
                BattleManager.Instance.targetSelf = false;
            }

            if (BattleManager.Instance.usingSkill == true && BattleManager.Instance.playerSelecting == true)
            {
                BattleManager.Instance.playerSelecting = false;
                ActivateSkillAlly(selectedSkill);

            }
        }
    }

    public void ToolTipAdder(GameObject button)
    {
        toolTip.text = SkillMaker.Instance.GetById(button.GetComponent<ToolTipSkill>().skillId).toolTip;
    }
    public void ToolTipRemover()
    {
        toolTip.text = " ";
    }

    public void SetSkillButtons()
    {
        for (int i = 0; i < skillButtons.Count; i++)
        {
            if (TurnOrderManager.Instance.turnPlayer != null && TurnOrderManager.Instance.turnPlayer.partyMember == true)
            {
                if ((i + page) < TurnOrderManager.Instance.turnPlayer.skills.Count && TurnOrderManager.Instance.turnPlayer.skills[i + page] != SkillId.None)
                {
                    skillButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = TurnOrderManager.Instance.turnPlayer.skills[i + page].ToString();
                    skillButtons[i].GetComponent<ToolTipSkill>().skillId = TurnOrderManager.Instance.turnPlayer.skills[i + page];

                }
                else
                {
                    skillButtons[i].SetActive(false);
                }


            }
        }
    }

    public void ActivateSkill(Skill skill)
    {
         BattleManager.Instance.ButtonsOff();
       
            ButtonsOff();
            if (BattleManager.Instance.selecting == false)
            {
                StartCoroutine(PlayerSkill(skill));
                toolTip.text = "";
                TurnOrderManager.Instance.turnPlayer.AP -= skill.cost;
            }

        



    }
    
    public void ActivateSkillAlly(Skill skill)
    {
         BattleManager.Instance.ButtonsOff();
         BattleManager.Instance.targetArrow.SetActive(false);
       
            ButtonsOff();
            if (BattleManager.Instance.playerSelecting == false)
            {
                StartCoroutine(PlayerSkillAlly(skill));
                toolTip.text = "";
                TurnOrderManager.Instance.turnPlayer.AP -= skill.cost;
            }

        



    }

    public void ActivateSkillSelf(Skill skill)
    {
        BattleManager.Instance.ButtonsOff();
        ButtonsOff();

        StartCoroutine(PlayerSkillSelf(skill));
        toolTip.text = "";
        TurnOrderManager.Instance.turnPlayer.AP -= skill.cost;


    }

    public void ActivateMultiSkill(Skill skill, List<Unit> targets)
    {
        BattleManager.Instance.ButtonsOff();
       
        ButtonsOff();
        if (BattleManager.Instance.selecting == false)
        {
            StartCoroutine(PlayerMultSkill(skill, targets));
            toolTip.text = "";
            TurnOrderManager.Instance.turnPlayer.AP -= skill.cost;
        }
    }

    public void ActivateMultiSkillParty(Skill skill, List<Unit> targets)
    {
        BattleManager.Instance.ButtonsOff();
       
        ButtonsOff();
        if (BattleManager.Instance.selecting == false)
        {
            StartCoroutine(PlayerMultSkill(skill, targets));
            toolTip.text = "";
            TurnOrderManager.Instance.turnPlayer.AP -= skill.cost;
        }
    }

    IEnumerator PlayerSkill(Skill skill)
    {
        BattleManager.Instance.usingSkill = false;
        if (selectedSkill.power != 0)
        {
            BattleManager.Instance.target.GetComponent<Unit>().TakeDamage(selectedSkill.power,selectedSkill.category,selectedSkill.element);
        }
        skill.ApplyEffects(TurnOrderManager.Instance.turnPlayer,BattleManager.Instance.target.GetComponent<Unit>());
        dialogueText.text =  TurnOrderManager.Instance.turnPlayer.unitName + " used " + skill.name + " On " + BattleManager.Instance.target.name;
        yield return new WaitForSeconds(2f);
        TurnOrderManager.Instance.turnOrder[0].turnShift = skill.turnShift;
        dialogueText.text = "";
        BattleManager.Instance.actionMenu.SetActive(true);
        ButtonsOn();
        BattleManager.Instance.skillMenu.SetActive(false);
        selectedSkill = null;
        TurnOrderManager.Instance.EndTurn();
  
    }

    IEnumerator PlayerSkillAlly(Skill skill)
    {
        BattleManager.Instance.usingSkill = false;
        if (selectedSkill.power != 0)
        {
            BattleManager.Instance.target.GetComponent<Unit>().TakeDamage(selectedSkill.power,selectedSkill.category,selectedSkill.element);
        }
        skill.ApplyEffects(TurnOrderManager.Instance.turnPlayer,BattleManager.Instance.target.GetComponent<Unit>());
        dialogueText.text =  TurnOrderManager.Instance.turnPlayer.unitName + " used " + skill.name + " On " + BattleManager.Instance.target.name;
        yield return new WaitForSeconds(2f);
        TurnOrderManager.Instance.turnOrder[0].turnShift = skill.turnShift;
        dialogueText.text = "";
        BattleManager.Instance.actionMenu.SetActive(true);
        ButtonsOn();
        BattleManager.Instance.skillMenu.SetActive(false);
        selectedSkill = null;
        TurnOrderManager.Instance.EndTurn();
  
    }

    IEnumerator PlayerMultSkill(Skill skill, List<Unit> targets)
    {
        BattleManager.Instance.usingSkill = false;
        dialogueText.text = TurnOrderManager.Instance.turnPlayer.unitName + " used " + skill.name + " On" ;
        for (int i = 0; i < targets.Count; i++)
        {
            dialogueText.text += " " + targets[i].unitName;
            if (selectedSkill.power != 0)
            {
                targets[i].TakeDamage(selectedSkill.power, selectedSkill.category, selectedSkill.element);
            }
           
            // Remeber to cross this bridge (self buff multiple times)
            skill.ApplyEffects(TurnOrderManager.Instance.turnPlayer,targets[i]);
        }
        yield return new WaitForSeconds(2f);
        TurnOrderManager.Instance.turnOrder[0].turnShift = skill.turnShift;
        dialogueText.text = "";
        BattleManager.Instance.actionMenu.SetActive(true);
        ButtonsOn();
        BattleManager.Instance.skillMenu.SetActive(false);
        selectedSkill = null;
        TurnOrderManager.Instance.EndTurn();
    }

    IEnumerator PlayerMultSkillParty(Skill skill, List<Unit> targets)
    {
        BattleManager.Instance.usingSkill = false;
        dialogueText.text = TurnOrderManager.Instance.turnPlayer.unitName + " used " + skill.name + " On" ;
        for (int i = 0; i < targets.Count; i++)
        {
            dialogueText.text += " " + targets[i].unitName;
            if (selectedSkill.power != 0)
            {
                targets[i].TakeDamage(selectedSkill.power, selectedSkill.category, selectedSkill.element);
            }
           
            // Remeber to cross this bridge (self buff multiple times)
            skill.ApplyEffects(TurnOrderManager.Instance.turnPlayer,targets[i]);
        }
        yield return new WaitForSeconds(2f);
        TurnOrderManager.Instance.turnOrder[0].turnShift = skill.turnShift;
        dialogueText.text = "";
        BattleManager.Instance.actionMenu.SetActive(true);
        ButtonsOn();
        BattleManager.Instance.skillMenu.SetActive(false);
        selectedSkill = null;
        TurnOrderManager.Instance.EndTurn();
    }


    IEnumerator PlayerSkillSelf(Skill skill)
    {
        BattleManager.Instance.usingSkill = false;
        skill.ApplyEffects(TurnOrderManager.Instance.turnPlayer, TurnOrderManager.Instance.turnPlayer);
        dialogueText.text = TurnOrderManager.Instance.turnPlayer.unitName + " used " + skill.name;
        yield return new WaitForSeconds(2f);
         TurnOrderManager.Instance.turnOrder[0].turnShift = skill.turnShift;
        dialogueText.text = "";
        BattleManager.Instance.actionMenu.SetActive(true);
        ButtonsOn();
        BattleManager.Instance.skillMenu.SetActive(false);
        selectedSkill = null;
        TurnOrderManager.Instance.EndTurn();
    }

    public void ButtonsOn()
    {
        foreach (var obj in skillButtons)
            obj.SetActive(true);
    }

    public void ButtonsOff()
    {
        foreach (var obj in skillButtons)
            obj.SetActive(false);
    }

    public void SkillUse(GameObject button)
    {
        selectedSkill = SkillMaker.Instance.GetById(button.GetComponent<ToolTipSkill>().skillId);
        if (TurnOrderManager.Instance.turnPlayer.AP >= selectedSkill.cost)
        {



            if (selectedSkill.target == Target.single)
            {

                toolTip.text = "";
                ButtonsOff();
                BattleManager.Instance.selecting = true;
                BattleManager.Instance.targetArrow.SetActive(true);
            }

            if (selectedSkill.target == Target.mutipleEnemy)
            {
                toolTip.text = "";
                ButtonsOff();
                BattleManager.Instance.multiTarget = true;

            }

            if (selectedSkill.target == Target.self)
            {
                toolTip.text = "";
                ButtonsOff();
                BattleManager.Instance.targetSelf = true;
            }

            if (selectedSkill.target == Target.ally)
            {
                toolTip.text = "";
                ButtonsOff();
                BattleManager.Instance.playerSelecting = true;
            }

            if (selectedSkill.target == Target.party)
            {
                toolTip.text = "";
                ButtonsOff();
                BattleManager.Instance.targetParty = true;
            }
        }
        else
        {
            toolTip.text = "Not Enough AP";
        }
    }
}

