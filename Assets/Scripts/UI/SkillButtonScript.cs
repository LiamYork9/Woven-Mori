using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using MoriSkills;
using UnityEngine.Rendering;



public class SkillButtonScript : MonoBehaviour
{
    public List<GameObject> skillButtons;

    public TMP_Text toolTip;
    
    public TMP_Text currentAP;
    
    public TMP_Text skillCost;
    
    public TMP_Text turnShift;

    public int page = 0;

    public TextMeshProUGUI dialogueText;

    public Skill selectedSkill;

    public GameObject buttonPrefab;

    public GameObject buttonParent;




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

    private void OnEnable()
    {
        skillButtons.Clear();
        foreach (Transform child in buttonParent.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        for(int i = 0; i < TurnOrderManager.Instance.turnPlayer.unit.skills.Count; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonParent.transform);
            newButton.GetComponent<Button>().onClick.AddListener(()=> SkillUse(newButton));
            skillButtons.Add(newButton);
            
        }

        for (int i = 0; i < skillButtons.Count; i++)
        {
            if (skillButtons[i].GetComponent<ToolTipSkill>() != null)
            {
                skillButtons[i].GetComponent<ToolTipSkill>().hoverEvent.AddListener(ToolTipAdder);
                skillButtons[i].GetComponent<ToolTipSkill>().unHoverEvent.AddListener(ToolTipRemover);
            }
        }
        currentAP.text = "Current AP: " + TurnOrderManager.Instance.turnPlayer.AP;
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
                List<UnitBody> temp = new List<UnitBody> { };
                for (int i = 0; i < BattleManager.Instance.enemySlots.Count; i++)
                {
                    temp.Add(BattleManager.Instance.enemySlots[i].GetComponent<UnitBody>());
                }
                ActivateMultiSkill(selectedSkill, temp);
                BattleManager.Instance.multiTarget = false;
            }

            if (BattleManager.Instance.usingSkill == true && BattleManager.Instance.targetParty == true)
            {
                List<UnitBody> temp = new List<UnitBody> { };
                for (int i = 0; i < BattleManager.Instance.playerSlots.Count; i++)
                {
                    temp.Add(BattleManager.Instance.playerSlots[i].GetComponent<UnitBody>());
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
        Skill temp = button.GetComponent<ToolTipSkill>().skill;
        toolTip.text = temp.toolTip;
        turnShift.text = "TurnShift: " + temp.turnShift;
        skillCost.text = "Cost: " + temp.cost;
    }
    public void ToolTipRemover()
    {
        toolTip.text = " ";
        turnShift.text =  " ";
        skillCost.text = " ";
    }

    public void SetSkillButtons()
    {
        for (int i = 0; i < skillButtons.Count; i++)
        {
            if (TurnOrderManager.Instance.turnPlayer != null && TurnOrderManager.Instance.turnPlayer.partyMember == true)
            {
                if ((i + page) < TurnOrderManager.Instance.turnPlayer.skills.Count && TurnOrderManager.Instance.turnPlayer.skills[i + page] != SkillId.None)
                {
                    skillButtons[i].GetComponent<ToolTipSkill>().skill = SkillMaker.Instance.GetById(TurnOrderManager.Instance.turnPlayer.skills[i + page]);
                    skillButtons[i].GetComponent<ToolTipSkill>().skillId = skillButtons[i].GetComponent<ToolTipSkill>().skill.skillId;
                    skillButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = TurnOrderManager.Instance.turnPlayer.skills[i + page].ToString();
                    
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
                NullText();
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
                NullText();
                TurnOrderManager.Instance.turnPlayer.AP -= skill.cost;
            }

        



    }

    public void ActivateSkillSelf(Skill skill)
    {
        BattleManager.Instance.ButtonsOff();
        ButtonsOff();

        StartCoroutine(PlayerSkillSelf(skill));
        NullText();
        TurnOrderManager.Instance.turnPlayer.AP -= skill.cost;


    }

    public void ActivateMultiSkill(Skill skill, List<UnitBody> targets)
    {
        BattleManager.Instance.ButtonsOff();
       
        ButtonsOff();
        if (BattleManager.Instance.selecting == false)
        {
            StartCoroutine(PlayerMultSkill(skill, targets));
            NullText();
            TurnOrderManager.Instance.turnPlayer.AP -= skill.cost;
        }
    }

    public void ActivateMultiSkillParty(Skill skill, List<UnitBody> targets)
    {
        BattleManager.Instance.ButtonsOff();
       
        ButtonsOff();
        if (BattleManager.Instance.selecting == false)
        {
            StartCoroutine(PlayerMultSkill(skill, targets));
            NullText();
            TurnOrderManager.Instance.turnPlayer.AP -= skill.cost;
        }
    }

    IEnumerator PlayerSkill(Skill skill)
    {
        BattleManager.Instance.usingSkill = false;
        skill.ApplyEffects(TurnOrderManager.Instance.turnPlayer,BattleManager.Instance.target.GetComponent<UnitBody>());
        dialogueText.text =  TurnOrderManager.Instance.turnPlayer.name + " used " + skill.name + " On " + BattleManager.Instance.target.name;
        yield return new WaitForSeconds(2f);
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
        skill.ApplyEffects(TurnOrderManager.Instance.turnPlayer,BattleManager.Instance.target.GetComponent<UnitBody>());
        dialogueText.text =  TurnOrderManager.Instance.turnPlayer.name + " used " + skill.name + " On " + BattleManager.Instance.target.name;
        yield return new WaitForSeconds(2f);
        dialogueText.text = "";
        BattleManager.Instance.actionMenu.SetActive(true);
        ButtonsOn();
        BattleManager.Instance.skillMenu.SetActive(false);
        selectedSkill = null;
        TurnOrderManager.Instance.EndTurn();
  
    }

    IEnumerator PlayerMultSkill(Skill skill, List<UnitBody> targets)
    {
        BattleManager.Instance.usingSkill = false;
        dialogueText.text = TurnOrderManager.Instance.turnPlayer.name + " used " + skill.name + " On" ;
        for (int i = 0; i < targets.Count; i++)
        {
            dialogueText.text += " " + targets[i].name;
           
            // Remeber to cross this bridge (self buff multiple times)
            skill.ApplyEffects(TurnOrderManager.Instance.turnPlayer,targets[i]);
        }
        yield return new WaitForSeconds(2f);
        dialogueText.text = "";
        BattleManager.Instance.actionMenu.SetActive(true);
        ButtonsOn();
        BattleManager.Instance.skillMenu.SetActive(false);
        selectedSkill = null;
        TurnOrderManager.Instance.EndTurn();
    }

    IEnumerator PlayerMultSkillParty(Skill skill, List<UnitBody> targets)
    {
        BattleManager.Instance.usingSkill = false;
        dialogueText.text = TurnOrderManager.Instance.turnPlayer.name + " used " + skill.name + " On" ;
        for (int i = 0; i < targets.Count; i++)
        {
            dialogueText.text += " " + targets[i].name;
           
            // Remeber to cross this bridge (self buff multiple times)
            skill.ApplyEffects(TurnOrderManager.Instance.turnPlayer,targets[i]);
        }
        yield return new WaitForSeconds(2f);
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
        dialogueText.text = TurnOrderManager.Instance.turnPlayer.name + " used " + skill.name;
        yield return new WaitForSeconds(2f);
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
        selectedSkill = button.GetComponent<ToolTipSkill>().skill;
        if (TurnOrderManager.Instance.turnPlayer.AP >= selectedSkill.cost)
        {
            if (selectedSkill.target == Target.single)
            {
                NullText();
                ButtonsOff();
                BattleManager.Instance.selecting = true;
                BattleManager.Instance.targetArrow.SetActive(true);
            }

            if (selectedSkill.target == Target.mutipleEnemy)
            {
                NullText();
                ButtonsOff();
                BattleManager.Instance.multiTarget = true;

            }

            if (selectedSkill.target == Target.self)
            {
                NullText();
                ButtonsOff();
                BattleManager.Instance.targetSelf = true;
            }

            if (selectedSkill.target == Target.ally)
            {
                NullText();
                ButtonsOff();
                BattleManager.Instance.playerSelecting = true;
            }

            if (selectedSkill.target == Target.party)
            {
                NullText();
                ButtonsOff();
                BattleManager.Instance.targetParty = true;
            }
        }
        else
        {
            toolTip.text = "Not Enough AP";
        }
    }

    public void NullText()
    {
        currentAP.text = "";
        toolTip.text = "";
        turnShift.text =  "";
        skillCost.text = "";
    }
}

