using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SkillButtonScript : MonoBehaviour
{
    public List<GameObject> skillButtons;

    public TMP_Text toolTip;

    public int page = 0;

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
                skillButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = TurnOrderManager.Instance.turnPlayer.skills[i + page].ToString();
                skillButtons[i].GetComponent<ToolTipSkill>().skillId = TurnOrderManager.Instance.turnPlayer.skills[i + page];
            }
        }
    }
}

