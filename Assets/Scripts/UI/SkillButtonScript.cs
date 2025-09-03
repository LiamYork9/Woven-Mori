using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using MoriSkills;

public class SkillButtonScript : MonoBehaviour
{
    public List<GameObject> skillButtons;

    public TMP_Text toolTip;

    public int page = 0;

    public TextMeshProUGUI dialogueText;

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
                if ( (i + page) < TurnOrderManager.Instance.turnPlayer.skills.Count && TurnOrderManager.Instance.turnPlayer.skills[i + page] != SkillId.None)
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

    public void ActivateSkill(GameObject button)
    {
       
        BattleManager.Instance.ButtonsOff();
        if (TurnOrderManager.Instance.turnPlayer.AP >= SkillMaker.Instance.GetById(button.GetComponent<ToolTipSkill>().skillId).cost)
        {
            ButtonsOff();
            StartCoroutine(PlayerSkill(button));
            toolTip.text = "";
            TurnOrderManager.Instance.turnPlayer.AP -= SkillMaker.Instance.GetById(button.GetComponent<ToolTipSkill>().skillId).cost;
        }
        else
        {
            toolTip.text = "Not Enough AP";
        }
       
        
       
    }

    IEnumerator PlayerSkill(GameObject button)
    {
        BattleManager.Instance.usingSkill = false;
        dialogueText.text = " Player used " + SkillMaker.Instance.GetById(button.GetComponent<ToolTipSkill>().skillId).name;
        yield return new WaitForSeconds(2f);
        TurnOrderManager.Instance.TurnShift(SkillMaker.Instance.GetById(button.GetComponent<ToolTipSkill>().skillId).turnShift);
        yield return new WaitForSeconds(1f);
        dialogueText.text = " It is now " + TurnOrderManager.Instance.turnPlayer.unitName + "Turn";
        yield return new WaitForSeconds(1f);
        dialogueText.text = "";
        BattleManager.Instance.actionMenu.SetActive(true);
        BattleManager.Instance.ButtonsOn();
        ButtonsOn();
        BattleManager.Instance.skillMenu.SetActive(false);
        BattleManager.Instance.playerTurn = false;
        TurnOrderManager.Instance.turnOrder[0].EndTurn();
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
}

